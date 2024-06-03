using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TrickRoomChangeEvent : EventAnimationPlayer, Event
{
    private bool IsBegin;
    private int Turn;
    private BattlePokemon SourcePokemon;
    private BattleManager ReferenceBattleManager;
    private bool bSuccessed = true;
    public TrickRoomChangeEvent(BattlePokemon InSourcePokemon, BattleManager InBattleManager, bool InIsBegin, int InTurn)
    {
        ReferenceBattleManager = InBattleManager;
        SourcePokemon = InSourcePokemon;   
        IsBegin = InIsBegin;
        Turn = InTurn;

        if(InBattleManager.HasSpecialRule("特殊规则(玛绣)"))
        {
            Turn = 999;
        }
    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        if(InBattleManager.GetBattleEnd() == true) return false;
        return true;
    }

    public override void OnAnimationFinished()
    {

    }


    public override void InitAnimation()
    {
        TimelineAnimationManager Timelines = TimelineAnimationManager.GetGlobalTimelineAnimationManager();
        if(bSuccessed)
        {
            GameObject FloorObj = GameObject.Find("Floor");
            MeshRenderer meshRenderer = FloorObj.GetComponent<MeshRenderer>();
            Material SelfMaterial = meshRenderer.material;
            if(IsBegin)
            {
                SelfMaterial.SetFloat("_UseNoise", 1.0f);
                AudioManager.GetGlobalAudioManager().PlayTrickRoomAudio();
            }
            else
            {
                SelfMaterial.SetFloat("_UseNoise", 0.0f);
            }
            if(IsBegin)
            {
                PlayableDirector MessageDirector = Timelines.MessageAnimation;
                TimelineAnimation MessageTimeline = new TimelineAnimation(MessageDirector);
                MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", "空间被扭曲了！");
                AddAnimation(MessageTimeline);      
            }
            else
            {
                PlayableDirector MessageDirector = Timelines.MessageAnimation;
                TimelineAnimation MessageTimeline = new TimelineAnimation(MessageDirector);
                MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", "扭曲的空间复原了！");
                AddAnimation(MessageTimeline);   
            }
        }
        else
        {
            PlayableDirector MessageDirector = Timelines.MessageAnimation;
            TimelineAnimation MessageTimeline = new TimelineAnimation(MessageDirector);
            MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", "切换戏法空间失败了！");
            AddAnimation(MessageTimeline);            
        }
    }

    public void Process(BattleManager InManager)
    {
        if(!ShouldProcess(InManager)) return;
        InManager.TranslateTimePoint(ETimePoint.BeforeChangeTrickRoom, this);
        if(IsBegin)
        {
            InManager.BeginTrickRoom(Turn);
        }
        else
        {
            InManager.EndTrickRoom();
        }
        InManager.AddAnimationEvent(this);
        InManager.TranslateTimePoint(ETimePoint.AfterChangeTrickRoom, this);
    }

    public EventType GetEventType()
    {
        return EventType.TrickRoomChange;
    }

}
