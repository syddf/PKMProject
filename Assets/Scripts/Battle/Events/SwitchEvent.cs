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

public class SwitchWhenDefeatedEvent : EventAnimationPlayer, Event
{
    private BattlePokemon EnemyDefeatedPokemon;
    private BattlePokemon EnemyNewPokemon;
    private BattlePokemon PlayerDefeatedPokemon;
    private BattlePokemon PlayerNewPokemon;

    public SwitchWhenDefeatedEvent(BattlePokemon InEnemyDefeatedPokemon, BattlePokemon InEnemyNewPokemon, BattlePokemon InPlayerDefeatedPokemon, BattlePokemon InPlayerNewPokemon)
    {
        EnemyDefeatedPokemon = InEnemyDefeatedPokemon;
        EnemyNewPokemon = InEnemyNewPokemon;
        PlayerDefeatedPokemon = InPlayerDefeatedPokemon;
        PlayerNewPokemon = InPlayerNewPokemon;
    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        return true;
    }

    public void Process(BattleManager InManager)
    {
        InManager.AddAnimationEvent(this);
        if(PlayerNewPokemon != null)
        {
            InManager.SetNewPlayerPokemon(PlayerNewPokemon);
        }
        if(EnemyNewPokemon != null)
        {
            InManager.SetNewEnemyPokemon(EnemyNewPokemon);
        }
        InManager.TranslateTimePoint(ETimePoint.PokemonIn, this);
    }

    public override void InitAnimation()
    {
        TimelineAnimationManager Timelines = TimelineAnimationManager.GetGlobalTimelineAnimationManager();
        TimelineAnimation TargetTimeline = new TimelineAnimation(Timelines.SwitchWhenDefeatedAnimation);
        SubObjects SubObj = Timelines.SwitchWhenDefeatedAnimation.gameObject.GetComponent<SubObjects>();
        if(!PlayerNewPokemon)
        {
            TargetTimeline.SetTrackObject("Pokemon1", null);
            TargetTimeline.SetTrackObject("MonsterBall1", null);
            TargetTimeline.SetTrackObject("Explosion1", null);            
        }
        else
        {
            
            TargetTimeline.SetTrackObject("Pokemon1", PlayerNewPokemon.GetPokemonModel());
            TargetTimeline.SetTrackObject("MonsterBall1", SubObj.SubObject2);
            TargetTimeline.SetTrackObject("Explosion1", SubObj.SubObject1);    
            PlayerNewPokemon.GetPokemonModel().gameObject.transform.position = SubObj.SubObject5.transform.position;        
        }

        if(!EnemyNewPokemon)
        {
            TargetTimeline.SetTrackObject("Pokemon2", null);
            TargetTimeline.SetTrackObject("MonsterBall2", null);
            TargetTimeline.SetTrackObject("Explosion2", null);            
        }
        else
        {
            TargetTimeline.SetTrackObject("Pokemon2", EnemyNewPokemon.GetPokemonModel());
            TargetTimeline.SetTrackObject("MonsterBall2", SubObj.SubObject4);
            TargetTimeline.SetTrackObject("Explosion2", SubObj.SubObject3);            
            EnemyNewPokemon.GetPokemonModel().gameObject.transform.position = SubObj.SubObject6.transform.position;        
        }
        AddAnimation(TargetTimeline);
    }

    public EventType GetEventType()
    {
        return EventType.SwitchAfterDefeated;
    }

    public BattlePokemon GetPlayerNewPokemon()
    {
        return PlayerNewPokemon;
    }

    public BattlePokemon GetEnemyNewPokemon()
    {
        return EnemyNewPokemon;
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