using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EjectButton : BaseItem
{
    public override bool IsConsumable()
    {
        return true;
    }

    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent, BattlePokemon ReferencePokemon)
    {
        if(TimePoint != ETimePoint.AfterTakenDamage)
        {
            return false;
        }

        if(SourceEvent.GetEventType() == EventType.UseSkill)
        {
            SkillEvent CastedEvent = (SkillEvent)SourceEvent;
            int Damage = CastedEvent.GetCurrentDamage();
            BattlePokemon SkillReferencePokemon = CastedEvent.GetCurrentProcessTargetPokemon();
            if(Damage > 0 && SkillReferencePokemon == ReferencePokemon && SkillReferencePokemon.IsDead() == false && BattleManager.StaticManager.IsPokemonInField(SkillReferencePokemon))
            {
                return true;
            }
        }

        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent, BattlePokemon ReferencePokemon)
    {
        List<Event> NewEvents = new List<Event>();
        if(ReferencePokemon.GetIsEnemy())
        {
            if(InManager.GetEnemyTrainer().GetRemainPokemonNum() > 1)
            {
                SwitchAfterSkillUseEvent switchEvent = new SwitchAfterSkillUseEvent(InManager, ReferencePokemon);
                NewEvents.Add(switchEvent);
            }
        }
        else
        {
            if(InManager.GetPlayerTrainer().GetRemainPokemonNum() > 1)
            {
                SwitchAfterSkillUseEvent switchEvent = new SwitchAfterSkillUseEvent(InManager, ReferencePokemon);
                NewEvents.Add(switchEvent);
            }
        }
        return NewEvents;
    }
}
