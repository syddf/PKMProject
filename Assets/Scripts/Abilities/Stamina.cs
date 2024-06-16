using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : BaseAbility
{
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(TimePoint != ETimePoint.AfterTakenDamage)
        {
            return false;
        }

        if(SourceEvent.GetEventType() == EventType.UseSkill)
        {
            SkillEvent CastedEvent = (SkillEvent)SourceEvent;
            int Damage = CastedEvent.GetCurrentDamage();
            BattlePokemon SkillReferencePokemon = CastedEvent.GetCurrentProcessTargetPokemon();
            if(SkillReferencePokemon == this.ReferencePokemon && SkillReferencePokemon.IsDead() == false && BattleManager.StaticManager.IsPokemonInField(SkillReferencePokemon))
            {
                return true;
            }
        }

        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        List<Event> NewEvents = new List<Event>();
        StatChangeEvent statChangeEvent = new StatChangeEvent(this.ReferencePokemon, null, "Def", 1);
        NewEvents.Add(statChangeEvent);
        return NewEvents;
    }
}
