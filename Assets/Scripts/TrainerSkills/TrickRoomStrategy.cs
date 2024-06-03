using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickRoomStrategy : BaseTrainerSkill
{
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(TimePoint != ETimePoint.FinishBattleStart)
        {
            return false;
        }

        return true;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        List<Event> NewEvents = new List<Event>();
        NewEvents.Add(new TrickRoomChangeEvent(null, InManager, true, 5));
        return NewEvents;
    }

}
