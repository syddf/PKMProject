using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sturdy : BaseAbility
{
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(TimePoint != ETimePoint.BeforeTakenDamage)
        {
            return false;
        }

        if(SourceEvent.GetEventType() == EventType.UseSkill && ReferencePokemon.GetHP() == ReferencePokemon.GetMaxHP())
        {
            SkillEvent CastEvent = (SkillEvent)SourceEvent;
            return CastEvent.GetSkill().IsDamageSkill() && CastEvent.GetCurrentProcessTargetPokemon() == ReferencePokemon && CastEvent.GetCurrentDamage() == ReferencePokemon.GetHP();
        }

        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        List<Event> NewEvents = new List<Event>();
        SkillEvent CastEvent = (SkillEvent)SourceEvent;
        CastEvent.SetCurrentDamage(ReferencePokemon.GetHP() - 1);
        return NewEvents;
    }
}