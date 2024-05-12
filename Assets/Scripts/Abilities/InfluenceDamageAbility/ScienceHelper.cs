using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScienceHelper : BaseAbility
{
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(TimePoint != ETimePoint.BeforeTakenDamage)
        {
            return false;
        }

        if(SourceEvent.GetEventType() == EventType.UseSkill && ReferencePokemon.GetHP() > ReferencePokemon.GetMaxHP() / 2)
        {
            SkillEvent CastEvent = (SkillEvent)SourceEvent;
            return CastEvent.GetSkill().IsDamageSkill() 
            && CastEvent.GetCurrentDamage() == ReferencePokemon.GetHP()
            && CastEvent.GetReferenceManager().GetTerrainType() == EBattleFieldTerrain.Electric;
        }

        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        List<Event> NewEvents = new List<Event>();
        SkillEvent CastEvent = (SkillEvent)SourceEvent;
        CastEvent.SetCurrentDamage(ReferencePokemon.GetHP() - 1);
        return NewEvents;
    }
}
