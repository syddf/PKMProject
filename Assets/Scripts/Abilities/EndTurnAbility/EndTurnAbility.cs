using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnAbility : BaseAbility
{
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(TimePoint != ETimePoint.TurnEnd)
        {
            return false;
        }

        if(SourceEvent.GetEventType() == EventType.TurnEnd && this.ReferencePokemon.IsDead() == false)
        {
            return true;
        }

        return false;
    }
}
