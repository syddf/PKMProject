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
    private BattlePokemon SourcePokemon;
    public SetPokemonStatusChangeEvent(BattlePokemon InReferencePokemon, BattlePokemon InSourcePokemon, BattleManager InBattleManager, EStatusChange InStatusChangeType, int InStatusChangeTurn, bool InTurnIsLimited)
    {
        ReferenceBattleManager = InBattleManager;
        ReferencePokemon = InReferencePokemon;   
        StatusChangeType = InStatusChangeType;
        StatusChangeTurn= InStatusChangeTurn;
        TurnIsLimited = InTurnIsLimited;
        SourcePokemon = InSourcePokemon;
    }

    public static bool IsStatusChangeEffective(BattleManager InBattleManager, BattlePokemon ReferencePokemon, BattlePokemon SourcePokemon, EStatusChange StatusChangeType)
    {
        if(ReferencePokemon.IsDead() == true) return false;
        if(ReferencePokemon.HasStatusChange(StatusChangeType)) return false;
        if(InBattleManager.GetTerrainType() == EBattleFieldTerrain.Electric && ReferencePokemon.IsGroundPokemon(InBattleManager) && StatusChangeType == EStatusChange.Drowsy) return false;
        if(InBattleManager.HasBattleFieldStatus(!ReferencePokemon.GetIsEnemy(), EBattleFieldStatus.Safeguard) && StatusChange.IsStatusChange(StatusChangeType)) return false;
        if(ReferencePokemon.HasAbility("精神力", InBattleManager, SourcePokemon, ReferencePokemon) && StatusChangeType == EStatusChange.Flinch) return false;
        if(ReferencePokemon.HasType(EType.Fire, InBattleManager, null, null) && StatusChangeType == EStatusChange.Burn) return false;
        if(ReferencePokemon.HasType(EType.Ice, InBattleManager, null, null) && StatusChangeType == EStatusChange.Frostbite) return false;
        if(ReferencePokemon.HasType(EType.Electric, InBattleManager, null, null) && StatusChangeType == EStatusChange.Paralysis) return false;
        if(InBattleManager.GetWeatherType() == EWeather.SunLight && StatusChangeType == EStatusChange.Frostbite) return false;
        if(ReferencePokemon.HasType(EType.Grass, InBattleManager, null, null) && StatusChangeType == EStatusChange.LeechSeed) return false;
        if((ReferencePokemon.HasType(EType.Poison, InBattleManager, null, null) || ReferencePokemon.HasType(EType.Steel, InBattleManager, null, null) ) && StatusChangeType == EStatusChange.Poison) return false;
        return true;
    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        if(InBattleManager.GetBattleEnd() == true) return false;
        if(IsStatusChangeEffective(InBattleManager, ReferencePokemon, SourcePokemon, StatusChangeType) == false)
        {
            return false;
        }
        if(ReferencePokemon.IsDead()) return false;
        return true;
    }

    public string GetSetMessageText()
    {
        if(StatusChangeType == EStatusChange.ThroatChop)
        {
            return "痛苦得难以发出声音！";
        }
        if(StatusChangeType == EStatusChange.Protect)
        {
            return "保护了自己！";
        }
        if(StatusChangeType == EStatusChange.Poison)
        {
            return "中毒了！";
        }
        if(StatusChangeType == EStatusChange.Paralysis)
        {
            return "麻痹了！";
        }
        if(StatusChangeType == EStatusChange.ForbidHeal)
        {
            return "陷入无法回复的状态了！";
        }
        if(StatusChangeType == EStatusChange.Flinch)
        {
            return "畏缩了！";
        }
        if(StatusChangeType == EStatusChange.Taunt)
        {
            return "被挑衅了！无法使用变化类招式！";
        }
        if(StatusChangeType == EStatusChange.Burn)
        {
            return "被烧伤了！";
        }
        if(StatusChangeType == EStatusChange.Frostbite)
        {
            return "被冻伤了！";
        }
        if(StatusChangeType == EStatusChange.Drowsy)
        {
            return "感觉到有点困倦了！";
        }
        if(StatusChangeType == EStatusChange.LeechSeed)
        {
            return "身上缠绕了寄生种子！";
        }
        if(StatusChangeType == EStatusChange.Confusion)
        {
            return "混乱了！";
        }
        return "";
    }

    public EStatusChange GetStatusType()
    {
        return StatusChangeType;
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
        string SetMessage = GetSetMessageText();
        if(!Forbidden)
        {
            Message = ReferencePokemon.GetName() + SetMessage;
            if(SetMessage == "")
            {
                return;
            }
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
                if(StatusChangeType == EStatusChange.Confusion)
                {
                    StatusChangeAnimationFakeEvent FakeEvent = new StatusChangeAnimationFakeEvent(ReferencePokemon, EStatusChange.Confusion);
                    FakeEvent.Process(InManager);
                }
                
            }
        }
        CloneInPokemon = ReferencePokemon.CloneBattlePokemonStats();
        if(!SameAsOrigin)
        {
            InManager.AddAnimationEvent(this);
            InManager.TranslateTimePoint(ETimePoint.AfterSetPokemonStatusChange, this);
        }
    }

    public EventType GetEventType()
    {
        return EventType.SetPokemonStatusChange;
    }
    public BattlePokemon GetReferencePokemon()
    {
        return ReferencePokemon;
    }

}

public class RemovePokemonStatusChangeEvent : EventAnimationPlayer, Event
{
    private EStatusChange StatusChangeType;
    private BattlePokemon ReferencePokemon;
    private BattleManager ReferenceBattleManager;
    private string RemoveReason = "";
    private BattlePokemonStat CloneInPokemon;
    public RemovePokemonStatusChangeEvent(BattlePokemon InSourcePokemon, BattleManager InBattleManager, EStatusChange InStatusChangeType, string InRemoveReason)
    {
        ReferenceBattleManager = InBattleManager;
        ReferencePokemon = InSourcePokemon;   
        StatusChangeType = InStatusChangeType;
        RemoveReason = InRemoveReason;
    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        if(InBattleManager.GetBattleEnd() == true) return false;
        if(ReferencePokemon.IsDead()) return false;
        return true;
    }

    public BattlePokemon GetReferencePokemon()
    {
        return ReferencePokemon;
    }
    public string GetRemoveMessageText()
    {
        if(StatusChangeType == EStatusChange.Poison)
        {
            return "解除了中毒！";
        }
        if(StatusChangeType == EStatusChange.Paralysis)
        {
            return "解除了麻痹！";
        }
        if(StatusChangeType == EStatusChange.Burn)
        {
            return "解除了烧伤！";
        }
        if(StatusChangeType == EStatusChange.Frostbite)
        {
            return "解除了冻伤！";
        }
        if(StatusChangeType == EStatusChange.Drowsy)
        {
            return "解除了瞌睡！";
        }
        if(StatusChangeType == EStatusChange.Confusion)
        {
            return "解除了混乱！";
        }
        if(StatusChangeType == EStatusChange.Taunt)
        {
            return "解除了挑衅！";
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
        string Desc = GetRemoveMessageText();
        if(Desc != "")
        {
            string Message = ReferencePokemon.GetName() + "因" + RemoveReason + Desc;
            if(RemoveReason == "")
            {
                Message = ReferencePokemon.GetName() + Desc;
            }
            TimelineAnimation MessageTimeline = new TimelineAnimation(MessageDirector);
            MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", Message);
            AddAnimation(MessageTimeline);
        }
    }

    public void Process(BattleManager InManager)
    {
        if(!ShouldProcess(InManager)) return;
        ReferencePokemon.RemoveStatusChange(StatusChangeType);
        CloneInPokemon = ReferencePokemon.CloneBattlePokemonStats();
        InManager.AddAnimationEvent(this);
    }

    public EventType GetEventType()
    {
        return EventType.RemovePokemonStatusChange;
    }
}