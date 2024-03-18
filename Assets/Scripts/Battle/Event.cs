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
    AfterStatChange,
    BeforeActivateSkill,
    BeforeGetSkillCount,
    BeforeJudgeSkillIsEffective,
    BeforeJudgeAccuracy,
    BeforeSkillEffect,
    BeforeTakenDamage,
    AfterTakenDamage,
    AfterSkillEffect,
    BeforePokemonDefeated,
    AfterPokemonDefeated,
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
    PokemonDefeated,
    StatChange,
}

public interface Event
{
    public void PlayAnimation();
    public bool ShouldProcess(BattleManager InBattleManager);
    public void Process(BattleManager InManager);
    public EventType GetEventType();
}
