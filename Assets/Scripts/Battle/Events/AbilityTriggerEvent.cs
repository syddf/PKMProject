using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTriggerEvent : EventAnimationPlayer, Event
{
    private List<Event> NewEvents;
    private BaseAbility SourceAbility;
    public AbilityTriggerEvent(List<Event> InEvents, BaseAbility InAbility)
    {
        NewEvents = InEvents;
        SourceAbility = InAbility;
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
        TimelineAnimation TargetTimeline = new TimelineAnimation(Timelines.AbilityStateAnimation);
        GameObject AbilityObj = GameObject.Find("Canvas/SingleBattleUI/AbilityStates/EnemyAbility");
        if(!SourceAbility.GetReferencePokemon().GetIsEnemy())
        {
            AbilityObj = GameObject.Find("Canvas/SingleBattleUI/AbilityStates/PlayerAbility");
        }
        TargetTimeline.SetTrackObject("StateObject", AbilityObj);
        TargetTimeline.SetSignalReceiver("SignalObject", AbilityObj);
        TargetTimeline.SetSignalParameter("SignalObject", "AbilityTriggerSignal", "AbilityName", SourceAbility.GetAbilityName());
        TargetTimeline.SetSignalParameter("SignalObject", "AbilityTriggerSignal", "PokemonIndex", SourceAbility.GetReferencePokemon().GetIndexInPKDex().ToString());
        AddAnimation(TargetTimeline);
    }

    public EventType GetEventType()
    {
        return EventType.AbilityTrigger;
    }
}