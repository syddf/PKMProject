using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moxie : BaseAbility
{
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(TimePoint != ETimePoint.AfterPokemonDefeated)
        {
            return false;
        }

        if(SourceEvent.GetEventType() == EventType.PokemonDefeated)
        {
            PokemonDefeatedEvent CastedEvent = (PokemonDefeatedEvent)SourceEvent;
            return CastedEvent.GetSourcePokemon() == this.ReferencePokemon;
        }

        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        PokemonDefeatedEvent CastedEvent = (PokemonDefeatedEvent)SourceEvent;
        List<BattlePokemon> Enemies = InManager.GetOpppoitePokemon(ReferencePokemon);
        List<Event> NewEvents = new List<Event>();
        NewEvents.Add(new StatChangeEvent(CastedEvent.GetSourcePokemon(), "Atk", 1));
        return NewEvents;
    }
}
