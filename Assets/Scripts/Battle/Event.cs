using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ETimePoint
{
    None,
    BattleStart,
    PokemonIn,
    UseSkill,
    PokemonOut,
    BeforeStatChange,
    AfterStatChange
}

public enum EEventResultType
{

}

public struct EventResult
{
    public EEventResultType ResultType;
    public List<string> ResultValues;
}

public enum EventType
{
    BattleStart,
    Switch,
    UseSkill,
    TriggerAbility,
    TriggerTrainerSkill,

    StatChange
}

public interface Event
{
    public void PlayAnimation();
    public bool ShouldProcess(BattleManager InBattleManager);
    public void Process(BattleManager InManager);
    public EventType GetEventType();
}
