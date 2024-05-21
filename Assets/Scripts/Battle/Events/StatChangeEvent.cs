using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class StatChangeEvent : EventAnimationPlayer, Event
{
    private BattlePokemon TargetPokemon;
    private BattlePokemon SourcePokemon;
    private List<string> ChangedStatName;
    private List<int> ChangedStatLevel;
    private List<bool> ShouldChangeList;
    private bool ReverseChangeLevel;
    private List<bool> ChangedSuccessedList;
    private string Reason = "";

    public StatChangeEvent(BattlePokemon InTargetPokemon, BattlePokemon InSourcePokemon, List<string> InChangedStatName, List<int> InChangedStatLevel, string InReason = "")
    {
        SourcePokemon = InSourcePokemon;
        TargetPokemon = InTargetPokemon;
        ChangedStatName = InChangedStatName;
        ChangedStatLevel = InChangedStatLevel;

        ShouldChangeList = new List<bool>();
        ChangedSuccessedList = new List<bool>();
        for(int Index = 0; Index < ChangedStatName.Count; Index++)
        {
            ShouldChangeList.Add(true);
            ChangedSuccessedList.Add(false);
        }

        ReverseChangeLevel = false;
        Reason = InReason;
    }

    public StatChangeEvent(BattlePokemon InTargetPokemon, BattlePokemon InSourcePokemon, string InChangedStatName, int InChangedStatLevel, string InReason = "")
    {
        TargetPokemon = InTargetPokemon;
        ChangedStatName = new List<string>();
        ChangedStatName.Add(InChangedStatName);
        ChangedStatLevel = new List<int>();
        ChangedStatLevel.Add(InChangedStatLevel);
        ShouldChangeList = new List<bool>();
        ShouldChangeList.Add(true);
        ChangedSuccessedList = new List<bool>();
        ChangedSuccessedList.Add(false);

        ReverseChangeLevel = false;
        Reason = InReason;
    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        if(InBattleManager.GetBattleEnd() == true) return false;
        if(TargetPokemon.IsDead() == true) return false;
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

    public string GetMessageText(int Index)
    {
        string PokemonName = TargetPokemon.GetName();
        string StatName = GetStatName(ChangedStatName[Index]);
        string Description = GetChangeDescription(ChangedStatLevel[Index]);
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
        bool ShouldChange = false;
        bool ChangedSuccessed = false;
        string Desc = GetChangeLevel()[0] > 0 ? "提高" : "降低";

        for(int Index = 0; Index < ShouldChangeList.Count; Index++)
        {
            if(ShouldChangeList[Index] == true)
            {
                ShouldChange = true;
            }
            if(ChangedSuccessedList[Index] == true)
            {
                ChangedSuccessed = true;                
            }
        }

        for(int Index = 0; Index < ShouldChangeList.Count; Index++)
        {
            if(ShouldChangeList[Index] == false)
            {
                PlayableDirector MessageDirector = Timelines.MessageAnimation;
                TimelineAnimation MessageTimeline = new TimelineAnimation(MessageDirector);

                MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", TargetPokemon.GetName() + "的" + GetStatName(ChangedStatName[Index]) + "无法" + Desc + "！");
                AddAnimation(MessageTimeline);
            }
        }

        if(ChangedSuccessed)
        {
            PlayableDirector AnimDirector = Timelines.DebuffAnimation;
            if(GetChangeLevel()[0] > 0)
            {
                AnimDirector = Timelines.BuffAnimation;
            }
            AnimDirector.gameObject.transform.position = TargetPokemon.GetPokemonModel().transform.position;
            TimelineAnimation TargetTimeline = new TimelineAnimation(AnimDirector);
            AddAnimation(TargetTimeline);
        }
        

        for(int Index = 0; Index < ShouldChangeList.Count; Index++)
        {
            if(ShouldChangeList[Index])
            {
                if(ChangedSuccessedList[Index])
                {
                    PlayableDirector MessageDirector = Timelines.MessageAnimation;
                    TimelineAnimation MessageTimeline = new TimelineAnimation(MessageDirector);
                    MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", GetMessageText(Index));
                    AddAnimation(MessageTimeline);
                }
                else
                {
                    PlayableDirector MessageDirector = Timelines.MessageAnimation;
                    TimelineAnimation MessageTimeline = new TimelineAnimation(MessageDirector);
                    MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", TargetPokemon.GetName() + "的" + GetStatName(ChangedStatName[Index]) + "不能再进一步" + Desc + "了！");
                    AddAnimation(MessageTimeline);
                }
            }
        }
    }

    public void Process(BattleManager InManager)
    {
        if(!ShouldProcess(InManager)) return;
        InManager.TranslateTimePoint(ETimePoint.BeforeStatChange, this);
        List<int> ChangedLevelList = GetChangeLevel();
        bool Successed = false;
        if(GetChangeLevel()[0] != 0)
        {
            InManager.AddAnimationEvent(this);
            for(int Index = 0; Index < ShouldChangeList.Count; Index++)
            {
                if(ShouldChangeList[Index] == true)
                {
                    if(TargetPokemon.ChangeStat(ChangedStatName[Index], ChangedLevelList[Index]))
                    {
                        Successed = true;
                        ChangedSuccessedList[Index] = true;
                    }
                }
            }

            if(Successed)
            {
                InManager.TranslateTimePoint(ETimePoint.AfterStatChange, this);
            }
        }
    }

    public EventType GetEventType()
    {
        return EventType.StatChange;
    }

    public BattlePokemon GetTargetPokemon() => TargetPokemon;
    public BattlePokemon GetSourcePokemon() => SourcePokemon;
    public List<int> GetChangeLevel()
    {
        int Factor = 1;
        if(ReverseChangeLevel)
        {
            Factor = -1;
        }
        List<int> Result = new List<int>();

        for(int Index = 0; Index < ChangedStatLevel.Count; Index++)
        {
            Result.Add(Factor * ChangedStatLevel[Index]);
        }
        return Result;
    }
    public void SetReverseLevel() { ReverseChangeLevel = true;}
    public List<string> GetChangeStatName() { return ChangedStatName; }
    public void ForbidChange(string stat) 
    { 
        for(int Index = 0; Index < ChangedStatName.Count; Index++)
        {
            if(ChangedStatName[Index] == stat)
            {
                ShouldChangeList[Index] = false;
            }
        }
    }
    public void ForbidAllChange()
    { 
        for(int Index = 0; Index < ChangedStatName.Count; Index++)
        {
            ShouldChangeList[Index] = false;
        }
    }
}
