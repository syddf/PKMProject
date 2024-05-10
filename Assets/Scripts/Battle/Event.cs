using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
public enum ETimePoint
{
    None,
    BattleStart,
    PokemonIn,
    UseSkill,
    PokemonOut,
    BeforeStatChange,
    AfterStatChange,
    BeforeActivateSkill,
    BeforeGetSkillCount,
    BeforeJudgeSkillIsEffective,
    BeforeJudgeAccuracy,
    BeforeSkillEffectAnimFake,
    BeforeSkillEffect,
    BeforeTakenDamage,
    AfterTakenDamage,
    AfterSkillEffect,
    AfterActivateSkill,
    BeforePokemonDefeated,
    AfterPokemonDefeated,
    BeforeChangeTerrain,
    AfterChangeTerrain,
    BeforeChangeWeather,
    AfterChangeWeather,
    TurnEnd,
    BeforeHeal,
    AfterHeal,
    BeforeSetPokemonStatusChange,
    AfterSetPokemonStatusChange,
    BeforeSetBattleFieldStatusChange,
    AfterSetBattleFieldStatusChange,
    BeforeMegaEvolution,
    AfterMegaEvolution
}

public enum EEventResultType
{

}

public struct EventResult
{
    public EEventResultType ResultType;
    public List<string> ResultValues;
}

public enum EventType
{
    BattleStart,
    Switch,
    MegaEvolution,
    UseSkill,
    PokemonDefeated,
    StatChange,
    AbilityTrigger,
    ItemTrigger,
    TrainerSkillTrigger,
    SwitchAfterDefeated,
    SwitchAfterSkillUse,
    BattleEnd,
    TerrainChange,
    WeatherChange,
    TurnEnd,
    Heal,
    Damage,
    SetPokemonStatusChange,
    SetBattleFieldStatusChange,
    RemovePokemonStatusChange,
    RemoveBattleFieldStatusChange,
    KnockOffItem,
    Fake
}

public interface Event
{
    public bool ShouldProcess(BattleManager InBattleManager);
    public void Process(BattleManager InManager);
    public EventType GetEventType();
}

public interface EventAnimation
{
    public bool Finished();
    public void Begin();
    public void Play();
}
public class TimelineTrackSetCache
{
    public string TrackName;
    public GameObject Obj;

    public TimelineTrackSetCache(string InTrackName, GameObject InObj)
    {
        Obj = InObj;
        TrackName = InTrackName;
    }
}

public class TimelineTrackParameterCache
{
    public string TrackName;
    public string SignalName;
    public string ParamName;
    public string Value;
    public TimelineTrackParameterCache(string InTrackName, string InSignalName, string InParamName, string InValue)
    {
        TrackName = InTrackName;
        SignalName = InSignalName;
        ParamName = InParamName;
        Value = InValue;
    }
}

public class TimelineAnimation : EventAnimation
{   
    private PlayableDirector Director;
    private bool PlayFinished;

    private List<TimelineTrackParameterCache> ParamsCache = new List<TimelineTrackParameterCache>();
    private List<TimelineTrackSetCache> SignalTrackSetCache = new List<TimelineTrackSetCache>();
    private List<TimelineTrackSetCache> ActivationTrackSetCache = new List<TimelineTrackSetCache>();
 
    public TimelineAnimation(PlayableDirector InDirector)
    {
        Director = InDirector;
        PlayFinished = false;
        Director.stopped += OnPlayableDirectorStopped;
    }
    public bool Finished()
    {
        return PlayFinished;
    }
    public void SetSignalParameter(string TrackName, string SignalName, string ParamName, string Value)
    {
        ParamsCache.Add(new TimelineTrackParameterCache(TrackName, SignalName, ParamName, Value));
    }

    public void SetSignalReceiver(string TrackName, GameObject Obj)
    {
        SignalTrackSetCache.Add(new TimelineTrackSetCache(TrackName, Obj));
    }

    public void SetSignalParameterInner(string TrackName, string SignalName, string ParamName, string Value)
    {
        TimelineAsset timeline = (TimelineAsset)Director.playableAsset;
        foreach (var track in timeline.GetOutputTracks())
        {
            if (track is TrackWithParameterizedSignal Track)
            {
                if(Track.name == TrackName)
                {
                    var markers = Track.GetMarkers();
                    foreach (var marker in markers)
                    {
                        if (marker is SignalWithParams signalWithParam)
                        {
                            if(signalWithParam.sigName == SignalName)
                            {
                                for(int ParamIndex = 0; ParamIndex < 16; ParamIndex++)
                                {
                                    if(signalWithParam.ParamsName[ParamIndex] == ParamName)
                                    {
                                        signalWithParam.ParamsValue[ParamIndex] = Value;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void SetSignalReceiverInner(string TrackName, GameObject Obj)
    {
        TimelineAsset timeline = (TimelineAsset)Director.playableAsset;
        foreach (var track in timeline.GetOutputTracks())
        {
            if (track is TrackWithParameterizedSignal Track)
            {
                if(Track.name == TrackName)
                    Director.SetGenericBinding(Track, Obj);
            }
        }
    }
    public void SetTrackObject(string TrackName, GameObject Obj)
    {
        ActivationTrackSetCache.Add(new TimelineTrackSetCache(TrackName, Obj));
    }
    public void SetTrackObjectInner(string TrackName, GameObject Obj)
    {
        TimelineAsset timeline = (TimelineAsset)Director.playableAsset;
        foreach (var track in timeline.GetOutputTracks())
        {
            if (track is ActivationTrack Track)
            {
                if(Track.name == TrackName)
                    Director.SetGenericBinding(Track, Obj);
            }
        }
    }

    public void Begin()
    {        
        foreach(var TrackIter in ActivationTrackSetCache)
        {
            SetTrackObjectInner(TrackIter.TrackName, TrackIter.Obj);
        }
        foreach(var TrackIter in SignalTrackSetCache)
        {
            SetSignalReceiverInner(TrackIter.TrackName, TrackIter.Obj);
        }
        foreach(var ParamIter in ParamsCache)
        {
            SetSignalParameterInner(ParamIter.TrackName, ParamIter.SignalName, ParamIter.ParamName, ParamIter.Value);
        }
        Director.gameObject.SetActive(true);
        Director.Play();
        PlayFinished = false;
    }

    public void Play()
    {
    }

    private void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (Director == aDirector)
        {
            PlayFinished = true;
        }
    }

    void OnDestroy()
    {
        // 不要忘记移除事件监听，以避免内存泄漏
        if (Director != null)
        {
            Director.stopped -= OnPlayableDirectorStopped;
        }
    }
}

public class EventAnimationPlayer
{
    private List<EventAnimation> Anims = new List<EventAnimation>();
    private int CurAnimationIndex = -1;

    public virtual void OnAnimationFinished()
    {

    }

    public virtual void AddAnimation(EventAnimation Anim)
    {
        Anims.Add(Anim);
    }

    public virtual bool Finished()
    {
        if(Anims.Count == 0) return true;
        if(CurAnimationIndex == Anims.Count)
        {
            Anims.Clear();
            OnAnimationFinished();
            return true;
        }
        return false;
    }

    public virtual void InitAnimation()
    {

    }

    public void BeginPlay()
    {
        CurAnimationIndex = 0;
        InitAnimation();
        if(CurAnimationIndex < Anims.Count)
        {
            Anims[CurAnimationIndex].Begin();
        }
    }

    public virtual void Play()
    {
        if(CurAnimationIndex >= Anims.Count || CurAnimationIndex == -1)
        {
            return;
        }

        Anims[CurAnimationIndex].Play();
        if(Anims[CurAnimationIndex].Finished())
        {
            CurAnimationIndex++;
            if(CurAnimationIndex < Anims.Count)
            {
                Anims[CurAnimationIndex].Begin();
            }
        }
    }
}

public class EventComparer : IComparer<Event>
{
    public int Compare(Event x, Event y)
    {
        int XType = (int)x.GetEventType();
        int YType = (int)y.GetEventType();
        if(XType > YType)
        {
            return 1;
        }
        else if(XType < YType)
        {
            return -1;
        }
        else
        {
            if(x.GetEventType() == EventType.Switch)
            {
                SwitchEvent CastX = (SwitchEvent)x;
                SwitchEvent CastY = (SwitchEvent)y;
                if(CastX.GetOutPokemon().GetSpeed(ECaclStatsMode.Normal) < CastY.GetOutPokemon().GetSpeed(ECaclStatsMode.Normal))
                {
                    return 1;
                }
                else if(CastX.GetOutPokemon().GetSpeed(ECaclStatsMode.Normal) >  CastY.GetOutPokemon().GetSpeed(ECaclStatsMode.Normal))
                {
                    return -1;
                }
                else
                {
                    System.Random rnd = new System.Random();
                    int Rand = rnd.Next(0, 2);
                    if(Rand == 0) return 1;
                    if(Rand == 1) return -1;
                }
            }
            else if(x.GetEventType() == EventType.UseSkill)
            {
                SkillEvent CastX = (SkillEvent)x;
                SkillEvent CastY = (SkillEvent)y;
                BattleSkill SkillX = CastX.GetSkill();
                BattleSkill SkillY = CastY.GetSkill();
                BattlePokemon TargetX = CastX.GetReferenceManager().GetTargetPokemon(CastX.GetTargetPokemon()[0]);
                BattlePokemon TargetY = CastY.GetReferenceManager().GetTargetPokemon(CastY.GetTargetPokemon()[0]);
                if(SkillX.GetSkillPriority(CastX.GetReferenceManager(), TargetX) < SkillY.GetSkillPriority(CastX.GetReferenceManager(), TargetY))
                {
                    return 1;
                }
                else if(SkillX.GetSkillPriority(CastX.GetReferenceManager(), TargetX) > SkillY.GetSkillPriority(CastX.GetReferenceManager(), TargetY))
                {
                    return -1;
                }
                else
                {
                    if(CastX.GetSourcePokemon().GetSpeed(ECaclStatsMode.Normal) < CastY.GetSourcePokemon().GetSpeed(ECaclStatsMode.Normal))
                    {
                        return 1;
                    }
                    else if(CastX.GetSourcePokemon().GetSpeed(ECaclStatsMode.Normal) >  CastY.GetSourcePokemon().GetSpeed(ECaclStatsMode.Normal))
                    {
                        return -1;
                    }
                    else
                    {
                        System.Random rnd = new System.Random();
                        int Rand = rnd.Next(0, 2);
                        if(Rand == 0) return 1;
                        if(Rand == 1) return -1;
                    }
                }
            }
        }
        return 0;
    }
}
