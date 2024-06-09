using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TerrainChangeEvent : EventAnimationPlayer, Event
{
    private EBattleFieldTerrain OriginTerrainType;
    private EBattleFieldTerrain NewTerrainType;
    private BattlePokemon SourcePokemon;
    private BattleManager ReferenceBattleManager;
    private bool bSuccessed = false;

    public EBattleFieldTerrain GetNewTerrain()
    {
        return NewTerrainType;
    }
    public string GetTerrainTypeName()
    {
        if(NewTerrainType == EBattleFieldTerrain.Grass) return "青草场地";
        if(NewTerrainType == EBattleFieldTerrain.Misty) return "薄雾场地";
        if(NewTerrainType == EBattleFieldTerrain.Electric) return "电气场地";
        if(NewTerrainType == EBattleFieldTerrain.Psychic) return "超能场地";
        return "";
    }

    public GameObject GetTerrainParticlesObject(EBattleFieldTerrain TerrainType)
    {
        TimelineAnimationManager Timelines = TimelineAnimationManager.GetGlobalTimelineAnimationManager();
        PlayableDirector MessageDirector = Timelines.SwitchTerrainAnimation;
        if(TerrainType == EBattleFieldTerrain.Grass)
        {
            return MessageDirector.gameObject.GetComponent<SubObjects>().SubObject1;
        }
        if(TerrainType == EBattleFieldTerrain.Psychic)
        {
            return MessageDirector.gameObject.GetComponent<SubObjects>().SubObject2;
        }
        if(TerrainType == EBattleFieldTerrain.Electric)
        {
            return MessageDirector.gameObject.GetComponent<SubObjects>().SubObject3;
        }
        if(TerrainType == EBattleFieldTerrain.Misty)
        {
            return MessageDirector.gameObject.GetComponent<SubObjects>().SubObject4;
        }
        return null;
    }

    public TerrainChangeEvent(BattlePokemon InSourcePokemon, BattleManager InBattleManager, EBattleFieldTerrain InNewTerrainType)
    {
        ReferenceBattleManager = InBattleManager;
        SourcePokemon = InSourcePokemon;   
        NewTerrainType = InNewTerrainType;
    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        if(InBattleManager.GetBattleEnd() == true) return false;
        return true;
    }

    public override void InitAnimation()
    {
        TimelineAnimationManager Timelines = TimelineAnimationManager.GetGlobalTimelineAnimationManager();
        if(bSuccessed)
        {
            if(NewTerrainType == EBattleFieldTerrain.None)
            {
                PlayableDirector ResetTerrainDirector = Timelines.ResetTerrainAnimation;
                TimelineAnimation ResetTerrainTimeline = new TimelineAnimation(ResetTerrainDirector);
                ResetTerrainTimeline.SetTrackObject("ResetTerrain", GetTerrainParticlesObject(OriginTerrainType));
                AddAnimation(ResetTerrainTimeline);

                PlayableDirector MessageDirector = Timelines.MessageAnimation;
                TimelineAnimation MessageTimeline = new TimelineAnimation(MessageDirector);
                MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", "场地复原了！");
                AddAnimation(MessageTimeline);      
            }
            else
            {
                PlayableDirector SwitchTerrainDirector = Timelines.SwitchTerrainAnimation;
                TimelineAnimation SwitchTerrainTimeline = new TimelineAnimation(SwitchTerrainDirector);
                SwitchTerrainTimeline.SetTrackObject("OriginTerrain", GetTerrainParticlesObject(OriginTerrainType));
                SwitchTerrainTimeline.SetTrackObject("NewTerrain", GetTerrainParticlesObject(NewTerrainType));
                AddAnimation(SwitchTerrainTimeline);

                PlayableDirector MessageDirector = Timelines.MessageAnimation;
                TimelineAnimation MessageTimeline = new TimelineAnimation(MessageDirector);
                MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", "场地变成了" + GetTerrainTypeName() + "！");
                AddAnimation(MessageTimeline);   
            }
        }
        else
        {
            PlayableDirector MessageDirector = Timelines.MessageAnimation;
            TimelineAnimation MessageTimeline = new TimelineAnimation(MessageDirector);
            MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", "切换" + GetTerrainTypeName() + "失败了！");
            AddAnimation(MessageTimeline);            
        }
    }

    public void Process(BattleManager InManager)
    {
        OriginTerrainType = InManager.GetTerrainType();
        if(!ShouldProcess(InManager)) return;
        InManager.TranslateTimePoint(ETimePoint.BeforeChangeTerrain, this);
        if(InManager.GetTerrainType() == NewTerrainType)
        {
            bSuccessed = false;
        }
        else
        {
            InManager.SetTerrain(NewTerrainType, 5);
            bSuccessed = true;
        }
        InManager.AddAnimationEvent(this);
        InManager.TranslateTimePoint(ETimePoint.AfterChangeTerrain, this);
    }

    public EventType GetEventType()
    {
        return EventType.TerrainChange;
    }

}
