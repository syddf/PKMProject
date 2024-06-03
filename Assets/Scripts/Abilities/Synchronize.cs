using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Synchronize : BaseAbility
{
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(TimePoint != ETimePoint.AfterSetPokemonStatusChange)
        {
            return false;
        }

        if(SourceEvent.GetEventType() == EventType.SetPokemonStatusChange)
        {
            SetPokemonStatusChangeEvent CastedEvent = (SetPokemonStatusChangeEvent)SourceEvent;
            return CastedEvent.GetReferencePokemon() == this.ReferencePokemon 
            && StatusChange.IsStatusChange(CastedEvent.GetStatusType()) 
            && (CastedEvent.GetSourcePokemon() != null && CastedEvent.GetSourcePokemon() != this.ReferencePokemon);
        }

        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        List<Event> NewEvents = new List<Event>();
        SetPokemonStatusChangeEvent CastedEvent = (SetPokemonStatusChangeEvent)SourceEvent;
        SetPokemonStatusChangeEvent setStatChangeEvent = new SetPokemonStatusChangeEvent(CastedEvent.GetSourcePokemon(), this.ReferencePokemon , InManager, CastedEvent.GetStatusType(), 1, false);
        NewEvents.Add(setStatChangeEvent);
        return NewEvents;
    }
}
