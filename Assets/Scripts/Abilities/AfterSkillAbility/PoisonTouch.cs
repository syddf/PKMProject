using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonTouch : BaseAbility
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
            BattlePokemon Source = CastedEvent.GetSourcePokemon();
            if(Source == this.ReferencePokemon && Target)
            {
                if(BattleSkillMetaInfo.IsTouchingSkill(CastedEvent.GetSkill().GetSkillName()))
                {
                    if(SetPokemonStatusChangeEvent.IsStatusChangeEffective(CastedEvent.GetReferenceManager(), Target, ReferencePokemon, EStatusChange.Poison))
                    {
                        if(Target.HasAbility("鳞粉", CastedEvent.GetReferenceManager(), Source, Target) == true)
                        {
                            return false;
                        }
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
            CastedEvent.GetCurrentProcessTargetPokemon(),
            ReferencePokemon,
            InManager,
            EStatusChange.Poison,
            0, 
            false
        );
        NewEvents.Add(StatusEvent);
        return NewEvents;
    }
}
