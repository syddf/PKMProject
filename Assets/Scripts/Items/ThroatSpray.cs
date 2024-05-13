using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThroatSpray : BaseItem
{
    public override bool IsConsumable()
    {
        return true;
    }

    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent, BattlePokemon ReferencePokemon)
    {
        if(TimePoint != ETimePoint.AfterActivateSkill)
        {
            return false;
        }

        if(SourceEvent.GetEventType() == EventType.UseSkill && ReferencePokemon.IsDead() == false)
        {
            SkillEvent CastEvent = (SkillEvent)SourceEvent;
            return BattleSkillMetaInfo.IsSoundSkill(CastEvent.GetSkill().GetSkillName()) && CastEvent.GetSourcePokemon() == ReferencePokemon;
        }
        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent, BattlePokemon ReferencePokemon)
    {
        SkillEvent CastEvent = (SkillEvent)SourceEvent;
        List<Event> NewEvents = new List<Event>();
        NewEvents.Add(new StatChangeEvent(CastEvent.GetSourcePokemon(), null, "SAtk", 1));
        return NewEvents;
    }
}
