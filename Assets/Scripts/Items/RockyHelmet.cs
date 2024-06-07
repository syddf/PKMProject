using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockyHelmet : BaseItem
{
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent, BattlePokemon ReferencePokemon)
    {
        if(TimePoint != ETimePoint.AfterSkillEffect)
        {
            return false;
        }

        if(SourceEvent.GetEventType() == EventType.UseSkill)
        {
            SkillEvent CastedEvent = (SkillEvent)SourceEvent;
            BattlePokemon Target = CastedEvent.GetCurrentProcessTargetPokemon();
            if(Target == ReferencePokemon)
            {
                if(BattleSkillMetaInfo.IsTouchingSkill(CastedEvent.GetSkill().GetSkillName()))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent, BattlePokemon ReferencePokemon)
    {
        List<Event> NewEvents = new List<Event>();
        SkillEvent CastedEvent = (SkillEvent)SourceEvent;
        BattlePokemon SourcePokemon = CastedEvent.GetSourcePokemon();
        DamageEvent damageEvent = new DamageEvent(SourcePokemon, SourcePokemon.GetMaxHP() / 6, "凸凸头盔");
        NewEvents.Add(damageEvent);
        return NewEvents;
    }
}
