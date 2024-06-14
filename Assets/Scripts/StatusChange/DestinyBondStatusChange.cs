using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DestinyBondStatusChange : BaseStatusChange
{
    public DestinyBondStatusChange(BattlePokemon InReferencePokemon) : base(InReferencePokemon)
    {
    }
    
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(TimePoint != ETimePoint.AfterPokemonDefeated)
        {
            return false;
        }
        PokemonDefeatedEvent CastedEvent = (PokemonDefeatedEvent)SourceEvent;
        if(CastedEvent.GetSourcePokemon() != null && 
           CastedEvent.GetTargetPokemon() == ReferencePokemon &&
           CastedEvent.GetSourcePokemon().IsDead() == false &&
           BattleManager.StaticManager.IsPokemonInField(CastedEvent.GetSourcePokemon()) == true)
        {
            return true;
        }
        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        PokemonDefeatedEvent CastedEvent = (PokemonDefeatedEvent)SourceEvent;
        List<Event> NewEvents = new List<Event>();
        DamageEvent damageEvent = new DamageEvent(CastedEvent.GetSourcePokemon(), CastedEvent.GetSourcePokemon().GetMaxHP(), "同命");
        NewEvents.Add(damageEvent);
        return NewEvents;
    }

}
