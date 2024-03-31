using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TurnEndEvent : EventAnimationPlayer, Event
{
    private BattleManager ReferenceBattleManager;

    public TurnEndEvent(BattleManager InBattleManager)
    {
        ReferenceBattleManager = InBattleManager;
    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        if(InBattleManager.GetBattleEnd() == true) return false;
        return true;
    }

    public override void InitAnimation()
    {
        
    }

    public void ProcessGrassTerrainHealEvent(BattleManager InManager)
    {
        var BattlePokemons = InManager.GetBattlePokemons();
        for(int Index = 0; Index < BattlePokemons.Count; Index++)
        {
            if(!BattlePokemons[Index].IsDead() && BattlePokemons[Index].IsGroundPokemon())
            {
                int HealHP = BattlePokemons[Index].GetMaxHP() / 16;
                HealEvent healEvent = new HealEvent(BattlePokemons[Index], HealHP, "青草场地");
                healEvent.Process(InManager);
            }
        }
    }

    public void ProcessTimeReduceEvent(BattleManager InManager)
    {
        if(InManager.ReduceTerrainTurn())
        {
            TerrainChangeEvent resetTerrain = new TerrainChangeEvent(null, InManager, EBattleFieldTerrain.None);
            resetTerrain.Process(InManager);
        }

        var BattlePokemons = InManager.GetBattlePokemons();
        for(int Index = 0; Index < BattlePokemons.Count; Index++)
        {
            if(!BattlePokemons[Index].IsDead())
            {
                BattlePokemons[Index].ReduceAllStatusChangeRemainTime();
            }
        }
    }

    public void Process(BattleManager InManager)
    {
        if(!ShouldProcess(InManager)) return;
        if(InManager.GetTerrainType() == EBattleFieldTerrain.Grass)
        {
            ProcessGrassTerrainHealEvent(InManager);
        }
        InManager.TranslateTimePoint(ETimePoint.TurnEnd, this);
        ProcessTimeReduceEvent(InManager);
        InManager.AddAnimationEvent(this);
    }

    public EventType GetEventType()
    {
        return EventType.TurnEnd;
    }

    public BattleManager GetReferenceManager()
    {
        return ReferenceBattleManager;   
    }

}
