using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aftermath : BaseAbility
{
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(TimePoint != ETimePoint.AfterPokemonDefeated)
        {
            return false;
        }
        PokemonDefeatedEvent CastedEvent = (PokemonDefeatedEvent)SourceEvent;
        if(CastedEvent.GetSourcePokemon() != null && 
           CastedEvent.GetTargetPokemon() == ReferencePokemon &&
           CastedEvent.GetSourcePokemon().IsDead() == false &&
           BattleManager.StaticManager.IsPokemonInField(CastedEvent.GetSourcePokemon()) == true)
        {
            BattleSkill ReferenceSkill = CastedEvent.GetReferenceSkill();
            if(BattleSkillMetaInfo.IsTouchingSkill(ReferenceSkill.GetSkillName()))
            {
                return true;
            }
        }
        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        PokemonDefeatedEvent CastedEvent = (PokemonDefeatedEvent)SourceEvent;
        List<Event> NewEvents = new List<Event>();
        DamageEvent damageEvent = new DamageEvent(CastedEvent.GetSourcePokemon(), CastedEvent.GetSourcePokemon().GetMaxHP() / 4, "引爆");
        NewEvents.Add(damageEvent);
        return NewEvents;
    }
}
