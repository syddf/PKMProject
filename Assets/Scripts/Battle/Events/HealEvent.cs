using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class HealEvent : EventAnimationPlayer, Event
{
    private BattlePokemon ReferencePokemon;
    private int HealHP;
    private int RealHealHP;
    private bool CanHeal;
    private string HealReason;
    public HealEvent(BattlePokemon TargetPokemon, int InHealHP, string InHealReason)
    {
        ReferencePokemon = TargetPokemon;
        HealHP = InHealHP;
        RealHealHP = 0;
        CanHeal = true;
        HealReason = InHealReason;
    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        if(InBattleManager.GetBattleEnd() == true) return false;
        if(ReferencePokemon.IsDead()) return false;
        return true;
    }

    public override void InitAnimation()
    {
        if(RealHealHP == 0)
        {
            return;
        }
        TimelineAnimationManager Timelines = TimelineAnimationManager.GetGlobalTimelineAnimationManager();
        if(CanHeal)
        {
            PlayableDirector AnimDirector = Timelines.HealAnimation;
            AnimDirector.gameObject.transform.position = ReferencePokemon.GetPokemonModel().transform.position;
            TimelineAnimation TargetTimeline = new TimelineAnimation(AnimDirector);
            TargetTimeline.SetSignalParameter("BattleUI", "HealSignal", "Heal", RealHealHP.ToString());

            if(ReferencePokemon.GetIsEnemy())
            {
                TargetTimeline.SetSignalParameter("BattleUI", "HealSignal", "Target", "Enemy1");
            }                  
            else
            {
                TargetTimeline.SetSignalParameter("BattleUI", "HealSignal", "Target", "Player1");
            }  
            AddAnimation(TargetTimeline);

            PlayableDirector MessageDirector = Timelines.MessageAnimation;
            TimelineAnimation MessageTimeline = new TimelineAnimation(MessageDirector);
            MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", ReferencePokemon.GetName() + "因为" + HealReason + "回复了生命值!");
            AddAnimation(MessageTimeline);        
        }
        else
        {
            PlayableDirector MessageDirector = Timelines.MessageAnimation;
            TimelineAnimation MessageTimeline = new TimelineAnimation(MessageDirector);
            MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", ReferencePokemon.GetName() + "无法回复生命值!");
            AddAnimation(MessageTimeline);
        }
    }

    public void Process(BattleManager InManager)
    {
        if(!ShouldProcess(InManager)) return;
        InManager.TranslateTimePoint(ETimePoint.BeforeHeal, this);
        RealHealHP = ReferencePokemon.HealHP(HealHP);
        InManager.AddAnimationEvent(this);
        InManager.TranslateTimePoint(ETimePoint.AfterHeal, this);
    }

    public EventType GetEventType()
    {
        return EventType.Heal;
    }
}