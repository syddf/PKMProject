using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameBody : BaseAbility
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
            if(Target == this.ReferencePokemon)
            {
                if(BattleSkillMetaInfo.IsTouchingSkill(CastedEvent.GetSkill().GetSkillName()) && CastedEvent.GetSourcePokemon().IsDead() == false)
                {
                    if(SetPokemonStatusChangeEvent.IsStatusChangeEffective(CastedEvent.GetReferenceManager(), CastedEvent.GetSourcePokemon(), null, EStatusChange.Burn))
                    {
                        System.Random rnd = new System.Random();
                        int randNumber = rnd.Next(0, 100);
                        if(randNumber < 30)
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        List<Event> NewEvents = new List<Event>();
        SkillEvent CastedEvent = (SkillEvent)SourceEvent;
        SetPokemonStatusChangeEvent StatusEvent = new SetPokemonStatusChangeEvent(
            CastedEvent.GetSourcePokemon(),
            null,
            InManager,
            EStatusChange.Burn,
            0, 
            false
        );
        NewEvents.Add(StatusEvent);
        return NewEvents;
    }
}
