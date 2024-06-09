using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBody : BaseAbility
{    
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(TimePoint != ETimePoint.TurnEnd)
        {
            return false;
        }

        if(SourceEvent.GetEventType() == EventType.TurnEnd && this.ReferencePokemon.IsDead() == false)
        {
            if(BattleManager.StaticManager.GetWeatherType() == EWeather.Snow && this.ReferencePokemon.GetHP() < this.ReferencePokemon.GetMaxHP())
            {
                return true;
            }
        }

        return false;
    }
    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        List<Event> NewEvents = new List<Event>();
        int HealHP = ReferencePokemon.GetMaxHP() / 16;
        HealEvent healEvent = new HealEvent(ReferencePokemon, HealHP, "冰冻之躯");
        NewEvents.Add(healEvent);
        return NewEvents;
    }
}
