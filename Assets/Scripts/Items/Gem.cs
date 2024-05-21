using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : BaseItem
{
    public EType GemType;
    public override bool IsConsumable()
    {
        return true;
    }

    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent, BattlePokemon ReferencePokemon)
    {
        if(TimePoint != ETimePoint.BeforeSkillEffectAnimFake)
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
            return CastEvent.GetSkill().IsDamageSkill() 
            && CastEvent.GetSkill().GetReferenceSkill().GetSkillType(ReferencePokemon) == GemType 
            && CastEvent.GetSourcePokemon() == ReferencePokemon
            && CastEvent.GetSkill().GetSkillClass() != ESkillClass.StatusMove
            && CastDamageSkill.GetPower(BattleManager.StaticManager, CastEvent.GetSourcePokemon(), CastEvent.GetCurrentProcessTargetPokemon()) != 0;
        }
        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent, BattlePokemon ReferencePokemon)
    {
        SkillEvent CastEvent = (SkillEvent)SourceEvent;
        List<Event> NewEvents = new List<Event>();
        List<string> Message = new List<string>();
        Message.Add(CastEvent.GetSkill().GetSkillName() + "因" + ItemName + "的效果提高了威力!");
        NewEvents.Add(new MessageAnimationFakeEvent(Message));
        return NewEvents;
    }

    public override bool IsGem()
    {
        return true;
    }
}
