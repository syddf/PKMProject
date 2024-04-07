using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DamageEvent : EventAnimationPlayer, Event
{
    private BattlePokemon ReferencePokemon;
    private int Damage;
    private string DamageReason;
    public DamageEvent(BattlePokemon TargetPokemon, int InDamage, string InDamageReason)
    {
        ReferencePokemon = TargetPokemon;
        Damage = InDamage;
        DamageReason = InDamageReason;
    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        if(InBattleManager.GetBattleEnd() == true) return false;
        if(ReferencePokemon.IsDead()) return false;
        return true;
    }

    public override void InitAnimation()
    {
        if(Damage == 0)
        {
            return;
        }
        TimelineAnimationManager Timelines = TimelineAnimationManager.GetGlobalTimelineAnimationManager();
        TimelineAnimation DamageAnimation = new TimelineAnimation(Timelines.DamageAnimation);
        DamageAnimation.SetSignalParameter("BattleUI", "DamageSignal", "Damage", Damage.ToString());

        if(ReferencePokemon.GetIsEnemy())
        {
            DamageAnimation.SetSignalParameter("BattleUI", "DamageSignal", "Target", "Enemy1");
        }                  
        else
        {
            DamageAnimation.SetSignalParameter("BattleUI", "DamageSignal", "Target", "Player1");
        }    
        AddAnimation(DamageAnimation);      
        PlayableDirector MessageDirector = Timelines.MessageAnimation;
        TimelineAnimation MessageTimeline = new TimelineAnimation(MessageDirector);
        MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", ReferencePokemon.GetName() + "因为" + DamageReason + "受到了伤害！");
        AddAnimation(MessageTimeline);    
    }

    public void Process(BattleManager InManager)
    {
        if(!ShouldProcess(InManager)) return;
        InManager.TranslateTimePoint(ETimePoint.BeforeTakenDamage, this);
        bool Dead = ReferencePokemon.TakenDamage(Damage);
        InManager.AddAnimationEvent(this);
        if(Dead)
        {
            PokemonDefeatedEvent defeatedEvent = new PokemonDefeatedEvent(ReferencePokemon, null, null);
            defeatedEvent.Process(InManager);
        }  
        InManager.TranslateTimePoint(ETimePoint.AfterTakenDamage, this);
    }

    public EventType GetEventType()
    {
        return EventType.Damage;
    }
}