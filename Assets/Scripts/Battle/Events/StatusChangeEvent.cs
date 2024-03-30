using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class SetPokemonStatusChangeEvent : EventAnimationPlayer, Event
{
    private EStatusChange StatusChangeType;
    private BattlePokemon ReferencePokemon;
    private bool TurnIsLimited;
    private BattleManager ReferenceBattleManager;
    private int StatusChangeTurn;
    private bool Forbidden = false;
    private string ForbiddenReason = "";
    public SetPokemonStatusChangeEvent(BattlePokemon InSourcePokemon, BattleManager InBattleManager, EStatusChange InStatusChangeType, int InStatusChangeTurn, bool InTurnIsLimited)
    {
        ReferenceBattleManager = InBattleManager;
        ReferencePokemon = InSourcePokemon;   
        StatusChangeType = InStatusChangeType;
        StatusChangeTurn= InStatusChangeTurn;
        TurnIsLimited = InTurnIsLimited;
    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        if(InBattleManager.GetBattleEnd() == true) return false;
        if(ReferencePokemon.IsDead()) return false;
        return true;
    }

    public override void InitAnimation()
    {
        TimelineAnimationManager Timelines = TimelineAnimationManager.GetGlobalTimelineAnimationManager();
        PlayableDirector MessageDirector = Timelines.MessageAnimation;
        string Message = ReferencePokemon.GetName() + ForbiddenReason;
        if(!Forbidden)
        {
            Message = ReferencePokemon.GetBaseStatusChange(StatusChangeType).GetSetMessageText();
        }
        TimelineAnimation MessageTimeline = new TimelineAnimation(MessageDirector);
        MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", Message);
        AddAnimation(MessageTimeline);
    }

    public void Process(BattleManager InManager)
    {
        if(!ShouldProcess(InManager)) return;
        InManager.TranslateTimePoint(ETimePoint.BeforeSetPokemonStatusChange, this);
        if(!Forbidden)
        {
            ReferencePokemon.AddStatusChange(StatusChangeType, TurnIsLimited, StatusChangeTurn);
        }
        InManager.AddAnimationEvent(this);
        InManager.TranslateTimePoint(ETimePoint.AfterSetPokemonStatusChange, this);
    }

    public EventType GetEventType()
    {
        return EventType.PokemonStatusChange;
    }

}
