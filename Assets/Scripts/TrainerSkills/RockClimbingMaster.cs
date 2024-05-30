using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockClimbingMaster : BaseTrainerSkill
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
            if(Target && Target.GetIsEnemy() != ReferenceTrainer.IsPlayer && Target.HasType(EType.Rock, BattleManager.StaticManager, null, null))
            {
                if(CastedEvent.GetSkill().GetReferenceSkill().GetSkillType(CastedEvent.GetSourcePokemon()) == EType.Ground)
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
        CastedEvent.MakeCurrentTargetNoEffect("因为训练家技能攀岩大师的效果不会受到地面属性招式的攻击!");
        return NewEvents;
    }

}