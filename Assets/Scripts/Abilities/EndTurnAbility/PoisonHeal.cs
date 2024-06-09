using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonHeal : BaseAbility
{    
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(TimePoint != ETimePoint.TurnEnd)
        {
            return false;
        }

        if(SourceEvent.GetEventType() == EventType.TurnEnd && this.ReferencePokemon.IsDead() == false)
        {
            if(this.ReferencePokemon.HasStatusChange(EStatusChange.Poison) && this.ReferencePokemon.GetHP() < this.ReferencePokemon.GetMaxHP())
            {
                return true;
            }
        }

        return false;
    }
    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        List<Event> NewEvents = new List<Event>();
        int HealHP = ReferencePokemon.GetMaxHP() / 8;
        HealEvent healEvent = new HealEvent(ReferencePokemon, HealHP, "毒疗");
        NewEvents.Add(healEvent);
        return NewEvents;
    }
}
