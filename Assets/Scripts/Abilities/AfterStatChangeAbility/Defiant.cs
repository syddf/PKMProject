using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defiant : BaseAbility
{
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(TimePoint != ETimePoint.AfterStatChange)
        {
            return false;
        }

        if(SourceEvent.GetEventType() == EventType.StatChange)
        {
            StatChangeEvent CastedEvent = (StatChangeEvent)SourceEvent;
            return CastedEvent.GetTargetPokemon() == this.ReferencePokemon 
            && CastedEvent.GetChangeLevel() < 0 
            && CastedEvent.GetSourcePokemon().GetIsEnemy() != this.ReferencePokemon.GetIsEnemy();
        }

        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        List<Event> NewEvents = new List<Event>();
        NewEvents.Add(new StatChangeEvent(this.ReferencePokemon, this.ReferencePokemon, "Atk", 2));
        return NewEvents;
    }
}
