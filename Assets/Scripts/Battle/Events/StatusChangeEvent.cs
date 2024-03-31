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

    public string GetSetMessageText()
    {
        if(StatusChangeType == EStatusChange.ThroatChop)
        {
            return "痛苦得难以发出声音!";
        }
        if(StatusChangeType == EStatusChange.Protect)
        {
            return "保护了自己!";
        }
        if(StatusChangeType == EStatusChange.Poison)
        {
            return "中毒了!";
        }
        if(StatusChangeType == EStatusChange.ForbidHeal)
        {
            return "陷入无法回复的状态了!";
        }
        if(StatusChangeType == EStatusChange.Flinch)
        {
            return "畏缩了!";
        }
        return "";
    }

    public override void InitAnimation()
    {
        TimelineAnimationManager Timelines = TimelineAnimationManager.GetGlobalTimelineAnimationManager();
        PlayableDirector MessageDirector = Timelines.MessageAnimation;
        string Message = ReferencePokemon.GetName() + ForbiddenReason;
        if(!Forbidden)
        {
            Message = ReferencePokemon.GetName() + GetSetMessageText();
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
