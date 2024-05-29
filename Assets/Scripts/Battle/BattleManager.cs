using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Net.WebSockets;
using UnityEngine;

public enum ETarget
{
    None,
    P0,
    E0
}

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private PokemonTrainer PlayerTrainer;
    [SerializeField]
    private PokemonTrainer EnemyTrainer;
    private List<Event> EventsList;
    private List<List<Event>> EventsListHistory;
    private ETimePoint CurrentTimePoint;

    [SerializeField]
    private List<BattlePokemon> BattlePokemonList;

    private List<BattlePokemon> DefeatedPokemonList;
    
    [SerializeField]
    private BaseSkill TestSkill;

    private List<EventAnimationPlayer> AnimationEventList = new List<EventAnimationPlayer>();

    public BattleUI BattleUIManager;

    private bool PlayingAnimation;
    private int CurPlayingAnimationEvent;
    private bool WaitForPlayerSwitchPokemonWhenDefeated;
    private bool WaitForPlayerSwitchPokemonAfterSkillUse;
    private bool WaitForEnemySwitchPokemonAfterSkillUse;
    private int EndAnimEventIndex;
    private bool BattleEnd = false;
    private bool Win = false;

    private BattleFieldState FieldState;
    private int TurnIndex;

    private List<List<BattleFieldStatus>> BattleFieldStatusLists;
    public BaseSkill StruggleSkill;
    public BaseSkill ConfusionSkill;
    public BattleMenuUI BeforeBattleUI;
    public static BattleManager StaticManager;
    private int ChapterIndex;
    private bool IsFirstBattle;
    public ChapterUI ReferenceChapterUI;
    public FadeUI TransitionUI;
    public GameObject MapObj;
    public AudioController ReferenceBGM;
    private BaseSpecialRule CurrentSpecialRule;
    // Start is called before the first frame update
    void Start()
    {
        StaticManager = this;
        EventsList = new List<Event>();
        CurrentTimePoint = ETimePoint.None;
        DefeatedPokemonList = new List<BattlePokemon>();
        PlayingAnimation = false;
        EventsListHistory = new List<List<Event>>();
        BattleFieldStatusLists = new List<List<BattleFieldStatus>>();
        BattleFieldStatusLists.Add(new List<BattleFieldStatus>());
        BattleFieldStatusLists.Add(new List<BattleFieldStatus>());
    }

    void Update()
    {
        if(PlayingAnimation)
        {
            if(GetComponent<BattleCameraSwitcher>().enabled)
            {
                GetComponent<BattleCameraSwitcher>().DisableSwitching();
            }
            if(AnimationEventList.Count == 0)
            {
                PlayingAnimation = false;
                AnimationEventList.Clear();
                return;
            }
            BattleUIManager.DisableCommandUI();
            if(CurPlayingAnimationEvent == -1)
            {
                CurPlayingAnimationEvent = 0;
                AnimationEventList[CurPlayingAnimationEvent].BeginPlay();
            }
            AnimationEventList[CurPlayingAnimationEvent].Play();
            if(AnimationEventList[CurPlayingAnimationEvent].Finished())
            {
                CurPlayingAnimationEvent =  CurPlayingAnimationEvent + 1;
                if(CurPlayingAnimationEvent >= AnimationEventList.Count)
                {
                    PlayingAnimation = false;
                    AnimationEventList.Clear();
                    if(GetBattleEnd())
                    {
                        EndBattle(Win);
                        return;
                    }
                    if(WaitForPlayerSwitchPokemonAfterSkillUse)
                    {
                        UpdateUI(true);
                        BattleUIManager.EnableCommandUI();
                    }
                    else if(WaitForEnemySwitchPokemonAfterSkillUse)
                    {
                        UpdateUI(false);
                        EnemyAI NewEnemyAI = new EnemyAI(null, this, EnemyTrainer);
                        BattlePokemon EnemyNext = NewEnemyAI.GetNextPokemon(BattlePokemonList[1]);
                        EventsList.Add(new SwitchEvent(this, BattlePokemonList[1], EnemyNext));
                        WaitForEnemySwitchPokemonAfterSkillUse = false;
                        ProcessEvents(true);
                    }
                    else if(DefeatedPokemonList.Count > 0)
                    {
                        // Currently Only SingleBattle
                        if(DefeatedPokemonList.Count == 1 && DefeatedPokemonList[0].GetIsEnemy())
                        {
                            UpdateUI(false);
                            EnemyAI NewEnemyAI = new EnemyAI(null, this, EnemyTrainer);
                            BattlePokemon EnemyNext = NewEnemyAI.GetNextPokemon(DefeatedPokemonList[0]);
                            EventsList.Add(new SwitchWhenDefeatedEvent(
                                this,
                                DefeatedPokemonList[0], 
                                EnemyNext, 
                                null,
                                null
                                ));
                            DefeatedPokemonList.Clear();
                            ProcessEvents(false);
                        }
                        else if(DefeatedPokemonList.Count == 1 && !DefeatedPokemonList[0].GetIsEnemy())
                        {
                            WaitForPlayerSwitchPokemonWhenDefeated = true;
                            UpdateUI(true);
                            BattleUIManager.EnableCommandUI();
                        }
                        else if(DefeatedPokemonList.Count == 2)
                        {
                            WaitForPlayerSwitchPokemonWhenDefeated = true;
                            UpdateUI(true);
                            BattleUIManager.EnableCommandUI();
                        }
                    }
                    else
                    {
                        UpdateUI(false);
                        BattleUIManager.EnableCommandUI();
                        GetComponent<BattleCameraSwitcher>().EnableSwitching();
                    }
                }
                else
                {
                    AnimationEventList[CurPlayingAnimationEvent].BeginPlay();
                }
            }
        }
    }

    public void AddAnimationEvent(EventAnimationPlayer InEvent)
    {
        AnimationEventList.Add(InEvent);
    }
    public void UpdatePokemonInfo(BattlePokemon InPokemon, BattlePokemonStat Stats)
    {
        if(InPokemon == null)
            return;
        if(Stats.StatusChangeList != null)
        {
            for(int Index = 0; Index < Stats.StatusChangeList.Count; Index++)
            {
                if(StatusChange.IsStatusChange(Stats.StatusChangeList[Index].StatusChangeType))
                {
                    if(InPokemon.GetIsEnemy())
                    {
                        BattleUIManager.SetEnemyStateChange(Stats.StatusChangeList[Index].StatusChangeType);
                    }
                    else
                    {
                        BattleUIManager.SetPlayerStateChange(Stats.StatusChangeList[Index].StatusChangeType);
                    }
                    break;
                }
            }
        }
        if(InPokemon.GetIsEnemy())
        {
            BattleUIManager.SetEnemyHP(Stats.HP);
        }
        else
        {
            BattleUIManager.SetPlayerHP(Stats.HP);
        }
    }
    public void UpdatePokemonStatusChange(BattlePokemon InPokemon, BattlePokemonStat Stats)
    {
        if(InPokemon == null)
            return;
        if(InPokemon.GetIsEnemy())
        {
            BattleUIManager.SetEnemyStateChange(EStatusChange.None);
        }
        else
        {
            BattleUIManager.SetPlayerStateChange(EStatusChange.None);
        }
        if(Stats.StatusChangeList != null)
        {
            for(int Index = 0; Index < Stats.StatusChangeList.Count; Index++)
            {
                if(StatusChange.IsStatusChange(Stats.StatusChangeList[Index].StatusChangeType))
                {
                    if(InPokemon.GetIsEnemy())
                    {
                        BattleUIManager.SetEnemyStateChange(Stats.StatusChangeList[Index].StatusChangeType);
                    }
                    else
                    {
                        BattleUIManager.SetPlayerStateChange(Stats.StatusChangeList[Index].StatusChangeType);
                    }
                    break;
                }
            }
        }
    }

    public void UpdatePlayerType()
    {
        BattleUIManager.UpdatePlayerType(BattlePokemonList[0]);
    }
    public void UpdateEnemyType()
    {
        BattleUIManager.UpdateEnemyType(BattlePokemonList[1 ]);
    }

    public void UpdatePlayerUI()
    {
        BattleUIManager.SetCurrentPlayerTrainer(PlayerTrainer);
        BattleUIManager.SetCurrentBattlePokemon(BattlePokemonList[0]);
        BattleUIManager.UpdatePlayer1UI(BattlePokemonList[0], PlayerTrainer);
    }

    public void UpdateEnemyUI()
    {
        BattleUIManager.UpdateEnemy1UI(BattlePokemonList[1], EnemyTrainer);
    }
    public void UpdateUI(bool SwitchCommand)
    {
        BattleUIManager.SetCurrentPlayerTrainer(PlayerTrainer);
        BattleUIManager.UpdatePlayer1UI(BattlePokemonList[0], PlayerTrainer);
        BattleUIManager.UpdateEnemy1UI(BattlePokemonList[1], EnemyTrainer);
        BattleUIManager.SetCurrentBattlePokemon(BattlePokemonList[0]);
        if(SwitchCommand)
        {
            BattleUIManager.GenerateSwitch();
        }
        else
        {
            BattleUIManager.GenerateSkills();
        }
    }

    public void AddDefeatedPokemon(BattlePokemon InPokemon)
    {
        DefeatedPokemonList.Add(InPokemon);
    }
    
    public void AddSkillEvent(BattleSkill InSkill, BattlePokemon SourcePokemon, ETarget TargetPokemon)
    {
        List<ETarget> TargetList = new List<ETarget>();
        TargetList.Add(TargetPokemon);
        EventsList.Add(new SkillEvent(this, InSkill, SourcePokemon, TargetList));
    }

    public void AddSwitchEvent(BattlePokemon OutPokemon, BattlePokemon InPokemon)
    {
        EventsList.Add(new SwitchEvent(this, OutPokemon, InPokemon));
    }

    public void TranslateTimePoint(ETimePoint NewTime, Event SourceEvent)
    {
        CurrentTimePoint = NewTime;

        List<BaseAbility> AbilitiesToTrigger = this.QueryAbilitiesWhenTimeChange(SourceEvent);
        AbilitiesToTrigger.Sort(new AbilityComparer());
        foreach(var AbilityIter in AbilitiesToTrigger)
        {
            if(BattlePokemon.IsSpecialAbility(AbilityIter.GetAbilityName()))
            {
                if(SourceEvent.GetEventType() == EventType.UseSkill)
                {
                    SkillEvent CastedEvent = (SkillEvent)SourceEvent;
                    if(CastedEvent.GetSourcePokemon().GetAbility().GetAbilityName() == "破格")
                    {
                        continue;
                    }
                }
            }
            AbilityIter.SetIsProcessing(true);
            string name = AbilityIter.GetAbilityName();
            List<Event> EventsToProcess = AbilityIter.Trigger(this, SourceEvent);
            AbilityTriggerEvent AbilityEvent = new AbilityTriggerEvent(EventsToProcess, AbilityIter);
            AbilityEvent.Process(this);
        }

        foreach(var AbilityIter in AbilitiesToTrigger)
        {
            AbilityIter.SetIsProcessing(false);
        }

        CurrentTimePoint = NewTime;           
        List<BattleItem> ItemsToTrigger = this.QueryItemsWhenTimeChange(SourceEvent);
        ItemsToTrigger.Sort(new BattleItemComparer());
        foreach(var ItemIter in ItemsToTrigger)
        {
            List<Event> EventsToProcess = ItemIter.Trigger(this, SourceEvent);
            ItemTriggerEvent ItemEvent = new ItemTriggerEvent(EventsToProcess, ItemIter);
            ItemEvent.Process(this);
        }

        CurrentTimePoint = NewTime;
        List<BaseStatusChange> BastStatusChangesToTrigger = this.QueryBaseStatusChangesWhenTimeChange(SourceEvent);
        BastStatusChangesToTrigger.Sort(new BaseStatusChangeComparer());
        foreach(var BaseStatusChangeIter in BastStatusChangesToTrigger)
        {
            List<Event> EventsToProcess = BaseStatusChangeIter.Trigger(this, SourceEvent);
            foreach(var EventIter in EventsToProcess)
            {
                EventIter.Process(this);
            }
        }

        CurrentTimePoint = NewTime;
        List<BattleFieldStatus> FieldStatusList =  this.QueryBattleFieldStatusWhenTimeChange(SourceEvent);
        foreach(var FieldStatus in FieldStatusList)
        {
            List<Event> EventsToProcess = FieldStatus.BaseStatusChange.Trigger(this, SourceEvent);
            foreach(var EventIter in EventsToProcess)
            {
                EventIter.Process(this);
            }
        }

        CurrentTimePoint = NewTime;
        if(PlayerTrainer.TrainerSkill && PlayerTrainer.TrainerSkill.ShouldTrigger(CurrentTimePoint, SourceEvent))
        {
            List<Event> EventsToProcess = PlayerTrainer.TrainerSkill.Trigger(this, SourceEvent);
            TrainerSkillTriggerEvent TrainerSkillEvent = new TrainerSkillTriggerEvent(EventsToProcess, PlayerTrainer.TrainerSkill);
            TrainerSkillEvent.Process(this);
        }

        CurrentTimePoint = NewTime;
        if(EnemyTrainer.TrainerSkill && EnemyTrainer.TrainerSkill.ShouldTrigger(CurrentTimePoint, SourceEvent))
        {
            List<Event> EventsToProcess = EnemyTrainer.TrainerSkill.Trigger(this, SourceEvent);
            TrainerSkillTriggerEvent TrainerSkillEvent = new TrainerSkillTriggerEvent(EventsToProcess, EnemyTrainer.TrainerSkill);
            TrainerSkillEvent.Process(this);
        }
    }

    public void ProcessEvents(bool NewTurn)
    {
        List<Event> TurnEvents = new List<Event>();
        EndAnimEventIndex = 0;
        while(EventsList.Count > 0)
        {
            EventsList.Sort(new EventComparer());
            if(EventsList[0].ShouldProcess(this))
            {
                EventsList[0].Process(this);
            }
            EndAnimEventIndex++;
            bool WaitForSwitchAfterUseSKill = (WaitForEnemySwitchPokemonAfterSkillUse || WaitForPlayerSwitchPokemonAfterSkillUse);
            TurnEvents.Add(EventsList[0]);
            EventsList.RemoveAt(0);
            if(WaitForSwitchAfterUseSKill)
            {
                PlayingAnimation = true;
                CurPlayingAnimationEvent = -1;
                return;
            }
        }
        EventsList.Clear();
        if(NewTurn)
        {
            TurnEndEvent TurnEnd = new TurnEndEvent(this);
            TurnEnd.Process(this);
            TurnEvents.Add(TurnEnd);
            EventsListHistory.Add(TurnEvents);
            TurnIndex = TurnIndex + 1;
        }
        else
        {
            if(EventsListHistory.Count > 0)
            {
                EventsListHistory[EventsListHistory.Count - 1].AddRange(TurnEvents);
            }
        }
        PlayingAnimation = true;
        CurPlayingAnimationEvent = -1;
    }

    public void BeginSingleBattle(BattlePokemon PlayerPokemon, BattlePokemon EnemyPokemon)
    {
        EventsList.Add(new SingleBattleGameStartEvent(PlayerPokemon, EnemyPokemon));
        TurnIndex = 0;
        EventsListHistory.Clear();
        BattleFieldStatusLists[0].Clear();
        BattleFieldStatusLists[1].Clear();
        this.Win = false;
        FieldState.WeatherType = EWeather.None;
        FieldState.WeatherRemainTime = 0;
        FieldState.FieldTerrain = EBattleFieldTerrain.None;
        FieldState.TerrainRemainTime = 0;
        FieldState.IsTrickRoomActive = false;
        FieldState.TrickRoomRemainTime = 0;
    }

    public List<BaseAbility> QueryAbilitiesWhenTimeChange(Event SourceEvent)
    {
        List<BaseAbility> AbilitiesToTrigger = new List<BaseAbility>();
        foreach(var BattlePokemonIter in BattlePokemonList)
        {
            BaseAbility CurrentAbility = BattlePokemonIter.GetAbility();
            if(CurrentAbility && CurrentAbility.GetIsProcessing() == false && CurrentAbility.ShouldTrigger(CurrentTimePoint, SourceEvent))
            {
                AbilitiesToTrigger.Add(CurrentAbility);
            }
        }
        return AbilitiesToTrigger;
    }

    public List<BaseStatusChange> QueryBaseStatusChangesWhenTimeChange(Event SourceEvent)
    {
        List<BaseStatusChange> BaseStatusChangesToTrigger = new List<BaseStatusChange>();
        foreach(var BattlePokemonIter in BattlePokemonList)
        {
            List<BaseStatusChange> PokemonStatusChanges = BattlePokemonIter.GetAllStatusChange();
            foreach(var PokemonStatusChange in PokemonStatusChanges)
            {   
                if(PokemonStatusChange.ShouldTrigger(CurrentTimePoint, SourceEvent))
                {
                    BaseStatusChangesToTrigger.Add(PokemonStatusChange);
                }
            }            
        }
        return BaseStatusChangesToTrigger;
    }

    public List<BattleFieldStatus> QueryBattleFieldStatusWhenTimeChange(Event SourceEvent)
    {
        List<BattleFieldStatus> Result = new List<BattleFieldStatus>();
        foreach(var StatusList  in BattleFieldStatusLists)
        {
            foreach(var Status in StatusList)
            {
                if(Status.BaseStatusChange != null && Status.BaseStatusChange.ShouldTrigger(CurrentTimePoint, SourceEvent))
                {
                    Result.Add(Status);
                }
            }
        }
        return Result;
    }

    public List<BattleItem> QueryItemsWhenTimeChange(Event SourceEvent)
    {
        List<BattleItem> ItemsToTrigger = new List<BattleItem>();
        foreach(var BattlePokemonIter in BattlePokemonList)
        {
            BattleItem CurrentItem = BattlePokemonIter.GetBattleItem();
            if(CurrentItem != null && CurrentItem.ShouldTrigger(CurrentTimePoint, SourceEvent))
            {
                ItemsToTrigger.Add(CurrentItem);
            }
        }
        return ItemsToTrigger;
    }

    public void ShowBeforeBattleMenu(string BattleName, int Index)
    {
        GameObject BattleConfigObj = GameObject.Find("BattleConfig/" + BattleName);
        PlayerTrainer = BattleConfigObj.GetComponent<BattleConfig>().PlayerTrainer;
        BeforeBattleUI.gameObject.SetActive(true);
        if(Index == 0)
        {
            BeforeBattleUI.SetEnemyPokemonTrainer(BattleConfigObj.GetComponent<BattleConfig>().EnemyTrainer1);
            BeforeBattleUI.SetSpecialRule(BattleConfigObj.GetComponent<BattleConfig>().SpecialRule1);
        }
        else if(Index == 1)
        {
            BeforeBattleUI.SetEnemyPokemonTrainer(BattleConfigObj.GetComponent<BattleConfig>().EnemyTrainer2);
            BeforeBattleUI.SetSpecialRule(BattleConfigObj.GetComponent<BattleConfig>().SpecialRule2);
        }
        BeforeBattleUI.SetPlayerPokemonTrainer(PlayerTrainer);
    }

    public void SetSpecialRule(BaseSpecialRule InRule)
    {
        CurrentSpecialRule = InRule;
    }

    public bool HasSpecialRule(string SpecialRuleName)
    {
        return CurrentSpecialRule && CurrentSpecialRule.Name == SpecialRuleName;
    }

    public void SetPlayerTrainer(PokemonTrainer InTrainer)
    {
        PlayerTrainer = InTrainer;
    }   

    public void SetEnemyTrainer(PokemonTrainer InTrainer)
    {
        EnemyTrainer = InTrainer;
    }

    public void EndBattle(bool Win)
    {
        GameObject SavedDataObj = GameObject.Find("SavedData");
        if(Win)
        {
            EProgress CurProgress = SavedDataObj.GetComponent<SavedData>().SavedPlayerData.MainChapterProgress[ChapterIndex];
            if(IsFirstBattle)
            {
                if(ChapterIndex == 0)
                {
                    SavedDataObj.GetComponent<SavedData>().SavedPlayerData.MainChapterProgress[ChapterIndex] = EProgress.FinishAllBattle;
                }
                else if(CurProgress != EProgress.FinishBattle2)
                {
                    SavedDataObj.GetComponent<SavedData>().SavedPlayerData.MainChapterProgress[ChapterIndex] = EProgress.FinishBattle1;
                }
                else
                {
                    SavedDataObj.GetComponent<SavedData>().SavedPlayerData.MainChapterProgress[ChapterIndex] = EProgress.FinishAllBattle;
                }
            }
            else
            {
                if(CurProgress != EProgress.FinishBattle1)
                {
                    SavedDataObj.GetComponent<SavedData>().SavedPlayerData.MainChapterProgress[ChapterIndex] = EProgress.FinishBattle2;
                }
                else
                {
                    SavedDataObj.GetComponent<SavedData>().SavedPlayerData.MainChapterProgress[ChapterIndex] = EProgress.FinishAllBattle;
                }
            }
            SavedDataObj.GetComponent<SavedData>().SavedPlayerData.UseableTrainerList.Add(EnemyTrainer.TrainerName);
            SavedDataObj.GetComponent<SavedData>().SaveData();
        }

        StartCoroutine(EnableObjectAfterDelay());
        TransitionUI.TransitionAnimation();
        ReferenceBGM.gameObject.SetActive(false);
    }

    IEnumerator EnableObjectAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        MapObj.SetActive(true);
        ReferenceChapterUI.gameObject.SetActive(true);
        BattleUIManager.DisableAllUI();
        ReferenceChapterUI.OnFinishBattle();
        this.GetComponent<BattleInitializer>().ClearAllObjects();
    }

    public void BeginBattle(int FirstPokemonIndex, int ChapterIndex, bool FirstBattle)
    {
        this.ChapterIndex = ChapterIndex;
        IsFirstBattle = FirstBattle;
        GameObject NaniObj = GameObject.Find("Naninovel<Runtime>");
        if(NaniObj)
        {
            NaniObj.SetActive(false);
        }
        this.GetComponent<BattleInitializer>().InitBattleResources(PlayerTrainer, EnemyTrainer);
        BattlePokemonList[0] = PlayerTrainer.BattlePokemons[FirstPokemonIndex];
        BattlePokemonList[1] = EnemyTrainer.BattlePokemons[0];
        UpdateUI(false);
        BeginSingleBattle(BattlePokemonList[0], BattlePokemonList[1]);
        ProcessEvents(false);
    }

    public void OnUseSkill(BaseSkill InSkill, BattlePokemon InReferencePokemon, bool Mega)
    {
        // Currently Only Single Battle.
        BattleSkill UseBattleSkill = new BattleSkill(InSkill, EMasterSkill.None, InReferencePokemon);
        List<ETarget> TargetPokemon = new List<ETarget>();
        List<BattlePokemon> Opposites = GetOpppoitePokemon(UseBattleSkill.GetReferencePokemon());
        if(InSkill.GetSkillRange() != ERange.None)
        {
            TargetPokemon.Add(ETarget.E0);
        }
        else
        {
            TargetPokemon.Add(ETarget.None);
        }
        if(InReferencePokemon.CanMega() && Mega)
        {
            EventsList.Add(new MegaEvent(this, InReferencePokemon));
        }
        EventsList.Add(new SkillEvent(this, UseBattleSkill, UseBattleSkill.GetReferencePokemon(), TargetPokemon));
        EnemyAI NewEnemyAI = new EnemyAI(Opposites[0], this, EnemyTrainer);
        NewEnemyAI.GenerateEnemyEvent(EventsList, this, EventsList[EventsList.Count - 1]);
        BattlePokemonList[0].NewTurn();
        BattlePokemonList[1].NewTurn();
        ProcessEvents(true);
    }

    public List<BattlePokemon> GetOpppoitePokemon(BattlePokemon InPokemon)
    {
        List<BattlePokemon> Result = new List<BattlePokemon>();
        bool IsEnemy = InPokemon.GetIsEnemy();
        foreach(var BattlePokemonIter in BattlePokemonList)
        {
            if(BattlePokemonIter.GetIsEnemy() != IsEnemy)
            {
                Result.Add(BattlePokemonIter);
            }
        }

        return Result;
    }

    public List<BattlePokemon> GetTeammatesPokemon(BattlePokemon InPokemon, bool IncludeSelf)
    {
        List<BattlePokemon> Result = new List<BattlePokemon>();
        bool IsEnemy = InPokemon.GetIsEnemy();
        foreach(var BattlePokemonIter in BattlePokemonList)
        {
            if(BattlePokemonIter.GetIsEnemy() == IsEnemy)
            {
                if(InPokemon == BattlePokemonIter && !IncludeSelf)
                {
                    continue;
                }
                Result.Add(BattlePokemonIter);
            }
        }

        return Result;
    }
    public bool BattleEndIfPokemonDefeated(BattlePokemon InPokemon)
    {
        if(InPokemon.GetIsEnemy())
        {
            return EnemyTrainer.GetRemainPokemonNum() == 0;
        }
        return PlayerTrainer.GetRemainPokemonNum() == 0;
    }
    public bool IsLastPokemon(BattlePokemon InPokemon)
    {
        if(InPokemon.GetIsEnemy())
        {
            return EnemyTrainer.GetRemainPokemonNum() == 1;
        }
        return PlayerTrainer.GetRemainPokemonNum() == 1;
    }

    public PokemonTrainer GetPlayerTrainer()
    {
        return PlayerTrainer;
    }

    public PokemonTrainer GetEnemyTrainer()
    {
        return EnemyTrainer;
    }

    public void SetNewPlayerPokemon(BattlePokemon InPokemon)
    {
        BattlePokemonList[0] = InPokemon;
    }
    public void SetNewEnemyPokemon(BattlePokemon InPokemon)
    {
        BattlePokemonList[1] = InPokemon;
    }

    public void OnPlayerSwitchNewPokemon(BattlePokemon InPokemon)
    {
        if(InPokemon.IsDead() || BattlePokemonList[0] == InPokemon)
        {
            return;
        }
        if(WaitForPlayerSwitchPokemonWhenDefeated)
        {
            UpdateUI(false);
            BattlePokemon PlayerOld = null;
            BattlePokemon PlayerNew = null;
            BattlePokemon EnemyOld = null;
            BattlePokemon EnemyNew = null;
            for(int Index = 0; Index < DefeatedPokemonList.Count; Index++)
            {  
                if(DefeatedPokemonList[Index].GetIsEnemy())
                {
                    EnemyAI NewEnemyAI = new EnemyAI(null, this, EnemyTrainer);
                    EnemyOld = DefeatedPokemonList[Index];
                    EnemyNew = NewEnemyAI.GetNextPokemon(DefeatedPokemonList[Index]);
                }
                else
                {
                    PlayerOld = DefeatedPokemonList[Index];
                    PlayerNew = InPokemon;
                }
            }

            EventsList.Add(
                new SwitchWhenDefeatedEvent(
                this,
                EnemyOld, 
                EnemyNew, 
                PlayerOld,
                PlayerNew
                ));
            DefeatedPokemonList.Clear();
            WaitForPlayerSwitchPokemonWhenDefeated = false;
            ProcessEvents(false);
        }
        else if(WaitForPlayerSwitchPokemonAfterSkillUse)
        {
            EventsList.Add(new SwitchEvent(this, BattlePokemonList[0], InPokemon));
            WaitForPlayerSwitchPokemonAfterSkillUse = false;
            if(WaitForEnemySwitchPokemonAfterSkillUse)
            {
                EnemyAI NewEnemyAI = new EnemyAI(null, this, EnemyTrainer);
                BattlePokemon EnemyNext = NewEnemyAI.GetNextPokemon(BattlePokemonList[1]);
                EventsList.Add(new SwitchEvent(this, BattlePokemonList[1], EnemyNext));
                WaitForEnemySwitchPokemonAfterSkillUse = false;
            }
            ProcessEvents(true);
        }
        else
        {
            EventsList.Add(new SwitchEvent(this, BattlePokemonList[0], InPokemon));
            EnemyAI NewEnemyAI = new EnemyAI(BattlePokemonList[1], this, EnemyTrainer);
            NewEnemyAI.GenerateEnemyEvent(EventsList, this, EventsList[EventsList.Count - 1]);
            BattlePokemonList[0].NewTurn();
            BattlePokemonList[1].NewTurn();
            ProcessEvents(true);
        }
    }

    public BattlePokemon GetTargetPokemon(ETarget Target)
    {
        if(Target == ETarget.P0)
        {
            return BattlePokemonList[0];
        }
        else if(Target == ETarget.E0)
        {
            return BattlePokemonList[1];
        }
        return null;
    }

    public void SetBattleEnd(bool End, bool Win)
    {
        BattleEnd = End;
        this.Win = Win;
    }

    public bool GetBattleEnd()
    {
        return BattleEnd;
    }

    public void SetTerrain(EBattleFieldTerrain TerrainType, int TurnNum)
    {
        FieldState.FieldTerrain = TerrainType;
        FieldState.TerrainRemainTime = TurnNum;
    }

    public EBattleFieldTerrain GetTerrainType()
    {
        return FieldState.FieldTerrain;
    }

    public int GetTerrainRemainTurn()
    {
        return FieldState.TerrainRemainTime;
    }

    public bool ReduceTrickRoomTurn()
    {
        if(FieldState.IsTrickRoomActive == false)
        {
            return false;
        }

        FieldState.TrickRoomRemainTime = FieldState.TrickRoomRemainTime - 1;
        return FieldState.TrickRoomRemainTime == 0;
    }
    public bool ReduceTerrainTurn()
    {
        if(FieldState.FieldTerrain == EBattleFieldTerrain.None)
        {
            return false;
        }
        FieldState.TerrainRemainTime = FieldState.TerrainRemainTime - 1;
        return FieldState.TerrainRemainTime == 0;
    }


    public List<BattlePokemon> GetBattlePokemons()
    {
        return BattlePokemonList;
    }

    public int GetCurrentTurnIndex()
    {
        return TurnIndex;
    }
    public List<BattleSkill> GetPokemonSkillInTurnEffective(BattlePokemon InPokemon, int TurnIndex)
    {
        List<BattleSkill> Result = new List<BattleSkill>();
        foreach(var EventIter in EventsListHistory[TurnIndex])
        {
            if(EventIter.GetEventType() == EventType.UseSkill)
            {
                SkillEvent CastedSkillEvent = (SkillEvent)EventIter;
                if(CastedSkillEvent.GetSourcePokemon() == InPokemon && CastedSkillEvent.IsSkillEffective() == true)
                {
                    Result.Add(CastedSkillEvent.GetSkill());
                }
            }
        }
        return Result;
    }

    public bool IsPokemonInField(BattlePokemon InPokemon)
    {
        return BattlePokemonList[0] == InPokemon || BattlePokemonList[1] == InPokemon;
    }

    public bool IsPokemonUseDamageSkillThisTurn(BattlePokemon InPokemon)
    {
        foreach(var EventIter in EventsList)
        {
            if(EventIter.GetEventType() == EventType.UseSkill)
            {
                SkillEvent CastedEvent = (SkillEvent)EventIter;
                if(CastedEvent.GetSourcePokemon() == InPokemon &&
                    CastedEvent.GetSkill().IsDamageSkill())
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool HasBattleFieldStatus(bool Player, EBattleFieldStatus StatusType)
    {
        int Index = 0;
        if(!Player) Index = 1;
        foreach(var BattleFieldStatus in BattleFieldStatusLists[Index])
        {
            if(BattleFieldStatus.StatusType == StatusType)
            {
                return true;
            }
        }
        return false;
    }

    public BattleFieldStatus AddBattleFieldStatus(BattlePokemon SourcePokemon, bool Player, EBattleFieldStatus StatusType, bool HasLimitedTime, int InTime)
    {
        int Index = 0;
        if(!Player) Index = 1;
        BattleFieldStatus NewStatus = new BattleFieldStatus(SourcePokemon, StatusType, HasLimitedTime, InTime, Player);
        BattleFieldStatusLists[Index].Add(NewStatus);
        return NewStatus;
    }

    public BattleFieldStatus RemoveBattleFieldStatus(bool Player, EBattleFieldStatus StatusType)
    {
        int Index = 0;
        if(!Player) Index = 1;
        BattleFieldStatus Tmp = new BattleFieldStatus(null, EBattleFieldStatus.None, false, 0, false);
        for(int StatusIndex = 0; StatusIndex < BattleFieldStatusLists[Index].Count; StatusIndex++)
        {
            if(BattleFieldStatusLists[Index][StatusIndex].StatusType == StatusType)
            {
                Tmp = BattleFieldStatusLists[Index][StatusIndex];
                BattleFieldStatusLists[Index].RemoveAt(StatusIndex);
                return Tmp;
            }
        }
        return Tmp;
    }

    public List<BattleFieldStatus> GetBattleFieldStatusList(bool IsPlayer)
    {
        int Index = 0;
        if(!IsPlayer) Index = 1;
        return BattleFieldStatusLists[Index];
    }

    public List<EBattleFieldStatus> ReduceBattleFieldTime(bool Player)
    {
        List<EBattleFieldStatus> RemoveStatus = new List<EBattleFieldStatus>();
        int Index = 0;
        if(!Player) Index = 1;
        for(int StatusIndex = 0; StatusIndex < BattleFieldStatusLists[Index].Count; StatusIndex++)
        {
            if(BattleFieldStatusLists[Index][StatusIndex].HasLimitedTime)
            {
                BattleFieldStatus OldStatus = BattleFieldStatusLists[Index][StatusIndex];
                OldStatus.RemainTurn = OldStatus.RemainTurn - 1;
                BattleFieldStatusLists[Index][StatusIndex] = OldStatus;

                if(BattleFieldStatusLists[Index][StatusIndex].RemainTurn == 0)
                {
                    RemoveStatus.Add(BattleFieldStatusLists[Index][StatusIndex].StatusType);
                }
            }
        }

        return RemoveStatus;
    }

    public BaseSkill GetStruggleSkill()
    {
        return StruggleSkill;
    }

    public BaseSkill GetConfusionSkill()
    {
        return ConfusionSkill;
    }

    public bool IsPokemonInLastTurn(BattlePokemon TargetPokemon)
    {
        if(TurnIndex == 0) return false;
        List<Event> LastTurnEvents = EventsListHistory[TurnIndex - 1];
        foreach(var EventIter in LastTurnEvents)
        {
            if(EventIter.GetEventType() == EventType.Switch)
            {
                SwitchEvent CastedEvent = (SwitchEvent)EventIter;
                if(CastedEvent.GetInPokemon() == TargetPokemon)
                {
                    return true;
                }
            }
            else if(EventIter.GetEventType() == EventType.SwitchAfterDefeated)
            {
                SwitchWhenDefeatedEvent CastedEvent = (SwitchWhenDefeatedEvent)EventIter;
                if(CastedEvent.GetPlayerNewPokemon() == TargetPokemon 
                || CastedEvent.GetEnemyNewPokemon() == TargetPokemon)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void SetWeather(EWeather WeatherType, int TurnNum)
    {
        FieldState.WeatherType = WeatherType;
        FieldState.WeatherRemainTime = TurnNum;
    }


    public EWeather GetWeatherType()
    {
        return FieldState.WeatherType;
    }

    public int GetWeatherRemainTurn()
    {
        return FieldState.WeatherRemainTime;
    }
    public bool ReduceWeatherTurn()
    {
        if(FieldState.WeatherType == EWeather.None)
        {
            return false;
        }
        FieldState.WeatherRemainTime = FieldState.WeatherRemainTime - 1;
        return FieldState.WeatherRemainTime == 0;
    }

    public int GetTrickRoomRemainTurn()
    {
        return FieldState.TrickRoomRemainTime;
    }

    public bool GetIsTrickRoomActive()
    {
        return FieldState.IsTrickRoomActive;
    }

    public void BeginTrickRoom(int Turn)
    {
        FieldState.IsTrickRoomActive = true;
        FieldState.TrickRoomRemainTime = Turn;
    }

    public void EndTrickRoom()
    {
        FieldState.IsTrickRoomActive = false;
    }

    public void TestAVG()
    {
        Debug.Log("Test.");
    }

    public void SetWaitForSwitchAfterSkillUse(bool IsPlayer)
    {
        if(IsPlayer == false)
        {
            WaitForEnemySwitchPokemonAfterSkillUse = true;
        }
        else
        {
            WaitForPlayerSwitchPokemonAfterSkillUse = true;
        }
    }
}