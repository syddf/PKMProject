using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsychicSurge : EnterAbilityBase
{    
    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        List<Event> NewEvents = new List<Event>();
        NewEvents.Add(new TerrainChangeEvent(ReferencePokemon, InManager, EBattleFieldTerrain.Psychic));
        return NewEvents;
    }
}
