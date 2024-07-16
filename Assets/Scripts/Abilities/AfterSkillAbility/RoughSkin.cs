using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoughSkin : BaseAbility
{
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(TimePoint != ETimePoint.AfterSkillEffect)
        {
            return false;
        }

        if(SourceEvent.GetEventType() == EventType.UseSkill)
        {
            SkillEvent CastedEvent = (SkillEvent)SourceEvent;
            BattlePokemon Target = CastedEvent.GetCurrentProcessTargetPokemon();
            if(Target == this.ReferencePokemon && Target && Target.IsDead() == false)
            {
                if(BattleSkillMetaInfo.IsTouchingSkill(CastedEvent.GetSkill().GetSkillName()))
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
        BattlePokemon SourcePokemon = CastedEvent.GetSourcePokemon();
        DamageEvent damageEvent = new DamageEvent(SourcePokemon, SourcePokemon.GetMaxHP() / 8, "粗糙皮肤");
        NewEvents.Add(damageEvent);
        return NewEvents;
    }
}
