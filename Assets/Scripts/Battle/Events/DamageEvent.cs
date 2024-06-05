using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DamageEvent : EventAnimationPlayer, Event
{
    private BattlePokemon ReferencePokemon;
    private int Damage;
    private string DamageReason;
    private double EffectiveFactor;
    public DamageEvent(BattlePokemon TargetPokemon, int InDamage, string InDamageReason, double InEffectiveFactor = 1.0)
    {
        ReferencePokemon = TargetPokemon;
        Damage = InDamage;
        if(Damage > TargetPokemon.GetHP())
        {
            Damage = TargetPokemon.GetHP();
        }
        DamageReason = InDamageReason;
        EffectiveFactor = InEffectiveFactor;
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
        if(DamageReason == "沙暴天气")
        {
            PlayableDirector AnimDirector = Timelines.WeatherDamageAnimation;
            TimelineAnimation WeatherDamageMessage = new TimelineAnimation(AnimDirector);
            WeatherDamageMessage.SetSignalReceiver("TargetPokemon", ReferencePokemon.GetPokemonModel());
            AddAnimation(WeatherDamageMessage);
        }
        if(DamageReason == "未来攻击")
        {
            PlayableDirector AnimDirector = Timelines.FutureAttackAnimation;
            TimelineAnimation FutureAttackMessage = new TimelineAnimation(AnimDirector);
            FutureAttackMessage.SetSignalReceiver("TargetPokemon", ReferencePokemon.GetPokemonModel());
            AddAnimation(FutureAttackMessage);
        
        
            FutureAttackMessage.SetSignalParameter("BattleUI", "DamageSignal", "Damage", Damage.ToString());

            if(ReferencePokemon.GetIsEnemy())
            {
                FutureAttackMessage.SetSignalParameter("BattleUI", "DamageSignal", "Target", "Enemy1");
            }                  
            else
            {
                FutureAttackMessage.SetSignalParameter("BattleUI", "DamageSignal", "Target", "Player1");
            }
            if(EffectiveFactor == 0.5 || EffectiveFactor == 0.25)
            {
                PlayableDirector LocalMessageDirector = Timelines.MessageAnimation;
                FutureAttackMessage.SetSignalParameter("BattleUI", "DamageSignal", "Effective", "Not");
                TimelineAnimation EffectiveMessage = new TimelineAnimation(LocalMessageDirector);
                EffectiveMessage.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", "这不是很有效！");
                AddAnimation(EffectiveMessage);
            }
            else if(EffectiveFactor == 1.0)
            {
                FutureAttackMessage.SetSignalParameter("BattleUI", "DamageSignal", "Effective", "Normal");
            }
            else
            {         
                PlayableDirector LocalMessageDirector = Timelines.MessageAnimation;
                FutureAttackMessage.SetSignalParameter("BattleUI", "DamageSignal", "Effective", "Super");
                TimelineAnimation EffectiveMessage = new TimelineAnimation(LocalMessageDirector);
                EffectiveMessage.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", "这非常有效！");
                AddAnimation(EffectiveMessage);                            
            }

            Vector3 position = ReferencePokemon.GetPokemonModel().transform.position;
            GameObject SkillRootObject = AnimDirector.gameObject;
            SkillAnimationRoot RootScript = SkillRootObject.GetComponent<SkillAnimationRoot>();
            if(ReferencePokemon && RootScript.TargetPokemonTransform != null)
            {
                RootScript.TargetPokemonTransform.position = ReferencePokemon.GetPokemonModel().GetComponent<PokemonReceiver>().BodyTransform.transform.position;
            }
        }

        if(DamageReason != "未来攻击")
        {
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
        }
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