using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BattleEndEvent : EventAnimationPlayer, Event
{
    private bool Win;
    public BattleEndEvent(bool InWin)
    {
        Win = InWin;
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
        TimelineAnimation MessageTimeline = new TimelineAnimation(MessageDirector);
        if(Win)
        {
            MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", "你战胜了对手!");
        }
        else
        {
            MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", "你被击败了!");
        }
        AddAnimation(MessageTimeline);
    }

    public void Process(BattleManager InManager)
    {        
        if(!ShouldProcess(InManager)) return;
        InManager.AddAnimationEvent(this);
        InManager.SetBattleEnd(true);
    }

    public EventType GetEventType()
    {
        return EventType.BattleEnd;
    }
}
