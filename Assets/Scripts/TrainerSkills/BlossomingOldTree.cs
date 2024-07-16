using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlossomingOldTree : BaseTrainerSkill
{
    private BattlePokemon GetTarget()
    {
        if(ReferenceTrainer.IsPlayer)
        {
            return BattleManager.StaticManager.GetBattlePokemons()[0];
        }
        return BattleManager.StaticManager.GetBattlePokemons()[1];

    }
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(TimePoint != ETimePoint.TurnEnd)
        {
            return false;
        }

        if(SourceEvent.GetEventType() == EventType.TurnEnd && GetTarget().IsDead() == false && GetTarget().GetHP() < GetTarget().GetMaxHP())
        {
            return true;
        }

        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        List<Event> NewEvents = new List<Event>();
        double Ratio = 0.1;
        BattlePokemon Target = GetTarget();
        int TurnInField = Target.GetTurnInField();
        if(TurnInField > 5)
        {
            TurnInField = 5;
        }
        Ratio = Ratio + TurnInField * 0.02;

        if(InManager.GetTerrainType() == EBattleFieldTerrain.Grass)
        {
            Ratio = 0.2;
        }

        int HealHP = (int)(Target.GetMaxHP() * Ratio);
        NewEvents.Add(new HealEvent(Target, HealHP, "训练家技能老树开花"));
        return NewEvents;
    }
}
