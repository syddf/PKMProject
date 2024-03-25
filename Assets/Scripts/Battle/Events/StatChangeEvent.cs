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
    public StatChangeEvent(BattlePokemon InTargetPokemon, string InChangedStatName, int InChangedStatLevel)
    {
        TargetPokemon = InTargetPokemon;
        ChangedStatName = InChangedStatName;
        ChangedStatLevel = InChangedStatLevel;
        ShouldChange = true;
        ReverseChangeLevel = false;
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
        if(Stat == "CT") return "集中要害率";
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

        return PokemonName + "的" + StatName + Description + "!";
    }

    public override void InitAnimation()
    {
        TimelineAnimationManager Timelines = TimelineAnimationManager.GetGlobalTimelineAnimationManager();
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
            MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", "能力不能再进一步变化了!");
            AddAnimation(MessageTimeline);
        }
    }

    public void Process(BattleManager InManager)
    {
        if(!ShouldProcess(InManager)) return;
        InManager.TranslateTimePoint(ETimePoint.BeforeStatChange, this);
        if(ShouldChange && GetChangeLevel() != 0)
        {
            InManager.AddAnimationEvent(this);

            if(TargetPokemon.ChangeStat(ChangedStatName, GetChangeLevel()))
            {
                InManager.TranslateTimePoint(ETimePoint.AfterStatChange, this);
                ChangedSuccessed = true;
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
}
