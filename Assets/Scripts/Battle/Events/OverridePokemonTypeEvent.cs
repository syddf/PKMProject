using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
public class OverridePokemonTypeEvent : EventAnimationPlayer, Event
{
    private BattlePokemon ReferencePokemon;
    private BattleManager ReferenceManager;
    private EType ReferenceOverrideType;
    public OverridePokemonTypeEvent(BattleManager InManager, BattlePokemon InReferencePokemon, EType InOverrideType)
    {
        ReferencePokemon = InReferencePokemon;
        ReferenceManager = InManager;
        ReferenceOverrideType = InOverrideType;
    }
    public BattlePokemon GetReferencePokemon()
    {
        return ReferencePokemon;
    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        if(InBattleManager.GetBattleEnd() == true) return false;
        if(ReferencePokemon.IsDead() == true) return false;
        return true;
    }
    public override void OnAnimationFinished()
    {
        if(ReferencePokemon.GetIsEnemy() == false)
        {
            ReferenceManager.UpdatePlayerType();
        }
        else
        {
            ReferenceManager.UpdateEnemyType();
        }
    }
    public override void InitAnimation()
    {
        TimelineAnimationManager Timelines = TimelineAnimationManager.GetGlobalTimelineAnimationManager();

        PlayableDirector MessageDirector = Timelines.MessageAnimation;
        TimelineAnimation MessageTimeline = new TimelineAnimation(MessageDirector);
        MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", ReferencePokemon.GetName() + "变换了属性！");
        AddAnimation(MessageTimeline);  
    }

    public void Process(BattleManager InManager)
    {
        if(!ShouldProcess(InManager)) return;
        InManager.AddAnimationEvent(this);
        ReferencePokemon.SetOverrideType(ReferenceOverrideType);
    }

    public EventType GetEventType()
    {
        return EventType.OverrideType;
    }
}
