using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;


public class ActiveHalfSecondAnimationEvent : EventAnimationPlayer, Event
{
    private GameObject TargetObject;
    public ActiveHalfSecondAnimationEvent(GameObject InTargetObject)
    {
        TargetObject = InTargetObject;
    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        if(InBattleManager.GetBattleEnd() == true) return false;
        return true;
    }

    public override void InitAnimation()
    {
        TimelineAnimationManager Timelines = TimelineAnimationManager.GetGlobalTimelineAnimationManager();
        PlayableDirector HalfSecondActiveDirector = Timelines.HalfSecondActiveAnimation;
        TimelineAnimation HalfSecondActiveTimeline = new TimelineAnimation(HalfSecondActiveDirector);
        HalfSecondActiveTimeline.SetTrackObject("Object", TargetObject);
        AddAnimation(HalfSecondActiveTimeline);
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

public class ChatAnimationFakeEvent : EventAnimationPlayer, Event
{
    private List<string> MessageList;
    private List<bool> IsPlayerList;
    public static Event AddChatEvent(List<string> MessageList, bool IsPlayer)
    {
        List<bool> IsPlayerList = new List<bool>();
        for(int Index = 0; Index < MessageList.Count; Index++)
            IsPlayerList.Add(IsPlayer);

        ChatAnimationFakeEvent NewEvent = new ChatAnimationFakeEvent(MessageList, IsPlayerList);

        return NewEvent;
    }
    public ChatAnimationFakeEvent(List<string> InMessageList, List<bool> InIsPlayer)
    {
        MessageList = InMessageList;
        IsPlayerList = InIsPlayer;
    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        if(InBattleManager.GetBattleEnd() == true) return false;
        return true;
    }

    public override void InitAnimation()
    {
        TimelineAnimationManager Timelines = TimelineAnimationManager.GetGlobalTimelineAnimationManager();
        PlayableDirector ChatDirector = Timelines.ChatAnimation;
        for(int Index = 0; Index < MessageList.Count; Index++)
        {
            TimelineAnimation ChatTimeline = new TimelineAnimation(ChatDirector);
            ChatTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", MessageList[Index]);
            ChatTimeline.SetSignalParameter("SignalObject", "MessageSignal", "Trainer", IsPlayerList[Index] ? "Player" : "Enemy");
            AddAnimation(ChatTimeline);
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
        if(StatusType == EStatusChange.LeechSeed)
        {
            MessageDirector = Timelines.LeechSeedDamageAnimation;
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