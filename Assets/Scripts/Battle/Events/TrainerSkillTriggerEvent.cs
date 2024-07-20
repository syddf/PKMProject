using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class TrainerSkillTriggerEvent : EventAnimationPlayer, Event
{
    private List<Event> NewEvents;
    private BaseTrainerSkill SourceSkill;
    public override void OnAnimationFinished()
    {
        RandomTimelinePlayer Player = SourceSkill.gameObject.GetComponent<RandomTimelinePlayer>();
        if(Player)
        {
            Player.RandomPlay();
        }
    }

    public TrainerSkillTriggerEvent(List<Event> InEvents, BaseTrainerSkill InSkill)
    {
        NewEvents = InEvents;
        SourceSkill = InSkill;
    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        if(InBattleManager.GetBattleEnd() == true) return false;
        return true;
    }

    public void Process(BattleManager InManager)
    {
        if(!ShouldProcess(InManager)) return;
        InManager.AddAnimationEvent(this);
        foreach(var NewEvent in NewEvents)
        {
            if(NewEvent.ShouldProcess(InManager))
            {
                NewEvent.Process(InManager);
            }
        }
    }

    public override void InitAnimation()
    {
        TimelineAnimationManager Timelines = TimelineAnimationManager.GetGlobalTimelineAnimationManager();
        TimelineAnimation TargetTimeline = new TimelineAnimation(Timelines.TrainerSkillAnimation);
        GameObject TrainerSkillObj = GameObject.Find("Canvas1/SingleBattleUI/AbilityStates/EnemyTrainerSkill");
        if(SourceSkill.GetReferenceTrainer().IsPlayer)
        {
            TrainerSkillObj = GameObject.Find("Canvas1/SingleBattleUI/AbilityStates/PlayerTrainerSkill");
        }
        TargetTimeline.SetTrackObject("StateObject", TrainerSkillObj);
        TargetTimeline.SetSignalReceiver("SignalObject", TrainerSkillObj);
        TargetTimeline.SetSignalParameter("SignalObject", "TrainerSkillTriggerSignal", "TrainerSkillName", SourceSkill.GetSkillName());
        AddAnimation(TargetTimeline);
    }

    public EventType GetEventType()
    {
        return EventType.TrainerSkillTrigger;
    }
}
