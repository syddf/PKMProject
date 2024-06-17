using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassySurge : EnterAbilityBase
{       
    public override bool ExtraCondition(ETimePoint TimePoint, Event SourceEvent)
    {
        return BattleManager.StaticManager.GetTerrainType() != EBattleFieldTerrain.Grass;
    }
    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        List<Event> NewEvents = new List<Event>();
        NewEvents.Add(new TerrainChangeEvent(ReferencePokemon, InManager, EBattleFieldTerrain.Grass));
        return NewEvents;
    }
}
