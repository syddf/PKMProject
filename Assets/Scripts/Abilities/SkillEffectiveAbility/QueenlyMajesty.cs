using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenlyMajesty : BaseAbility
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
                if(CastedEvent.GetSkill().GetSkillPriority(CastedEvent.GetReferenceManager(), CastedEvent.GetSourcePokemon(), Target) > 0)
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
        CastedEvent.MakeCurrentTargetNoEffect("因为女王的威严无法受到任何先制招式的攻击!");
        return NewEvents;
    }
}
