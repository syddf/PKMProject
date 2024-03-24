using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                    UpdateUI();
                    if(DefeatedPokemonList.Count > 0)
                    {
                        // Currently Only SingleBattle
                        if(DefeatedPokemonList.Count == 1 && DefeatedPokemonList[0].GetIsEnemy())
                        {
                            EnemyAI NewEnemyAI = new EnemyAI(null, this, EnemyTrainer);
                            BattlePokemon EnemyNext = NewEnemyAI.GetNextPokemon(DefeatedPokemonList[0]);
                            EventsList.Add(new SwitchWhenDefeatedEvent(
                                DefeatedPokemonList[0], 
                                EnemyNext, 
                                null,
                                null
                                ));
                            DefeatedPokemonList.Clear();
                            ProcessEvents();
                        }
                    }
                    else
                    {
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

    void UpdateUI()
    {
        BattleUIManager.UpdatePlayer1UI(BattlePokemonList[0], PlayerTrainer);
        BattleUIManager.UpdateEnemy1UI(BattlePokemonList[1], EnemyTrainer);
        BattleUIManager.SetCurrentBattlePokemon(BattlePokemonList[0]);
        BattleUIManager.GenerateSkills();
    }

    public void AddDefeatedPokemon(BattlePokemon InPokemon)
    {
        DefeatedPokemonList.Add(InPokemon);
    }
    
    public void AddSkillEvent(BattleSkill InSkill, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        List<BattlePokemon> TargetList = new List<BattlePokemon>();
        TargetList.Add(TargetPokemon);
        EventsList.Add(new SkillEvent(InSkill, SourcePokemon, TargetList));
    }

    public void AddSwitchEvent(BattlePokemon OutPokemon, BattlePokemon InPokemon)
    {
        EventsList.Add(new SwitchEvent(OutPokemon, InPokemon));
    }

    public void TranslateTimePoint(ETimePoint NewTime, Event SourceEvent)
    {
        CurrentTimePoint = NewTime;

        List<BaseAbility> AbilitiesToTrigger = this.QueryAbilitiesWhenTimeChange(SourceEvent);

        foreach(var AbilityIter in AbilitiesToTrigger)
        {
            string name = AbilityIter.GetAbilityName();
            List<Event> EventsToProcess = AbilityIter.Trigger(this, SourceEvent);
            AbilityTriggerEvent AbilityEvent = new AbilityTriggerEvent(EventsToProcess, AbilityIter);
            AbilityEvent.Process(this);
        }
    }

    public void ProcessEvents()
    {
        foreach(var EventIter in EventsList)
        {
            if(EventIter.ShouldProcess(this))
            {
                EventIter.Process(this);
            }
        }
        EventsList.Clear();
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

    public void Test()
    {
        BattlePokemonList[0].LoadBasePokemonStats();
        BattlePokemonList[1].LoadBasePokemonStats();
        UpdateUI();
        BeginSingleBattle(BattlePokemonList[0], BattlePokemonList[1]);
        ProcessEvents();
    }

    public void TestSkillFunc()
    {
        BattleSkill TestBattleSkill = new BattleSkill(TestSkill, EMasterSkill.None, BattlePokemonList[0]);
        List<BattlePokemon> TargetPokemon = new List<BattlePokemon>();
        TargetPokemon.Add(BattlePokemonList[1]);
        EventsList.Add(new SkillEvent(TestBattleSkill, BattlePokemonList[0], TargetPokemon));
        ProcessEvents();
    }

    public void OnUseSkill(BaseSkill InSkill, BattlePokemon InReferencePokemon)
    {
        // Currently Only Single Battle.
        BattleSkill UseBattleSkill = new BattleSkill(InSkill, EMasterSkill.None, InReferencePokemon);
        List<BattlePokemon> TargetPokemon = new List<BattlePokemon>();
        List<BattlePokemon> Opposites = GetOpppoitePokemon(UseBattleSkill.GetReferencePokemon());
        if(InSkill.GetSkillRange() != ERange.None)
        {
            TargetPokemon.Add(Opposites[0]);
        }
        EventsList.Add(new SkillEvent(UseBattleSkill, UseBattleSkill.GetReferencePokemon(), TargetPokemon));
        EnemyAI NewEnemyAI = new EnemyAI(Opposites[0], this, EnemyTrainer);
        NewEnemyAI.GenerateEnemyEvent(EventsList);
        ProcessEvents();
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

    public void SetNewPlayerPokemon(BattlePokemon InPokemon)
    {
        BattlePokemonList[0] = InPokemon;
    }
    public void SetNewEnemyPokemon(BattlePokemon InPokemon)
    {
        BattlePokemonList[1] = InPokemon;
    }
}
