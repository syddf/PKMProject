using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class StatChangeEvent : EventAnimationPlayer, Event
{
    private BattlePokemon TargetPokemon;
    private string ChangedStatName;
    private int ChangedStatLevel;
    private bool ShouldChange;
    private bool ReverseChangeLevel;
    private bool ChangedSuccessed = false;
    private string Reason = "";
    public StatChangeEvent(BattlePokemon InTargetPokemon, string InChangedStatName, int InChangedStatLevel, string InReason = "")
    {
        TargetPokemon = InTargetPokemon;
        ChangedStatName = InChangedStatName;
        ChangedStatLevel = InChangedStatLevel;
        ShouldChange = true;
        ReverseChangeLevel = false;
        Reason = InReason;
    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        if(InBattleManager.GetBattleEnd() == true) return false;
        return true;
    }

    public static string GetStatName(string Stat)
    {
        if(Stat == "Atk") return "攻击";
        if(Stat == "Def") return "物防";
        if(Stat == "SAtk") return "特攻";
        if(Stat == "SDef") return "特防";
        if(Stat == "Speed") return "速度";
        if(Stat == "Accuracyrate") return "命中率";
        if(Stat == "Evasionrate") return "闪避率";
        if(Stat == "CT") return "击中要害率";
        return "";
    }

    public static string GetChangeDescription(int val)
    {
        if(val == 1) return "提高了";
        if(val == 2) return "大幅提高了";
        if(val >= 3) return "巨幅提高了";
        if(val == -1) return "降低了";
        if(val == -2) return "大幅降低了";
        if(val <= -3) return "巨幅降低了";        
        return "";
    }

    public string GetMessageText()
    {
        string PokemonName = TargetPokemon.GetName();
        string StatName = GetStatName(ChangedStatName);
        string Description = GetChangeDescription(ChangedStatLevel);
        string ReasonString = "因" + Reason;
        if(Reason == "")
        {
            ReasonString = "";   
        }
        return PokemonName + "的" + StatName + ReasonString + Description + "！";
    }

    public override void InitAnimation()
    {
        TimelineAnimationManager Timelines = TimelineAnimationManager.GetGlobalTimelineAnimationManager();
        if(!ShouldChange)
        {
            PlayableDirector MessageDirector = Timelines.MessageAnimation;
            TimelineAnimation MessageTimeline = new TimelineAnimation(MessageDirector);
            string Desc = GetChangeLevel() > 0 ? "提高" : "降低";
            MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", TargetPokemon.GetName() + "的" + GetStatName(ChangedStatName) + "无法" + Desc + "！");
            AddAnimation(MessageTimeline);
            return;
        }
        if(ChangedSuccessed)
        {
            PlayableDirector AnimDirector = Timelines.DebuffAnimation;
            if(GetChangeLevel() > 0)
            {
                AnimDirector = Timelines.BuffAnimation;
            }
            AnimDirector.gameObject.transform.position = TargetPokemon.GetPokemonModel().transform.position;
            TimelineAnimation TargetTimeline = new TimelineAnimation(AnimDirector);
            AddAnimation(TargetTimeline);

            PlayableDirector MessageDirector = Timelines.MessageAnimation;
            TimelineAnimation MessageTimeline = new TimelineAnimation(MessageDirector);
            MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", GetMessageText());
            AddAnimation(MessageTimeline);
        }
        else
        {
            PlayableDirector MessageDirector = Timelines.MessageAnimation;
            TimelineAnimation MessageTimeline = new TimelineAnimation(MessageDirector);
            MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", "能力不能再进一步变化了！");
            AddAnimation(MessageTimeline);
        }
    }

    public void Process(BattleManager InManager)
    {
        if(!ShouldProcess(InManager)) return;
        InManager.TranslateTimePoint(ETimePoint.BeforeStatChange, this);
        if(GetChangeLevel() != 0)
        {
            InManager.AddAnimationEvent(this);
            if(ShouldChange)
            {
                if(TargetPokemon.ChangeStat(ChangedStatName, GetChangeLevel()))
                {
                    InManager.TranslateTimePoint(ETimePoint.AfterStatChange, this);
                    ChangedSuccessed = true;
                }
            }
        }
    }

    public EventType GetEventType()
    {
        return EventType.StatChange;
    }

    public BattlePokemon GetTargetPokemon() => TargetPokemon;
    public int GetChangeLevel()
    {
        int Factor = 1;
        if(ReverseChangeLevel)
        {
            Factor = -1;
        }
        return Factor * ChangedStatLevel;
    }
    public void SetReverseLevel() { ReverseChangeLevel = true;}
    public string GetChangeStatName() { return ChangedStatName; }
    public void ForbidChange() { ShouldChange = false;}
}
