using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchEvent : EventAnimationPlayer, Event
{
    private BattlePokemon OutPokemon;
    private BattlePokemon InPokemon;

    public SwitchEvent(BattlePokemon InOutPokemon, BattlePokemon InInPokemon)
    {
        OutPokemon = InOutPokemon;
        InPokemon = InInPokemon;
    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        return true;
    }

    public void Process(BattleManager InManager)
    {
        InManager.AddAnimationEvent(this);
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

public class SingleBattleGameStartEvent : EventAnimationPlayer, Event
{
    private BattlePokemon PlayerPokemon;
    private BattlePokemon EnemyPokemon;

    public SingleBattleGameStartEvent(BattlePokemon InPlayerPokemon, BattlePokemon InEnemyPokemon)
    {
        PlayerPokemon = InPlayerPokemon;
        EnemyPokemon = InEnemyPokemon;
    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        return true;
    }

    public void Process(BattleManager InManager)
    {
        InManager.AddAnimationEvent(this);
        InManager.TranslateTimePoint(ETimePoint.BattleStart, this);
    }

    public override void InitAnimation()
    {
        TimelineAnimationManager Timelines = TimelineAnimationManager.GetGlobalTimelineAnimationManager();
        TimelineAnimation TargetTimeline = new TimelineAnimation(Timelines.BattleStartAnimation);
        TargetTimeline.SetTrackObject("Pokemon1", PlayerPokemon.GetPokemonModel());
        TargetTimeline.SetTrackObject("Pokemon2", EnemyPokemon.GetPokemonModel());
        AddAnimation(TargetTimeline);
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