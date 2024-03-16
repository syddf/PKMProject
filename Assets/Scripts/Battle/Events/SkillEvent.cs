using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEvent : Event
{
    private BaseSkill Skill;
    private BattlePokemon SourcePokemon;
    private BattlePokemon TargetPokemon;

    public SkillEvent(BaseSkill InSkill, BattlePokemon InSourcePokemon, BattlePokemon InTargetPokemon)
    {
        Skill = InSkill;
        SourcePokemon = InSourcePokemon;
        TargetPokemon = InTargetPokemon;
    }

    public void PlayAnimation()
    {

    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        return true;
    }

    public void Process(BattleManager InManager)
    {

    }

    public EventType GetEventType()
    {
        return EventType.UseSkill;
    }
}
