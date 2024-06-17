using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drought : EnterAbilityBase
{
    public override bool ExtraCondition(ETimePoint TimePoint, Event SourceEvent)
    {
        return BattleManager.StaticManager.GetWeatherType() != EWeather.SunLight;
    }
    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        List<Event> NewEvents = new List<Event>();
        NewEvents.Add(new WeatherChangeEvent(ReferencePokemon, InManager, EWeather.SunLight));
        return NewEvents;
    }
}