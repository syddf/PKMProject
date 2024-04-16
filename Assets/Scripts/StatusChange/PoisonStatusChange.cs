using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PoisonStatusChange : BaseStatusChange
{
    public PoisonStatusChange(BattlePokemon InReferencePokemon) : base(InReferencePokemon)
    {
        
    }
    
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(TimePoint != ETimePoint.TurnEnd)
        {
            return false;
        }
        TurnEndEvent CastedEvent = (TurnEndEvent)SourceEvent;
        if(CastedEvent.GetReferenceManager().IsPokemonInField(ReferencePokemon) && ReferencePokemon.IsDead() == false)
        {
            return true;
        }
        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        List<Event> NewEvents = new List<Event>();
        int selfDamage = Math.Min(ReferencePokemon.GetHP(), Math.Max(1, ReferencePokemon.GetMaxHP() / 8));
        DamageEvent damageEvent = new DamageEvent(ReferencePokemon, selfDamage, "中毒效果");
        NewEvents.Add(new StatusChangeAnimationFakeEvent(ReferencePokemon, EStatusChange.Poison));
        NewEvents.Add(damageEvent);
        return NewEvents;
    }
}
