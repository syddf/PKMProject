using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumBerry : BaseItem
{
    public override bool IsConsumable()
    {
        return true;
    }

    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent, BattlePokemon ReferencePokemon)
    {
        if(TimePoint != ETimePoint.AfterSetPokemonStatusChange)
        {
            return false;
        }

        if(SourceEvent.GetEventType() == EventType.SetPokemonStatusChange)
        {
            SetPokemonStatusChangeEvent CastedEvent = (SetPokemonStatusChangeEvent)SourceEvent;
            EStatusChange Status = CastedEvent.GetStatusType();
            return CastedEvent.GetReferencePokemon() == ReferencePokemon && StatusChange.IsStatusChange(Status);
        }

        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent, BattlePokemon ReferencePokemon)
    {
        SetPokemonStatusChangeEvent CastedEvent = (SetPokemonStatusChangeEvent)SourceEvent;
        EStatusChange Status = CastedEvent.GetStatusType();
        List<Event> NewEvents = new List<Event>();
        NewEvents.Add(new RemovePokemonStatusChangeEvent(ReferencePokemon, InManager, Status, "木子果的效果"));
        return NewEvents;
    }
}
