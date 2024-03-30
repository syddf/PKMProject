using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
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
    private bool BattleEnd = false;

    private BattleFiledState FiledState;
    // Start is called before the first frame update
    void Start()
    {
        EventsList = new List<Event>();
        CurrentTimePoint = ETimePoint.None;
        DefeatedPokemonList = new List<BattlePokemon>();
        PlayingAnimation = false;
    }

    void Update()
    {
        if(PlayingAnimation)
        {
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
                    if(DefeatedPokemonList.Count > 0)
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
                    }
                    else
                    {
                        UpdateUI(false);
                        BattleUIManager.EnableCommandUI();
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
        if(InPokemon.GetIsEnemy())
        {
            BattleUIManager.SetEnemyHP(Stats.HP);
        }
        else
        {
            BattleUIManager.SetPlayerHP(Stats.HP);
        }
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
            string name = AbilityIter.GetAbilityName();
            List<Event> EventsToProcess = AbilityIter.Trigger(this, SourceEvent);
            AbilityTriggerEvent AbilityEvent = new AbilityTriggerEvent(EventsToProcess, AbilityIter);
            AbilityEvent.Process(this);
        }

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
    }

    public void ProcessEvents(bool NewTurn)
    {
        while(EventsList.Count > 0)
        {
            EventsList.Sort(new EventComparer());
            if(EventsList[0].ShouldProcess(this))
            {
                EventsList[0].Process(this);
            }
            EventsList.RemoveAt(0);
        }
        EventsList.Clear();
        if(NewTurn)
        {
            TurnEndEvent TurnEnd = new TurnEndEvent(this);
            TurnEnd.Process(this);
        }
        PlayingAnimation = true;
        CurPlayingAnimationEvent = -1;
    }

    public void BeginSingleBattle(BattlePokemon PlayerPokemon, BattlePokemon EnemyPokemon)
    {
        EventsList.Add(new SingleBattleGameStartEvent(PlayerPokemon, EnemyPokemon));
    }

    public List<BaseAbility> QueryAbilitiesWhenTimeChange(Event SourceEvent)
    {
        List<BaseAbility> AbilitiesToTrigger = new List<BaseAbility>();
        foreach(var BattlePokemonIter in BattlePokemonList)
        {
            BaseAbility CurrentAbility = BattlePokemonIter.GetAbility();
            if(CurrentAbility && CurrentAbility.ShouldTrigger(CurrentTimePoint, SourceEvent))
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

    public void Test()
    {
        BattlePokemonList[0].LoadBasePokemonStats();
        BattlePokemonList[1].LoadBasePokemonStats();
        UpdateUI(false);
        BeginSingleBattle(BattlePokemonList[0], BattlePokemonList[1]);
        ProcessEvents(false);
    }

    public void OnUseSkill(BaseSkill InSkill, BattlePokemon InReferencePokemon)
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
        EventsList.Add(new SkillEvent(this, UseBattleSkill, UseBattleSkill.GetReferencePokemon(), TargetPokemon));
        EnemyAI NewEnemyAI = new EnemyAI(Opposites[0], this, EnemyTrainer);
        NewEnemyAI.GenerateEnemyEvent(EventsList);
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
        else
        {
            EventsList.Add(new SwitchEvent(this, BattlePokemonList[0], InPokemon));
            EnemyAI NewEnemyAI = new EnemyAI(BattlePokemonList[1], this, EnemyTrainer);
            NewEnemyAI.GenerateEnemyEvent(EventsList);
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

    public void SetBattleEnd(bool End)
    {
        BattleEnd = End;
    }

    public bool GetBattleEnd()
    {
        return BattleEnd;
    }

    public void SetTerrain(EBattleFieldTerrain TerrainType, int TurnNum)
    {
        FiledState.FieldTerrain = TerrainType;
        FiledState.TerrainRemainTime = TurnNum;
    }

    public EBattleFieldTerrain GetTerrainType()
    {
        return FiledState.FieldTerrain;
    }

    public int GetTerrainRemainTurn()
    {
        return FiledState.TerrainRemainTime;
    }
    public bool ReduceTerrainTurn()
    {
        if(FiledState.FieldTerrain == EBattleFieldTerrain.None)
        {
            return false;
        }
        FiledState.TerrainRemainTime = FiledState.TerrainRemainTime - 1;
        return FiledState.TerrainRemainTime == 0;
    }


    public List<BattlePokemon> GetBattlePokemons()
    {
        return BattlePokemonList;
    }
}