using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResistBerry : BaseItem
{
    public EType ResistType;
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
            && CastEvent.GetSkill().GetReferenceSkill().GetSkillType(ReferencePokemon) == ResistType 
            && CastEvent.GetCurrentProcessTargetPokemon() == ReferencePokemon
            && CastEvent.GetSkill().GetSkillClass() != ESkillClass.StatusMove
            && CastDamageSkill.GetPower(BattleManager.StaticManager, CastEvent.GetSourcePokemon(), CastEvent.GetCurrentProcessTargetPokemon()) != 0
            && CastDamageSkill.GetTypeEffectiveFactor(BattleManager.StaticManager, CastEvent.GetSourcePokemon(), CastEvent.GetCurrentProcessTargetPokemon()) > 1.0;
        }
        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent, BattlePokemon ReferencePokemon)
    {
        SkillEvent CastEvent = (SkillEvent)SourceEvent;
        List<Event> NewEvents = new List<Event>();
        List<string> Message = new List<string>();
        Message.Add(CastEvent.GetSkill().GetSkillName() + "因" + ItemName + "的效果减轻了伤害!");
        NewEvents.Add(new MessageAnimationFakeEvent(Message));
        return NewEvents;
    }

    public override bool IsResistBerry()
    {
        return true;
    }
}
