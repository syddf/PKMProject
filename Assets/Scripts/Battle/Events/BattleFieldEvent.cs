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
    private BattleFieldStatus ReferenceFieldStatus;
    private BattlePokemon SourcePokemon;
    public SetBattleFieldStatusChangeEvent(BattlePokemon InSourcePokemon, BattleManager InBattleManager, EBattleFieldStatus InStatusChangeType, int InStatusChangeTurn, bool InHasLimitedTime, bool InIsPlayerField)
    {
        ReferenceBattleManager = InBattleManager;  
        StatusChangeType = InStatusChangeType;
        IsPlayerField = InIsPlayerField;
        StatusChangeTurn = InStatusChangeTurn;
        HasLimitedTime = InHasLimitedTime;
        SourcePokemon = InSourcePokemon;
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
        if(ReferenceFieldStatus.BaseStatusChange != null)
        {
            ReferenceFieldStatus.BaseStatusChange.OnSetAnimation();
        }
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
        ReferenceFieldStatus = InManager.AddBattleFieldStatus(SourcePokemon, IsPlayerField, StatusChangeType, HasLimitedTime, StatusChangeTurn);
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
    private BattleFieldStatus ReferenceFieldStatus;
    private bool ShowMessage;
    public RemoveBattleFieldStatusChangeEvent(BattleManager InBattleManager, EBattleFieldStatus InStatusChangeType, string InRemoveReason, bool InIsPlayerField, bool InShowMessage = false)
    {
        ReferenceBattleManager = InBattleManager;
        StatusChangeType = InStatusChangeType;
        RemoveReason = InRemoveReason;
        IsPlayerField = InIsPlayerField;
        ShowMessage = InShowMessage;
    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        if(InBattleManager.GetBattleEnd() == true) return false;
        if(InBattleManager.HasBattleFieldStatus(IsPlayerField, StatusChangeType) == false) return false;
        return true;
    }

    public override void InitAnimation()
    {
        TimelineAnimationManager Timelines = TimelineAnimationManager.GetGlobalTimelineAnimationManager();
        PlayableDirector MessageDirector = Timelines.MessageAnimation;
        if(ReferenceFieldStatus.BaseStatusChange != null)
        {
            ReferenceFieldStatus.BaseStatusChange.OnRemoveAnimation();
        }
        if(ShowMessage)
        {
            string Message = "己方场上的";
            if(IsPlayerField == false)
            {
                Message = "敌方场上的";
            }        
            string FieldName = BattleFieldStatus.GetFieldName(StatusChangeType);
            Message = Message + FieldName + "消失了！";
            TimelineAnimation MessageTimeline = new TimelineAnimation(MessageDirector);
            MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", Message);
            AddAnimation(MessageTimeline);
        }
    }

    public void Process(BattleManager InManager)
    {
        if(!ShouldProcess(InManager)) return;
        ReferenceFieldStatus = InManager.RemoveBattleFieldStatus(IsPlayerField, StatusChangeType);
        InManager.AddAnimationEvent(this);
    }

    public EventType GetEventType()
    {
        return EventType.RemoveBattleFieldStatusChange;
    }
}