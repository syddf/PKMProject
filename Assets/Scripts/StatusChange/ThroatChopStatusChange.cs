using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThroatChopStatusChange : BaseStatusChange
{
    public ThroatChopStatusChange(BattlePokemon InReferencePokemon) : base(InReferencePokemon)
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
                if(BattleSkillMetaInfo.IsSoundSkill(CastedEvent.GetSkill().GetSkillName()))
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
        CastedEvent.ForbidSkill("深渊突刺的效果");
        return NewEvents;
    }

    public override string GetSetMessageText()
    {
        return ReferencePokemon.GetName() + "变得无法发出声音了!";
    }
    
    public BattlePokemon GetReferencePokemon()
    {
        return ReferencePokemon;
    }
}
