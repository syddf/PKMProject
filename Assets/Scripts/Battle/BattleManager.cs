using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private List<Event> EventsList;
    private ETimePoint CurrentTimePoint;

    [SerializeField]
    private List<BattlePokemon> BattlePokemonList;

    private List<BattlePokemon> DefeatedPokemonList;
    
    [SerializeField]
    private BaseSkill TestSkill;

    // Start is called before the first frame update
    void Start()
    {
        EventsList = new List<Event>();
        CurrentTimePoint = ETimePoint.None;
        DefeatedPokemonList = new List<BattlePokemon>();
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
            EditorLog.DebugLog("Ability Trigger: " + name);
            List<Event> EventsToProcess = AbilityIter.Trigger(this, SourceEvent);
            foreach(var AbilityEventIter in EventsToProcess)
            {
                if(AbilityEventIter.ShouldProcess(this))
                {
                    AbilityEventIter.Process(this);
                }
            }        
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
}
