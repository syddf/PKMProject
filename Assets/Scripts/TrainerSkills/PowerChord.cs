using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerChord : BaseTrainerSkill
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
            BattleManager ReferenceManager = CastedEvent.GetReferenceManager();
            if(CastedEvent.GetSourcePokemon() != null && CastedEvent.GetSourcePokemon().GetIsEnemy() != ReferenceTrainer.IsPlayer)
            {
                ESkillClass SkillClass = CastedEvent.GetSkill().GetSkillClass();
                if(SkillClass == ESkillClass.PhysicalMove && !ReferenceManager.HasBattleFieldStatus(ReferenceTrainer.IsPlayer, EBattleFieldStatus.PowerChordBlue))
                {
                    return true;
                }
                if(SkillClass == ESkillClass.SpecialMove && !ReferenceManager.HasBattleFieldStatus(ReferenceTrainer.IsPlayer, EBattleFieldStatus.PowerChordGreen))
                {
                    return true;
                }
                if(SkillClass == ESkillClass.StatusMove && !ReferenceManager.HasBattleFieldStatus(ReferenceTrainer.IsPlayer, EBattleFieldStatus.PowerChordPurple))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void RemoveOldState(EBattleFieldStatus InStatus, BattleManager ReferenceManager, List<Event> NewEvents)
    {
        if(ReferenceManager.HasBattleFieldStatus(ReferenceTrainer.IsPlayer, InStatus))
        {
            NewEvents.Add(new RemoveBattleFieldStatusChangeEvent(ReferenceManager, InStatus, "状态切换", ReferenceTrainer.IsPlayer));
        }
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        List<Event> NewEvents = new List<Event>();
        SkillEvent CastedEvent = (SkillEvent)SourceEvent;
        ESkillClass SkillClass = CastedEvent.GetSkill().GetSkillClass();
        if(SkillClass == ESkillClass.PhysicalMove)
        {            
            RemoveOldState(EBattleFieldStatus.PowerChordGreen, InManager, NewEvents);
            RemoveOldState(EBattleFieldStatus.PowerChordPurple, InManager, NewEvents);
            SetBattleFieldStatusChangeEvent NewEvent = new SetBattleFieldStatusChangeEvent(null, InManager, EBattleFieldStatus.PowerChordBlue, 2, true, ReferenceTrainer.IsPlayer);
            NewEvents.Add(NewEvent);
        }
        if(SkillClass == ESkillClass.SpecialMove)
        {
            RemoveOldState(EBattleFieldStatus.PowerChordBlue, InManager, NewEvents);
            RemoveOldState(EBattleFieldStatus.PowerChordPurple, InManager, NewEvents);
            SetBattleFieldStatusChangeEvent NewEvent = new SetBattleFieldStatusChangeEvent(null, InManager, EBattleFieldStatus.PowerChordGreen, 2, true, ReferenceTrainer.IsPlayer);
            NewEvents.Add(NewEvent);
        }
        if(SkillClass == ESkillClass.StatusMove)
        {
            RemoveOldState(EBattleFieldStatus.PowerChordBlue, InManager, NewEvents);
            RemoveOldState(EBattleFieldStatus.PowerChordGreen, InManager, NewEvents);
            SetBattleFieldStatusChangeEvent NewEvent = new SetBattleFieldStatusChangeEvent(null, InManager, EBattleFieldStatus.PowerChordPurple, 2, true, ReferenceTrainer.IsPlayer);
            NewEvents.Add(NewEvent);
        }
        return NewEvents;
    }

}
