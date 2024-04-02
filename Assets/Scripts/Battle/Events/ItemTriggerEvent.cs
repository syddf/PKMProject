using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class ItemTriggerEvent : EventAnimationPlayer, Event
{
    private List<Event> NewEvents;
    private BattleItem SourceItem;
    public ItemTriggerEvent(List<Event> InEvents, BattleItem InItem)
    {
        NewEvents = InEvents;
        SourceItem = InItem;
    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        if(InBattleManager.GetBattleEnd() == true) return false;
        if(SourceItem.GetReferencePokemon().IsDead()) return false;
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
        TimelineAnimation TargetTimeline = new TimelineAnimation(Timelines.ItemStateAnimation);
        GameObject ItemObj = GameObject.Find("Canvas/SingleBattleUI/AbilityStates/EnemyItem");
        if(!SourceItem.GetReferencePokemon().GetIsEnemy())
        {
            ItemObj = GameObject.Find("Canvas/SingleBattleUI/AbilityStates/PlayerItem");
        }
        TargetTimeline.SetTrackObject("StateObject", ItemObj);
        TargetTimeline.SetSignalReceiver("SignalObject", ItemObj);
        TargetTimeline.SetSignalParameter("SignalObject", "ItemTriggerSignal", "ItemName", SourceItem.GetItemName());
        AddAnimation(TargetTimeline);
    }

    public EventType GetEventType()
    {
        return EventType.ItemTrigger;
    }
}

public class KnockOffItemEvent : EventAnimationPlayer, Event
{
    private BattlePokemon TargetPokemon;
    public KnockOffItemEvent(BattlePokemon InPokemon)
    {
        TargetPokemon = InPokemon;
    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        if(InBattleManager.GetBattleEnd() == true) return false;
        if(TargetPokemon.IsDead()) return false;
        return true;
    }

    public void Process(BattleManager InManager)
    {
        if(!ShouldProcess(InManager)) return;
        TargetPokemon.GetBattleItem().KnockOffItem();
        InManager.AddAnimationEvent(this);
    }

    public override void InitAnimation()
    {
        TimelineAnimationManager Timelines = TimelineAnimationManager.GetGlobalTimelineAnimationManager();
        PlayableDirector MessageDirector = Timelines.MessageAnimation;
        TimelineAnimation MessageTimeline = new TimelineAnimation(MessageDirector);
        MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", TargetPokemon.GetName() + "的" + TargetPokemon.GetBattleItem().GetItemName() + "被拍落了!");
        AddAnimation(MessageTimeline);
    }

    public EventType GetEventType()
    {
        return EventType.KnockOffItem;
    }
}