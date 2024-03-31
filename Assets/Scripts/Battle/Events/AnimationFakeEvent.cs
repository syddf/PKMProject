using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class StatusChangeAnimationFakeEvent : EventAnimationPlayer, Event
{
    private BattlePokemon ReferencePokemon;
    private EStatusChange StatusType;
    public StatusChangeAnimationFakeEvent(BattlePokemon InSourcePokemon, EStatusChange InStatusType)
    {
        ReferencePokemon = InSourcePokemon;
        StatusType = InStatusType;
    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        if(InBattleManager.GetBattleEnd() == true) return false;
        return true;
    }

    public override void InitAnimation()
    {
        TimelineAnimationManager Timelines = TimelineAnimationManager.GetGlobalTimelineAnimationManager();
        PlayableDirector MessageDirector = null;
        if(StatusType == EStatusChange.Poison)
        {
            MessageDirector = Timelines.PoisonStatusAnimation;
        }
        MessageDirector.gameObject.transform.position = ReferencePokemon.GetPokemonModel().transform.position;
        TimelineAnimation StatusAnimation = new TimelineAnimation(MessageDirector);                                
        StatusAnimation.SetSignalReceiver("SourcePokemon", ReferencePokemon.GetPokemonModel());
        AddAnimation(StatusAnimation);
    }

    public void Process(BattleManager InManager)
    {
        if(!ShouldProcess(InManager)) return;
        InManager.AddAnimationEvent(this);
    }

    public EventType GetEventType()
    {
        return EventType.Fake;
    }
}