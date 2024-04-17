using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusSash : BaseItem
{
    public override bool IsConsumable()
    {
        return true;
    }

    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent, BattlePokemon ReferencePokemon)
    {
        if(TimePoint != ETimePoint.BeforeTakenDamage)
        {
            return false;
        }

        if(SourceEvent.GetEventType() == EventType.UseSkill && ReferencePokemon.GetHP() == ReferencePokemon.GetMaxHP())
        {
            SkillEvent CastEvent = (SkillEvent)SourceEvent;
            return CastEvent.GetSkill().IsDamageSkill() && CastEvent.GetCurrentDamage() == ReferencePokemon.GetHP();
        }

        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent, BattlePokemon ReferencePokemon)
    {
        List<Event> NewEvents = new List<Event>();
        SkillEvent CastEvent = (SkillEvent)SourceEvent;
        CastEvent.SetCurrentDamage(ReferencePokemon.GetHP() - 1);
        return NewEvents;
    }
}
