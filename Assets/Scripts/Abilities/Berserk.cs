using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berserk : BaseAbility
{
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
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
            if(SkillReferencePokemon == this.ReferencePokemon && SkillReferencePokemon.IsDead() == false && BattleManager.StaticManager.IsPokemonInField(SkillReferencePokemon))
            {
                int HalfHP = ReferencePokemon.GetMaxHP() / 2;
                int CurHP = ReferencePokemon.GetHP();
                if(CurHP <= HalfHP)
                {
                    int PrevHP = CurHP + Damage;
                    if(PrevHP > HalfHP)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        List<Event> NewEvents = new List<Event>();
        StatChangeEvent statChangeEvent = new StatChangeEvent(this.ReferencePokemon, null, "SAtk", 1);
        NewEvents.Add(statChangeEvent);
        return NewEvents;
    }
}
