using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ParalysisStatusChange : BaseStatusChange
{
    public ParalysisStatusChange(BattlePokemon InReferencePokemon) : base(InReferencePokemon)
    {
        
    }
    
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(TimePoint != ETimePoint.BeforeActivateSkill)
        {
            return false;
        }

        if(SourceEvent.GetEventType() == EventType.UseSkill)
        {
            SkillEvent CastedEvent = (SkillEvent)SourceEvent;
            BattlePokemon Source = CastedEvent.GetSourcePokemon();
            System.Random rnd = new System.Random();
            int Random = rnd.Next(0, 100);
            if(Source == this.ReferencePokemon && Random < 25)
            {
                return true;
            }
        }

        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        List<Event> NewEvents = new List<Event>();
        SkillEvent CastedEvent = (SkillEvent)SourceEvent;
        CastedEvent.ForbidSkill("麻痹");
        NewEvents.Add(new StatusChangeAnimationFakeEvent(ReferencePokemon, EStatusChange.Paralysis));
        return NewEvents;
    }
}
