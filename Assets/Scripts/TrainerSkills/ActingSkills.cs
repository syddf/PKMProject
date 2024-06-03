using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActingSkills : BaseTrainerSkill
{
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(TimePoint != ETimePoint.BeforeSkillEffectAnimFake)
        {
            return false;
        }

        if(SourceEvent.GetEventType() == EventType.UseSkill)
        {
            SkillEvent CastedEvent = (SkillEvent)SourceEvent;
            BattleManager ReferenceManager = CastedEvent.GetReferenceManager();
            if(CastedEvent.GetSourcePokemon() != null && CastedEvent.GetSourcePokemon().GetIsEnemy() != ReferenceTrainer.IsPlayer)
            {
                EType SkillType = CastedEvent.GetSkill().GetReferenceSkill().GetSkillType(CastedEvent.GetSourcePokemon());
                if(CastedEvent.GetSourcePokemon().HasOnlyType(SkillType, ReferenceManager, CastedEvent.GetSourcePokemon(), null) == false)
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
        OverridePokemonTypeEvent newEvent = new OverridePokemonTypeEvent(InManager, CastedEvent.GetSourcePokemon(), CastedEvent.GetSkill().GetReferenceSkill().GetSkillType(CastedEvent.GetSourcePokemon()));
        NewEvents.Add(newEvent);
        return NewEvents;
    }

}
