using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leftovers : BaseItem
{
    public override bool IsConsumable()
    {
        return false;
    }

    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent, BattlePokemon ReferencePokemon)
    {
        if(TimePoint != ETimePoint.TurnEnd)
        {
            return false;
        }

        if(SourceEvent.GetEventType() == EventType.TurnEnd && ReferencePokemon.IsDead() == false)
        {
            return true;
        }

        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent, BattlePokemon ReferencePokemon)
    {
        List<Event> NewEvents = new List<Event>();
        int HealHP = ReferencePokemon.GetMaxHP() / 16;
        HealEvent healEvent = new HealEvent(ReferencePokemon, HealHP, "吃剩的东西");
        NewEvents.Add(healEvent);
        return NewEvents;
    }
}
