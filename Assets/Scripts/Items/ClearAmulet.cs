using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearAmulet : BaseItem
{
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent, BattlePokemon ReferencePokemon)
    {
        if(TimePoint != ETimePoint.BeforeStatChange)
        {
            return false;
        }

        if(SourceEvent.GetEventType() == EventType.StatChange)
        {
            StatChangeEvent CastedEvent = (StatChangeEvent)SourceEvent;
            return CastedEvent.GetTargetPokemon() == ReferencePokemon &&
            CastedEvent.GetTargetPokemon() != CastedEvent.GetSourcePokemon() &&
            CastedEvent.GetChangeLevel()[0] < 0;
        }

        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent, BattlePokemon ReferencePokemon)
    {
        List<BattlePokemon> Enemies = InManager.GetOpppoitePokemon(ReferencePokemon);
        List<Event> NewEvents = new List<Event>();
        StatChangeEvent CastedEvent = (StatChangeEvent)SourceEvent;
        CastedEvent.ForbidAllChange();
        return NewEvents;
    }
}
