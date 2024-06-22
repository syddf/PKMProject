using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defiant : BaseAbility
{
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(TimePoint != ETimePoint.AfterStatChange)
        {
            return false;
        }

        if(SourceEvent.GetEventType() == EventType.StatChange)
        {
            StatChangeEvent CastedEvent = (StatChangeEvent)SourceEvent;
            List<int> ChangedLevel = CastedEvent.GetChangeLevel();
            int Count = 0;
            for(int Index = 0; Index < ChangedLevel.Count; Index++)
            {
                if(ChangedLevel[Index] < 0)
                {
                    Count++;
                }
            }
            return CastedEvent.GetTargetPokemon() == this.ReferencePokemon 
            && Count > 0
            && CastedEvent.GetSourcePokemon() != null
            && CastedEvent.GetSourcePokemon().GetIsEnemy() != this.ReferencePokemon.GetIsEnemy();
        }

        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        StatChangeEvent CastedEvent = (StatChangeEvent)SourceEvent;
        List<int> ChangedLevel = CastedEvent.GetChangeLevel();
        int Count = 0;
        for(int Index = 0; Index < ChangedLevel.Count; Index++)
        {
            if(ChangedLevel[Index] < 0)
            {
                Count++;
            }
        }
        List<Event> NewEvents = new List<Event>();
        NewEvents.Add(new StatChangeEvent(this.ReferencePokemon, this.ReferencePokemon, "Atk", Count * 2));
        return NewEvents;
    }
}
