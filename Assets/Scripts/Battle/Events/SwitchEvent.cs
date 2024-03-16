using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchEvent : Event
{
    private BattlePokemon OutPokemon;
    private BattlePokemon InPokemon;

    public SwitchEvent(BattlePokemon InOutPokemon, BattlePokemon InInPokemon)
    {
        OutPokemon = InOutPokemon;
        InPokemon = InInPokemon;
    }

    public void PlayAnimation()
    {

    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        return true;
    }

    public void Process(BattleManager InManager)
    {

    }

    public EventType GetEventType()
    {
        return EventType.Switch;
    }

    public BattlePokemon GetOutPokemon()
    {
        return OutPokemon;
    }

    public BattlePokemon GetInPokemon()
    {
        return InPokemon;
    }
}

public class SingleBattleGameStartEvent : Event
{
    private BattlePokemon PlayerPokemon;
    private BattlePokemon EnemyPokemon;

    public SingleBattleGameStartEvent(BattlePokemon InPlayerPokemon, BattlePokemon InEnemyPokemon)
    {
        PlayerPokemon = InPlayerPokemon;
        EnemyPokemon = InEnemyPokemon;
    }

    public void PlayAnimation()
    {
        EditorLog.DebugLog("Play Enemy StepOut Animation.");
        EditorLog.DebugLog("Play Player StepOut Animation.");        
    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        return true;
    }

    public void Process(BattleManager InManager)
    {
        EditorLog.DebugLog("BattleStart");
        InManager.TranslateTimePoint(ETimePoint.BattleStart, this);
    }

    public EventType GetEventType()
    {
        return EventType.BattleStart;
    }

    public BattlePokemon GetPlayerPokemon()
    {
        return PlayerPokemon;
    }

    public BattlePokemon GetEnemyPokemon()
    {
        return EnemyPokemon;
    }
}