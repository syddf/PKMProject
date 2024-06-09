using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicOrb : BaseItem
{
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent, BattlePokemon ReferencePokemon)
    {
        if(TimePoint != ETimePoint.TurnEnd)
        {
            return false;
        }

        if(SourceEvent.GetEventType() == EventType.TurnEnd && 
        ReferencePokemon.IsDead() == false && 
        ReferencePokemon.HasStatusChange(EStatusChange.Poison) == false && 
        ReferencePokemon.HasStatusChange(EStatusChange.Drowsy) == false &&
        ReferencePokemon.HasStatusChange(EStatusChange.Paralysis) == false &&
        ReferencePokemon.HasStatusChange(EStatusChange.Burn) == false &&
        ReferencePokemon.HasStatusChange(EStatusChange.Frostbite) == false)
        {
            return true;
        }

        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent, BattlePokemon ReferencePokemon)
    {
        List<Event> NewEvents = new List<Event>();
        SetPokemonStatusChangeEvent setStatChangeEvent = 
        new SetPokemonStatusChangeEvent(ReferencePokemon, null, InManager, EStatusChange.Poison, 1, false);
        NewEvents.Add(setStatChangeEvent);
        return NewEvents;
    }
}
