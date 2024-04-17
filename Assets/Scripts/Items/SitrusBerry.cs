using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitrusBerry : BaseItem
{
    public override bool IsConsumable()
    {
        return true;
    }

    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent, BattlePokemon ReferencePokemon)
    {

        if(ReferencePokemon.IsDead() == false && ReferencePokemon.GetHP() < ReferencePokemon.GetMaxHP() / 2)
        {
            return true;
        }

        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent, BattlePokemon ReferencePokemon)
    {
        List<Event> NewEvents = new List<Event>();
        int HealHP = ReferencePokemon.GetMaxHP() / 4;
        HealEvent healEvent = new HealEvent(ReferencePokemon, HealHP, "文柚果");
        NewEvents.Add(healEvent);
        return NewEvents;
    }
}
