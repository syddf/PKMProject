using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlinchStatusChange : BaseStatusChange
{
    public FlinchStatusChange(BattlePokemon InReferencePokemon) : base(InReferencePokemon)
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
            if(Source == this.ReferencePokemon)
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
        CastedEvent.ForbidSkill("畏缩");
        return NewEvents;
    }

    public BattlePokemon GetReferencePokemon()
    {
        return ReferencePokemon;
    }
}
