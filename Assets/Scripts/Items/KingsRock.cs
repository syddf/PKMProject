using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingsRock : BaseItem
{
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent, BattlePokemon ReferencePokemon)
    {
        if(TimePoint != ETimePoint.AfterSkillEffect)
        {
            return false;
        }

        if(SourceEvent.GetEventType() == EventType.UseSkill && ReferencePokemon.IsDead() == false)
        {
            SkillEvent CastEvent = (SkillEvent)SourceEvent;
            if(CastEvent.GetSkill().GetSkillClass() == ESkillClass.StatusMove)
            {
                return false;
            }
            DamageSkill CastDamageSkill = (DamageSkill)CastEvent.GetSkill().GetReferenceSkill();
            System.Random rnd = new System.Random();
            int randNumber = rnd.Next(0, 100);
            int Prob = 10;
            if(CastEvent.GetSourcePokemon().HasAbility("天恩", null, null, null))
            {
                Prob = 20;
            }
            if(randNumber >= Prob)
            {
                return false;
            }
            if(SetPokemonStatusChangeEvent.IsStatusChangeEffective(BattleManager.StaticManager, CastEvent.GetCurrentProcessTargetPokemon(), ReferencePokemon, EStatusChange.Flinch) == false)
            {
                return false;
            }            
            return CastEvent.GetSkill().IsDamageSkill() 
            && CastEvent.GetSourcePokemon() == ReferencePokemon;
        }
        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent, BattlePokemon ReferencePokemon)
    {
        SkillEvent CastEvent = (SkillEvent)SourceEvent;
        List<Event> NewEvents = new List<Event>();
        BattlePokemon Target = CastEvent.GetCurrentProcessTargetPokemon();
        SetPokemonStatusChangeEvent setStatChangeEvent = new SetPokemonStatusChangeEvent(Target, ReferencePokemon, InManager, EStatusChange.Flinch, 1, true);
        NewEvents.Add(setStatChangeEvent);
        return NewEvents;
    }
}
