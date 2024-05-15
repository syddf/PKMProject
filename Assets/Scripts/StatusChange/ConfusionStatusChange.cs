using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ConfusionStatusChange : BaseStatusChange
{
    private int RemainTurn;
    private bool IsJudgingConfusion;
    public ConfusionStatusChange(BattlePokemon InReferencePokemon) : base(InReferencePokemon)
    {
        System.Random rnd = new System.Random();
        RemainTurn = rnd.Next(1, 5);
    }
    
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(TimePoint != ETimePoint.JudgeConfusion && TimePoint != ETimePoint.AfterActivateSkill)
        {
            return false;
        }
        
        if(SourceEvent.GetEventType() == EventType.UseSkill)
        {
            SkillEvent CastedEvent = (SkillEvent)SourceEvent;
            BattlePokemon Source = CastedEvent.GetSourcePokemon();
            if(Source == this.ReferencePokemon)
            {
                IsJudgingConfusion = TimePoint == ETimePoint.JudgeConfusion;
                return true;
            }
        }
        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        List<Event> NewEvents = new List<Event>();
        if(IsJudgingConfusion)
        {
            NewEvents.Add(new StatusChangeAnimationFakeEvent(ReferencePokemon, EStatusChange.Confusion));
            if(RemainTurn == 0)
            {
                SkillEvent CastedEvent = (SkillEvent)SourceEvent;
                NewEvents.Add(new RemovePokemonStatusChangeEvent(CastedEvent.GetSourcePokemon(), InManager, EStatusChange.Confusion, ""));
            }
            else
            {
                System.Random rnd = new System.Random();
                int Rand = rnd.Next(0, 3);
                if(Rand == 0)
                {
                    SkillEvent CastedEvent = (SkillEvent)SourceEvent;
                    CastedEvent.ReplaceConfusionSkill();
                }
            }
        }
        else
        {
            RemainTurn -= 1;
        }
        return NewEvents;
    }

}
