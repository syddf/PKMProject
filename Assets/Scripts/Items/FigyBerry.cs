using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigyBerry : BaseItem
{
    public override bool IsConsumable()
    {
        return true;
    }

    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent, BattlePokemon ReferencePokemon)
    {

        if(ReferencePokemon.IsDead() == false && ReferencePokemon.GetHP() < ReferencePokemon.GetMaxHP() / 4)
        {
            return true;
        }

        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent, BattlePokemon ReferencePokemon)
    {
        List<Event> NewEvents = new List<Event>();
        int HealHP = ReferencePokemon.GetMaxHP() / 3;
        HealEvent healEvent = new HealEvent(ReferencePokemon, HealHP, "勿花果");
        NewEvents.Add(healEvent);
        return NewEvents;
    }
}
