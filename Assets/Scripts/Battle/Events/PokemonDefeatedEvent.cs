using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonDefeatedEvent : EventAnimationPlayer, Event
{
    private BattlePokemon TargetPokemon;
    private BattlePokemon SourcePokemon;
    private BattleSkill SourceSkill;

    public PokemonDefeatedEvent(BattlePokemon InTargetPokemon, BattlePokemon InSourcePokemon, BattleSkill InSkill = null)
    {
        TargetPokemon = InTargetPokemon;
        SourcePokemon = InSourcePokemon;
        SourceSkill = InSkill;
    }
    public bool ShouldProcess(BattleManager InBattleManager)
    {
        if(InBattleManager.GetBattleEnd() == true) return false;
        return true;
    }

    public override void InitAnimation()
    {
        TimelineAnimationManager Timelines = TimelineAnimationManager.GetGlobalTimelineAnimationManager();
        TimelineAnimation DefeatedTimeline = new TimelineAnimation(Timelines.DefeatedAnimation);

        DefeatedTimeline.SetTrackObject("PokemonActivation", TargetPokemon.GetPokemonModel());
        DefeatedTimeline.SetSignalReceiver("PokemonAnim", TargetPokemon.GetPokemonModel());
        if(TargetPokemon.GetIsEnemy())
        {
            DefeatedTimeline.SetTrackObject("Laser", Timelines.DefeatedAnimation.gameObject.GetComponent<SubObjects>().SubObject2);
        }
        else
        {
            DefeatedTimeline.SetTrackObject("Laser", Timelines.DefeatedAnimation.gameObject.GetComponent<SubObjects>().SubObject1);
        }
        Timelines.DefeatedAnimation.gameObject.GetComponent<SubObjects>().SubObject3.
        GetComponent<PositionWithObject>().target = TargetPokemon.GetPokemonModel().GetComponent<PokemonReceiver>().BodyTransform;
        DefeatedTimeline.SetSignalParameter("PokemonCry", "AudioSignal", "Pokemon", TargetPokemon.GetEnName());
        //TargetTimeline.SetSignalParameter("SignalObject", "AbilityTriggerSignal", "AbilityName", SourceAbility.GetAbilityName());
        AddAnimation(DefeatedTimeline);

        TimelineAnimation MessageTimeline = new TimelineAnimation(Timelines.MessageAnimation);
        MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", TargetPokemon.GetName() + "被击败了！");
        AddAnimation(MessageTimeline);
    }

    public void Process(BattleManager InManager)
    {
        if(!ShouldProcess(InManager)) return;
        InManager.TranslateTimePoint(ETimePoint.BeforePokemonDefeated, this);
        InManager.AddAnimationEvent(this);
        InManager.AddDefeatedPokemon(TargetPokemon, SourcePokemon);
        if(InManager.BattleEndIfPokemonDefeated(TargetPokemon))
        {
            bool Win = TargetPokemon.GetIsEnemy();
            BattleEndEvent battleEndEvent = new BattleEndEvent(Win);
            battleEndEvent.Process(InManager);
        }
        else
        {
            InManager.TranslateTimePoint(ETimePoint.AfterPokemonDefeated, this);
        }      
    }

    public EventType GetEventType()
    {
        return EventType.PokemonDefeated;
    }

    public BattlePokemon GetTargetPokemon() => TargetPokemon;
    public BattlePokemon GetSourcePokemon() => SourcePokemon;
}