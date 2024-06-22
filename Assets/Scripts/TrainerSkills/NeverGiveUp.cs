using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeverGiveUp : BaseTrainerSkill
{
    private int Counter = 0;
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(Counter >= 3)
        {
            return false;
        }
        if(TimePoint != ETimePoint.BeforeTakenDamage)
        {
            return false;
        }
        if(SourceEvent.GetEventType() != EventType.UseSkill)
        {
            return false;
        }
        SkillEvent CastEvent = (SkillEvent)SourceEvent;
        BattlePokemon ReferencePokemon = CastEvent.GetCurrentProcessTargetPokemon();
        if(ReferencePokemon.GetHP() == ReferencePokemon.GetMaxHP())
        {
            return CastEvent.GetSkill().IsDamageSkill() && ReferencePokemon.GetIsEnemy() != ReferenceTrainer.IsPlayer && CastEvent.GetCurrentDamage() == ReferencePokemon.GetHP();
        }

        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        List<Event> NewEvents = new List<Event>();
        SkillEvent CastEvent = (SkillEvent)SourceEvent;
        BattlePokemon ReferencePokemon = CastEvent.GetCurrentProcessTargetPokemon();
        CastEvent.SetCurrentDamage(ReferencePokemon.GetHP() - 1);
        Counter += 1;
        return NewEvents;
    }
}
