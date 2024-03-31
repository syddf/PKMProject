using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectStatusChange : BaseStatusChange
{
    public ProtectStatusChange(BattlePokemon InReferencePokemon) : base(InReferencePokemon)
    {
        
    }
    
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(TimePoint != ETimePoint.BeforeJudgeSkillIsEffective)
        {
            return false;
        }

        if(SourceEvent.GetEventType() == EventType.UseSkill)
        {
            SkillEvent CastedEvent = (SkillEvent)SourceEvent;
            BattlePokemon Target = CastedEvent.GetCurrentProcessTargetPokemon();
            if(Target == this.ReferencePokemon)
            {
                if(CastedEvent.GetSkill().CanBeProtected())
                {
                    return true;
                }
            }
        }

        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        List<Event> NewEvents = new List<Event>();
        SkillEvent CastedEvent = (SkillEvent)SourceEvent;
        CastedEvent.MakeCurrentTargetNoEffect("因为守住效果保护了自己!");
        return NewEvents;
    }

    public BattlePokemon GetReferencePokemon()
    {
        return ReferencePokemon;
    }
}
