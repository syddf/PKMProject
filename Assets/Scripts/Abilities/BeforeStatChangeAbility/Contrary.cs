using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contrary : BaseAbility
{
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(TimePoint != ETimePoint.BeforeStatChange)
        {
            return false;
        }

        if(SourceEvent.GetEventType() == EventType.StatChange)
        {
            StatChangeEvent CastedEvent = (StatChangeEvent)SourceEvent;
            return CastedEvent.GetTargetPokemon() == this.ReferencePokemon;
        }

        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        List<BattlePokemon> Enemies = InManager.GetOpppoitePokemon(ReferencePokemon);
        List<Event> NewEvents = new List<Event>();
        StatChangeEvent CastedEvent = (StatChangeEvent)SourceEvent;
        CastedEvent.SetReverseLevel();
        return NewEvents;
    }
}
