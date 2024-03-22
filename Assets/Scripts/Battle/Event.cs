using System.Collections;
using System.Collections.Generic;
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
    BeforeSkillEffect,
    BeforeTakenDamage,
    AfterTakenDamage,
    AfterSkillEffect,
    BeforePokemonDefeated,
    AfterPokemonDefeated,
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
    UseSkill,
    PokemonDefeated,
    StatChange,
    AbilityTrigger
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

public class TimelineAnimation : EventAnimation
{   
    private PlayableDirector Director;
    private bool PlayFinished;
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

    public void SetTrackObject(string TrackName, GameObject Obj)
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