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

    public void ProcessSandEvent(BattleManager InManager)
    {
        var BattlePokemons = InManager.GetBattlePokemons();
        for(int Index = 0; Index < BattlePokemons.Count; Index++)
        {
            if(!BattlePokemons[Index].IsDead() && !(BattlePokemons[Index].HasType(EType.Ground) || BattlePokemons[Index].HasType(EType.Rock) || BattlePokemons[Index].HasType(EType.Steel)))
            {
                int Damage = BattlePokemons[Index].GetMaxHP() / 16;
                DamageEvent damageEvent = new DamageEvent(BattlePokemons[Index], Damage, "沙暴天气");
                damageEvent.Process(InManager);
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
                List<EStatusChange> RemoveStatus = BattlePokemons[Index].ReduceAllStatusChangeRemainTime();
                foreach(var Status in RemoveStatus)
                {
                    RemovePokemonStatusChangeEvent RemoveEvent = 
                    new RemovePokemonStatusChangeEvent(BattlePokemons[Index], InManager, Status, "持续时间结束");
                    RemoveEvent.Process(InManager);
                }
            }
        }

        List<EBattleFieldStatus> PlayerRemoveStatusList = ReferenceBattleManager.ReduceBattleFieldTime(true);
        List<EBattleFieldStatus> EnemyRemoveStatusList = ReferenceBattleManager.ReduceBattleFieldTime(false);
        foreach(var PlayerRemoveStatus in PlayerRemoveStatusList)
        {
            RemoveBattleFieldStatusChangeEvent RemoveStatusEvent = new RemoveBattleFieldStatusChangeEvent(InManager, PlayerRemoveStatus, "持续时间结束", true);
            RemoveStatusEvent.Process(InManager);
        }
        foreach(var EnemyRemoveStatus in EnemyRemoveStatusList)
        {
            RemoveBattleFieldStatusChangeEvent RemoveStatusEvent = new RemoveBattleFieldStatusChangeEvent(InManager, EnemyRemoveStatus, "持续时间结束", false);
            RemoveStatusEvent.Process(InManager);
        }
    }

    public void Process(BattleManager InManager)
    {
        if(!ShouldProcess(InManager)) return;
        if(InManager.GetTerrainType() == EBattleFieldTerrain.Grass)
        {
            ProcessGrassTerrainHealEvent(InManager);
        }
        if(InManager.GetWeatherType() == EWeather.Sand)
        {
            ProcessSandEvent(InManager);
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
