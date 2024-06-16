using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherStrategy : BaseTrainerSkill
{
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(TimePoint != ETimePoint.AfterChangeWeather)
        {
            return false;
        }

        return true;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        List<Event> NewEvents = new List<Event>();
        SkillEvent CastedEvent = (SkillEvent)SourceEvent;
        OverridePokemonTypeEvent newEvent = new OverridePokemonTypeEvent(InManager, CastedEvent.GetSourcePokemon(), CastedEvent.GetSkill().GetReferenceSkill().GetSkillType(CastedEvent.GetSourcePokemon()));
        NewEvents.Add(newEvent);
        return NewEvents;
    }

}
