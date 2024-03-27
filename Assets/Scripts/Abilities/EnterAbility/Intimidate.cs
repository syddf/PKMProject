using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intimidate : EnterAbilityBase
{
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
