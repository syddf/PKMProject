using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfuseStrategy : BaseTrainerSkill
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
            BattlePokemon SourcePokemon = CastedEvent.GetSourcePokemon();
            BattlePokemon TargetPokemon = CastedEvent.GetCurrentProcessTargetPokemon();
            BattleSkill ReferenceSkill = CastedEvent.GetSkill();
            if(SourcePokemon.GetIsEnemy() != ReferenceTrainer.IsPlayer && 
            ReferenceSkill.GetReferenceSkill().GetSkillClass() != ESkillClass.StatusMove &&
            TargetPokemon.IsDead() == false)
            {            
                System.Random rnd = new System.Random();
                int RandNum = rnd.Next(0, 100);                
                return RandNum < 20;
            }
        }

        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        SkillEvent CastedEvent = (SkillEvent)SourceEvent;
        BattlePokemon TargetPokemon = CastedEvent.GetCurrentProcessTargetPokemon();
        BattlePokemon SourcePokemon = CastedEvent.GetSourcePokemon();
        List<Event> NewEvents = new List<Event>();
        SetPokemonStatusChangeEvent setStatChangeEvent = new SetPokemonStatusChangeEvent(TargetPokemon, SourcePokemon, InManager, EStatusChange.Drowsy, 1, false);
        NewEvents.Add(setStatChangeEvent);
        return NewEvents;
    }

}
