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
    private BattlePokemonStat CloneInPokemon;
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
        if(ReferencePokemon.HasType(EType.Fire) && StatusChangeType == EStatusChange.Burn) return false;
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
        if(StatusChangeType == EStatusChange.Paralysis)
        {
            return "麻痹了!";
        }
        if(StatusChangeType == EStatusChange.ForbidHeal)
        {
            return "陷入无法回复的状态了!";
        }
        if(StatusChangeType == EStatusChange.Flinch)
        {
            return "畏缩了!";
        }
        if(StatusChangeType == EStatusChange.Burn)
        {
            return "被烧伤了!";
        }
        if(StatusChangeType == EStatusChange.Frostbite)
        {
            return "被冻伤了!";
        }
        if(StatusChangeType == EStatusChange.Drowsy)
        {
            return "感觉到有点困倦了!";
        }
        return "";
    }
    
    public override void OnAnimationFinished()
    {
        ReferenceBattleManager.UpdatePokemonStatusChange(ReferencePokemon, CloneInPokemon);
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
        bool SameAsOrigin = false;
        if(!Forbidden)
        {
            SameAsOrigin = ReferencePokemon.AddStatusChange(StatusChangeType, TurnIsLimited, StatusChangeTurn);
            if(!SameAsOrigin)
            {
                if(StatusChangeType == EStatusChange.Frostbite)
                {
                    StatusChangeAnimationFakeEvent FakeEvent = new StatusChangeAnimationFakeEvent(ReferencePokemon, EStatusChange.Frostbite);
                    FakeEvent.Process(InManager);
                }
                if(StatusChangeType == EStatusChange.Burn)
                {
                    StatusChangeAnimationFakeEvent FakeEvent = new StatusChangeAnimationFakeEvent(ReferencePokemon, EStatusChange.Burn);
                    FakeEvent.Process(InManager);
                }
                if(StatusChangeType == EStatusChange.Poison)
                {
                    StatusChangeAnimationFakeEvent FakeEvent = new StatusChangeAnimationFakeEvent(ReferencePokemon, EStatusChange.Poison);
                    FakeEvent.Process(InManager);
                }
                if(StatusChangeType == EStatusChange.Paralysis)
                {
                    StatusChangeAnimationFakeEvent FakeEvent = new StatusChangeAnimationFakeEvent(ReferencePokemon, EStatusChange.Paralysis);
                    FakeEvent.Process(InManager);
                }
                if(StatusChangeType == EStatusChange.Drowsy)
                {
                    StatusChangeAnimationFakeEvent FakeEvent = new StatusChangeAnimationFakeEvent(ReferencePokemon, EStatusChange.Drowsy);
                    FakeEvent.Process(InManager);
                }
            }
        }
        if(!SameAsOrigin)
        {
            InManager.AddAnimationEvent(this);
        }
        CloneInPokemon = ReferencePokemon.CloneBattlePokemonStats();
        InManager.TranslateTimePoint(ETimePoint.AfterSetPokemonStatusChange, this);
    }

    public EventType GetEventType()
    {
        return EventType.PokemonStatusChange;
    }

}
