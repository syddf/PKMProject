using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenMenu : BaseTrainerSkill
{
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(TimePoint != ETimePoint.AfterMegaEvolution)
        {
            return false;
        }

        if(SourceEvent.GetEventType() == EventType.MegaEvolution)
        {
            MegaEvent CastedEvent = (MegaEvent)SourceEvent;
            if(CastedEvent.GetReferencePokemon().GetIsEnemy() != ReferenceTrainer.IsPlayer)
            {
                return true;
            }
        }

        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        List<Event> NewEvents = new List<Event>();
        NewEvents.Add(new WeatherChangeEvent(null, InManager, EWeather.Rain));
        return NewEvents;
    }

}
