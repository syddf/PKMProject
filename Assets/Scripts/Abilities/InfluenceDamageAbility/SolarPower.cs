using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarPower : BaseAbility
{
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(TimePoint != ETimePoint.TurnEnd)
        {
            return false;
        }

        if(SourceEvent.GetEventType() == EventType.TurnEnd && this.ReferencePokemon.IsDead() == false)
        {
            TurnEndEvent CastedEvent = (TurnEndEvent)SourceEvent;
            if(CastedEvent.GetReferenceManager().GetWeatherType() == EWeather.SunLight)
            {
                return true;
            }
        }

        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        List<Event> NewEvents = new List<Event>();
        int Damage = ReferencePokemon.GetMaxHP() / 8;
        DamageEvent damageEvent = new DamageEvent(ReferencePokemon, Damage, "太阳之力");
        NewEvents.Add(damageEvent);
        return NewEvents;
    }
}