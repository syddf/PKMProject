using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwitchEvent : EventAnimationPlayer, Event
{
    private BattlePokemon OutPokemon;
    private BattlePokemon InPokemon;
    private BattleManager ReferenceManager;
    private BattlePokemonStat CloneInPokemon;
    private int HP;
    public SwitchEvent(BattleManager InManager, BattlePokemon InOutPokemon, BattlePokemon InInPokemon)
    {
        OutPokemon = InOutPokemon;
        InPokemon = InInPokemon;
        ReferenceManager = InManager;
    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        if(InBattleManager.GetBattleEnd() == true) return false;
        return true;
    }

    public override void InitAnimation()
    {
        TimelineAnimationManager Timelines = TimelineAnimationManager.GetGlobalTimelineAnimationManager();        
        TimelineAnimation TargetTimeline = new TimelineAnimation(Timelines.SwitchAnimation);
        SubObjects SubObj = Timelines.SwitchWhenDefeatedAnimation.gameObject.GetComponent<SubObjects>();
        InPokemon.GetPokemonModel().transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        if(InPokemon.GetIsEnemy())
        {
            TargetTimeline.SetTrackObject("Pokemon1", null);
            TargetTimeline.SetTrackObject("MonsterBall1", null);
            TargetTimeline.SetTrackObject("Explosion1", null);            
        }
        else
        {            
            TargetTimeline.SetTrackObject("Pokemon1", InPokemon.GetPokemonModel());
            TargetTimeline.SetTrackObject("MonsterBall1", SubObj.SubObject2);
            TargetTimeline.SetTrackObject("Explosion1", SubObj.SubObject1);    
            InPokemon.GetPokemonModel().gameObject.transform.position = SubObj.SubObject5.transform.position;        
        }

        if(!InPokemon.GetIsEnemy())
        {
            TargetTimeline.SetTrackObject("Pokemon2", null);
            TargetTimeline.SetTrackObject("MonsterBall2", null);
            TargetTimeline.SetTrackObject("Explosion2", null);            
        }
        else
        {
            TargetTimeline.SetTrackObject("Pokemon2", InPokemon.GetPokemonModel());
            TargetTimeline.SetTrackObject("MonsterBall2", SubObj.SubObject4);
            TargetTimeline.SetTrackObject("Explosion2", SubObj.SubObject3);            
            InPokemon.GetPokemonModel().gameObject.transform.position = SubObj.SubObject6.transform.position;        
        }

        TargetTimeline.SetTrackObject("PokemonActivation", OutPokemon.GetPokemonModel());
        TargetTimeline.SetSignalReceiver("PokemonAnim", OutPokemon.GetPokemonModel());
        if(OutPokemon.GetIsEnemy())
        {
            TargetTimeline.SetTrackObject("Laser", Timelines.SwitchAnimation.gameObject.GetComponent<SubObjects>().SubObject3);
        }
        else
        {
            TargetTimeline.SetTrackObject("Laser", Timelines.SwitchAnimation.gameObject.GetComponent<SubObjects>().SubObject2);
        }
        Timelines.SwitchAnimation.gameObject.GetComponent<SubObjects>().SubObject1.
        GetComponent<PositionWithObject>().target = OutPokemon.GetPokemonModel().GetComponent<PokemonReceiver>().BodyTransform;        
        AddAnimation(TargetTimeline);
    }
    public override void OnAnimationFinished()
    {
        ReferenceManager.UpdateUI(false);
        ReferenceManager.UpdatePokemonInfo(InPokemon, CloneInPokemon);
    }

    public void Process(BattleManager InManager)
    {
        if(!ShouldProcess(InManager)) return;
        InManager.AddAnimationEvent(this);
        if(!InPokemon.GetIsEnemy())
        {
            InManager.SetNewPlayerPokemon(InPokemon);
        }
        else
        {
            InManager.SetNewEnemyPokemon(InPokemon);
        }
        CloneInPokemon = InPokemon.CloneBattlePokemonStats();
        InManager.TranslateTimePoint(ETimePoint.PokemonIn, this);
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
    private BattleManager ReferenceManager;
    private BattlePokemonStat CloneEnemyNewPokemonStats;
    private BattlePokemonStat ClonePlayerNewPokemonStats;
    public SwitchWhenDefeatedEvent(BattleManager InManager, BattlePokemon InEnemyDefeatedPokemon, BattlePokemon InEnemyNewPokemon, BattlePokemon InPlayerDefeatedPokemon, BattlePokemon InPlayerNewPokemon)
    {
        EnemyDefeatedPokemon = InEnemyDefeatedPokemon;
        EnemyNewPokemon = InEnemyNewPokemon;
        PlayerDefeatedPokemon = InPlayerDefeatedPokemon;
        PlayerNewPokemon = InPlayerNewPokemon;
        ReferenceManager = InManager;
        if(EnemyNewPokemon)
            CloneEnemyNewPokemonStats = EnemyNewPokemon.CloneBattlePokemonStats();
        if(PlayerNewPokemon)
            ClonePlayerNewPokemonStats = PlayerNewPokemon.CloneBattlePokemonStats();
    }

    public override void OnAnimationFinished()
    {
        ReferenceManager.UpdateUI(false);
        if(EnemyNewPokemon)
            ReferenceManager.UpdatePokemonInfo(EnemyNewPokemon, CloneEnemyNewPokemonStats);
        if(PlayerNewPokemon)
            ReferenceManager.UpdatePokemonInfo(PlayerNewPokemon, ClonePlayerNewPokemonStats);
    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        if(InBattleManager.GetBattleEnd() == true) return false;
        return true;
    }

    public void Process(BattleManager InManager)
    {
        if(!ShouldProcess(InManager)) return;
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
            PlayerNewPokemon.GetPokemonModel().transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
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
            EnemyNewPokemon.GetPokemonModel().transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
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

        if(InBattleManager.GetBattleEnd() == true) return false;
        return true;
    }

    public void Process(BattleManager InManager)
    {
        if(!ShouldProcess(InManager)) return;
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