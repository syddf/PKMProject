using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;


public class MessageAnimationFakeEvent : EventAnimationPlayer, Event
{
    private List<string> MessageList;
    public MessageAnimationFakeEvent(List<string> InMessageList)
    {
        MessageList = InMessageList;
    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        if(InBattleManager.GetBattleEnd() == true) return false;
        return true;
    }

    public override void InitAnimation()
    {
        TimelineAnimationManager Timelines = TimelineAnimationManager.GetGlobalTimelineAnimationManager();
        PlayableDirector MessageDirector = Timelines.MessageAnimation;
        foreach(var MessageString in MessageList)
        {
            TimelineAnimation MessageTimeline = new TimelineAnimation(MessageDirector);
            MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", MessageString);
            AddAnimation(MessageTimeline);
        }
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
            MessageDirector.gameObject.transform.position = ReferencePokemon.GetPokemonModel().transform.position;
        }
        if(StatusType == EStatusChange.Paralysis)
        {
            MessageDirector = Timelines.ParalysisStatusAnimation;
            MessageDirector.gameObject.GetComponent<DistributeObjects>().Distribute(ReferencePokemon.GetPokemonModel().gameObject);
        }
        if(StatusType == EStatusChange.Burn)
        {
            MessageDirector = Timelines.BurnStatusAnimation;
            MessageDirector.gameObject.GetComponent<DistributeObjects>().Distribute(ReferencePokemon.GetPokemonModel().gameObject);
        }
        if(StatusType == EStatusChange.Frostbite)
        {
            MessageDirector = Timelines.FrostbiteStatusAnimation;
            MessageDirector.gameObject.GetComponent<DistributeObjects>().Distribute(ReferencePokemon.GetPokemonModel().gameObject);
        }
        if(StatusType == EStatusChange.Drowsy)
        {
            MessageDirector = Timelines.DrowsyStatusAnimation;
            MessageDirector.gameObject.transform.position = ReferencePokemon.GetPokemonModel().transform.position;
        }
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