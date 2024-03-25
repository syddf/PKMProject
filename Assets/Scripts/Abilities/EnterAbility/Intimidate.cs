using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intimidate : BaseAbility
{
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(TimePoint != ETimePoint.PokemonIn && TimePoint != ETimePoint.BattleStart)
        {
            return false;
        }

        if(SourceEvent.GetEventType() == EventType.Switch)
        {
            SwitchEvent CastedEvent = (SwitchEvent)SourceEvent;
            return CastedEvent.GetInPokemon() == this.ReferencePokemon;
        }

        if(SourceEvent.GetEventType() == EventType.SwitchAfterDefeated)
        {
            SwitchWhenDefeatedEvent CastedEvent = (SwitchWhenDefeatedEvent)SourceEvent;
            return CastedEvent.GetPlayerNewPokemon() == this.ReferencePokemon || CastedEvent.GetEnemyNewPokemon() == this.ReferencePokemon;
        }

        if(SourceEvent.GetEventType() == EventType.BattleStart)
        {
            SingleBattleGameStartEvent CastedEvent = (SingleBattleGameStartEvent)SourceEvent;
            return CastedEvent.GetPlayerPokemon() == this.ReferencePokemon || CastedEvent.GetEnemyPokemon() == this.ReferencePokemon;
        }

        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        List<BattlePokemon> Enemies = InManager.GetOpppoitePokemon(ReferencePokemon);
        List<Event> NewEvents = new List<Event>();
        foreach(var BattlePokemonIter in Enemies)
        {
            NewEvents.Add(new StatChangeEvent(BattlePokemonIter, "Atk", -1));
        }
        return NewEvents;
    }
}
