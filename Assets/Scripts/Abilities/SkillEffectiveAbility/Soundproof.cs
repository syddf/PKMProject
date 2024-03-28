using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soundproof : BaseAbility
{
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
        CastedEvent.MakeCurrentTargetNoEffect();
        return NewEvents;
    }
}
