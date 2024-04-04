using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class SetBattleFieldStatusChangeEvent : EventAnimationPlayer, Event
{
    private EBattleFieldStatus StatusChangeType;
    private bool IsPlayerField;
    private BattleManager ReferenceBattleManager;
    private int StatusChangeTurn;
    private bool HasLimitedTime;
    public SetBattleFieldStatusChangeEvent(BattleManager InBattleManager, EBattleFieldStatus InStatusChangeType, int InStatusChangeTurn, bool InHasLimitedTime, bool InIsPlayerField)
    {
        ReferenceBattleManager = InBattleManager;  
        StatusChangeType = InStatusChangeType;
        IsPlayerField = InIsPlayerField;
        StatusChangeTurn = InStatusChangeTurn;
        HasLimitedTime = InHasLimitedTime;
    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        if(InBattleManager.GetBattleEnd() == true) return false;
        return true;
    }

    public EBattleFieldStatus GetStatusType()
    {
        return StatusChangeType;
    } 

    public override void InitAnimation()
    {
        TimelineAnimationManager Timelines = TimelineAnimationManager.GetGlobalTimelineAnimationManager();
        PlayableDirector MessageDirector = Timelines.MessageAnimation;
        string Message = "己方";
        if(IsPlayerField == false)
        {
            Message = "敌方";
        } 
        Message = Message + BattleFieldStatus.GetFieldDescription(StatusChangeType);       
        TimelineAnimation MessageTimeline = new TimelineAnimation(MessageDirector);
        MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", Message);
        AddAnimation(MessageTimeline);
    }

    public void Process(BattleManager InManager)
    {
        if(!ShouldProcess(InManager)) return;
        InManager.TranslateTimePoint(ETimePoint.BeforeSetBattleFieldStatusChange, this);
        InManager.AddBattleFieldStatus(IsPlayerField, StatusChangeType, HasLimitedTime, StatusChangeTurn);
        InManager.AddAnimationEvent(this);
        InManager.TranslateTimePoint(ETimePoint.AfterSetBattleFieldStatusChange, this);
    }

    public EventType GetEventType()
    {
        return EventType.SetBattleFieldStatusChange;
    }

}

public class RemoveBattleFieldStatusChangeEvent : EventAnimationPlayer, Event
{
    private EBattleFieldStatus StatusChangeType;
    private BattleManager ReferenceBattleManager;
    private string RemoveReason;
    private bool IsPlayerField;
    public RemoveBattleFieldStatusChangeEvent(BattleManager InBattleManager, EBattleFieldStatus InStatusChangeType, string InRemoveReason, bool InIsPlayerField)
    {
        ReferenceBattleManager = InBattleManager;
        StatusChangeType = InStatusChangeType;
        RemoveReason = InRemoveReason;
        IsPlayerField = InIsPlayerField;
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
        string Message = "己方场上的";
        if(IsPlayerField == false)
        {
            Message = "敌方场上的";
        }        
        string FieldName = BattleFieldStatus.GetFieldName(StatusChangeType);
        Message = Message + FieldName + "消失了!";
        TimelineAnimation MessageTimeline = new TimelineAnimation(MessageDirector);
        MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", Message);
        AddAnimation(MessageTimeline);
    }

    public void Process(BattleManager InManager)
    {
        if(!ShouldProcess(InManager)) return;
        InManager.RemoveBattleFieldStatus(IsPlayerField, StatusChangeType);
        InManager.AddAnimationEvent(this);
    }

    public EventType GetEventType()
    {
        return EventType.RemoveBattleFieldStatusChange;
    }
}