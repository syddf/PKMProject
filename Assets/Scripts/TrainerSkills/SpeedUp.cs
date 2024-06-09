using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : BaseTrainerSkill
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
            return CastedEvent.GetSourcePokemon() != null && CastedEvent.GetSourcePokemon().GetIsEnemy() != ReferenceTrainer.IsPlayer;
        }

        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        PokemonDefeatedEvent CastedEvent = (PokemonDefeatedEvent)SourceEvent;
        List<Event> NewEvents = new List<Event>();
        NewEvents.Add(new StatChangeEvent(CastedEvent.GetSourcePokemon(), CastedEvent.GetSourcePokemon(), "Speed", 1));
        return NewEvents;
    }

}
