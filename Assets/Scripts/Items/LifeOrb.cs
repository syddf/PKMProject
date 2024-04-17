using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LifeOrb : BaseItem
{
    public override bool IsConsumable()
    {
        return false;
    }

    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent, BattlePokemon ReferencePokemon)
    {
        if(TimePoint != ETimePoint.AfterActivateSkill)
        {
            return false;
        }

        if(SourceEvent.GetEventType() == EventType.UseSkill && ReferencePokemon.IsDead() == false)
        {
            SkillEvent CastEvent = (SkillEvent)SourceEvent;
            return CastEvent.GetSkill().IsDamageSkill() && CastEvent.GetSourcePokemon() == ReferencePokemon;
        }

        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent, BattlePokemon ReferencePokemon)
    {
        List<Event> NewEvents = new List<Event>();
        int selfDamage = Math.Min(ReferencePokemon.GetHP(), Math.Max(1, ReferencePokemon.GetMaxHP() / 10));
        DamageEvent damageEvent = new DamageEvent(ReferencePokemon, selfDamage, "生命宝珠的效果");
        NewEvents.Add(damageEvent);
        return NewEvents;
    }
}
