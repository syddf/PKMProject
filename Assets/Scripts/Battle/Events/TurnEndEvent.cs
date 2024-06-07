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
            if(!BattlePokemons[Index].IsDead() && BattlePokemons[Index].IsGroundPokemon(InManager))
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
            if(!BattlePokemons[Index].IsDead() && !(BattlePokemons[Index].HasType(EType.Ground, InManager, null, null) || BattlePokemons[Index].HasType(EType.Rock, InManager, null, null) || BattlePokemons[Index].HasType(EType.Steel, InManager, null, null)))
            {
                if(BattlePokemons[Index].HasAbility("沙之力", null, null, null) == false)
                {
                    int Damage = BattlePokemons[Index].GetMaxHP() / 16;
                    DamageEvent damageEvent = new DamageEvent(BattlePokemons[Index], Damage, "沙暴天气");
                    damageEvent.Process(InManager);
                }
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

        if(InManager.ReduceWeatherTurn())
        {
            WeatherChangeEvent resetWeather = new WeatherChangeEvent(null, InManager, EWeather.None);
            resetWeather.Process(InManager);
        }

        if(InManager.ReduceTrickRoomTurn())
        {
            TrickRoomChangeEvent resetTrickRoom = new TrickRoomChangeEvent(null, InManager, false, 0);
            resetTrickRoom.Process(InManager);
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
            RemoveBattleFieldStatusChangeEvent RemoveStatusEvent = new RemoveBattleFieldStatusChangeEvent(InManager, PlayerRemoveStatus, "持续时间结束", true, true);
            RemoveStatusEvent.Process(InManager);
        }
        foreach(var EnemyRemoveStatus in EnemyRemoveStatusList)
        {
            RemoveBattleFieldStatusChangeEvent RemoveStatusEvent = new RemoveBattleFieldStatusChangeEvent(InManager, EnemyRemoveStatus, "持续时间结束", false, true);
            RemoveStatusEvent.Process(InManager);
        }

        if(InManager.HasSpecialRule("特殊规则(葛吉花)") && (InManager.GetCurrentTurnIndex() % 2) == 1)
        {
            List<BattleFieldStatus> EnemyStatusList = new List<BattleFieldStatus>(InManager.GetBattleFieldStatusList(false));
            if(EnemyStatusList != null)
            {
                foreach(var EnemyRemoveStatus in EnemyStatusList)
                {
                    RemoveBattleFieldStatusChangeEvent RemoveStatusEvent = new RemoveBattleFieldStatusChangeEvent(InManager, EnemyRemoveStatus.StatusType, "特殊规则", false, true);
                    RemoveStatusEvent.Process(InManager);
                }
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
