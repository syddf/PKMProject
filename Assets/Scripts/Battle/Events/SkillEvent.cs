using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class SkillEventMetaInfo
{
    public bool Hit;
    public int Damage;
    public float OtherAccuracy;
    public BattlePokemon ReferencePokemon;
    public bool NoEffect;
    public string NoEffectReason;
    public double EffectiveFactor;
    public bool CT;
}

//Just Used For MessageAnimation
public class UseSkillMessageEvent : EventAnimationPlayer, Event
{
    private BattleSkill Skill;
    private BattlePokemon SourcePokemon;
    private bool SkillForbidden;
    private string ForbiddenReason;
    public UseSkillMessageEvent(BattleSkill InSkill, BattlePokemon InSourcePokemon, bool InSkillForbidden, string InForbiddenReason)
    {
        Skill = InSkill;
        SourcePokemon = InSourcePokemon;
        SkillForbidden = InSkillForbidden;
        ForbiddenReason = InForbiddenReason;
    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        if(InBattleManager.GetBattleEnd() == true) return false;
        return true;
    }

    public override void InitAnimation()
    {
        TimelineAnimationManager Timelines = TimelineAnimationManager.GetGlobalTimelineAnimationManager();
        PlayableDirector MessageDirector = Timelines.MessageAnimation;
        if(SkillForbidden)
        {
            TimelineAnimation MessageTimeline = new TimelineAnimation(MessageDirector);
            string MessageText = SourcePokemon.GetName() + "因" + ForbiddenReason + "无法使用" + Skill.GetSkillName() + "！";
            if(Skill.GetSkillName() == "混乱")
            {
                MessageText = SourcePokemon.GetName() + "因" + ForbiddenReason + "无法使用招式！";
            }
            MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", MessageText);
            AddAnimation(MessageTimeline);
        }
        else
        {
            TimelineAnimation MessageTimeline = new TimelineAnimation(MessageDirector);
            string MessageText = SourcePokemon.GetName() + "使用了" + Skill.GetSkillName() + "！";
            if(Skill.GetSkillName() == "预知未来攻击")
            {
                MessageText = SourcePokemon.GetName() + "的未来攻击触发了！";
            }
            if(Skill.GetSkillName() == "混乱")
            {
                MessageText = SourcePokemon.GetName() + "因为混乱攻击了自己！";
            }
            MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", MessageText);
            AddAnimation(MessageTimeline);
        }
    }

    public void Process(BattleManager InManager)
    {
        if(!ShouldProcess(InManager)) return;
        InManager.AddAnimationEvent(this);
    }

    public EventType GetEventType()
    {
        return EventType.UseSkill;
    }
}

public class SkillEvent : EventAnimationPlayer, Event
{
    private BattleSkill Skill;
    private BattlePokemon SourcePokemon;
    private List<ETarget> TargetPokemon;
    private BattlePokemon CurrentProcessTargetPokemon;
    private List<SkillEventMetaInfo> SkillMetas;
    private bool SkillForbidden;
    private string SkillForbiddenReason;
    private List<List<SkillEventMetaInfo>> SkillMetasHistory;
    private int SkillAnimIndex = 0;
    private BattleManager ReferenceBattleManager;
    private int CurrentMetaIndex = 0;
    private int CurrentPokemonInMetaIndex = 0;
    private void ResetSkillMetas()
    {
        SkillMetas = new List<SkillEventMetaInfo>();
        foreach(var TargetPokemon in TargetPokemon)
        {
            SkillEventMetaInfo Result = new SkillEventMetaInfo();
            Result.Hit = false;
            Result.Damage = 0;
            Result.OtherAccuracy = 1.0f;
            Result.ReferencePokemon = ReferenceBattleManager.GetTargetPokemon(TargetPokemon);
            Result.NoEffect = false;
            SkillMetas.Add(Result);
        }
    }
    public SkillEvent(BattleManager InManager, BattleSkill InSkill, BattlePokemon InSourcePokemon, List<ETarget> InTargetPokemon)
    {
        Skill = InSkill;
        SourcePokemon = InSourcePokemon;
        TargetPokemon = InTargetPokemon;
        ReferenceBattleManager = InManager;
        CurrentProcessTargetPokemon = null;
        SkillForbidden = false;
        SkillMetasHistory = new List<List<SkillEventMetaInfo>>();
        ResetSkillMetas();
    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        if(InBattleManager.GetBattleEnd() == true) return false;
        return !SourcePokemon.IsDead();
    }

    private float GetSourceItemAccuracy(BattlePokemon InSourcePokemon)
    {
        return 1.0f;
    }

    private float GetTargetItemEvasion(BattlePokemon InSourcePokemon)
    {
        return 1.0f;
    }

    private float GetOtherAccuracy(BattlePokemon InTargetPokemon)
    {
        foreach(var SkillInfoIter in SkillMetas)
        {
            if(SkillInfoIter.ReferencePokemon == InTargetPokemon)
            {
                return SkillInfoIter.OtherAccuracy;
            }
        }
        return 1.0f;
    }

    public override void InitAnimation()
    {
        TimelineAnimationManager Timelines = TimelineAnimationManager.GetGlobalTimelineAnimationManager();
        PlayableDirector MessageDirector = Timelines.MessageAnimation;
        //if(Skill.IsDamageSkill())
        {
            if(SkillForbidden)
            {
                
            }
            else
            {
                int HitIndex = SkillAnimIndex;
                {
                    var SkillMetas = SkillMetasHistory[HitIndex];
                    if(SkillMetas.Count == 1)
                    {
                        var SkillMeta = SkillMetas[0];
                        if(SkillMeta.NoEffect)
                        {
                            if(SkillAnimIndex == 0 && SkillMeta.ReferencePokemon)
                            {
                                TimelineAnimation NoEffectMessage = new TimelineAnimation(MessageDirector);
                                if(SkillMeta.ReferencePokemon.IsDead())
                                {
                                    NoEffectMessage.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", "但是" + SkillMeta.ReferencePokemon.GetName() + "已经不在场上了！");
                                }
                                else if(SkillMeta.NoEffectReason != null && SkillMeta.NoEffectReason != "")
                                {
                                    NoEffectMessage.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", SkillMeta.NoEffectReason);
                                }
                                else
                                {
                                    NoEffectMessage.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", "这对" + SkillMeta.ReferencePokemon.GetName() + "似乎没有效果！");
                                }
                                AddAnimation(NoEffectMessage);
                            }
                            else
                            {
                                TimelineAnimation NoEffectMessage = new TimelineAnimation(MessageDirector);
                                if(SkillMeta.NoEffectReason != null && SkillMeta.NoEffectReason != "")
                                {
                                    NoEffectMessage.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", SkillMeta.NoEffectReason);
                                }
                                else
                                {
                                    NoEffectMessage.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", "但是失败了！");
                                }
                                AddAnimation(NoEffectMessage);
                            }
                        }
                        else
                        {
                            GameObject SkillRootObject = Skill.GetSkillAnimation().gameObject;
                            SkillAnimationRoot RootScript = SkillRootObject.GetComponent<SkillAnimationRoot>();
                            if(SkillMetas[0].ReferencePokemon && RootScript.TargetPokemonTransform != null)
                            {
                                RootScript.TargetPokemonTransform.position = SkillMetas[0].ReferencePokemon.GetPokemonModel().GetComponent<PokemonReceiver>().BodyTransform.transform.position;
                            }
                            if(RootScript.SourcePokemonTransform != null)
                            {
                                RootScript.SourcePokemonTransform.position = SourcePokemon.GetPokemonModel().GetComponent<PokemonReceiver>().BodyTransform.transform.position;
                            }
                            if(RootScript.SourcePokemonTouchTransform != null)
                            {
                                RootScript.SourcePokemonTouchTransform.position = SourcePokemon.GetPokemonModel().GetComponent<PokemonReceiver>().TouchHitTransform.transform.position;
                            }
                            if(SkillMetas[0].ReferencePokemon && RootScript.TargetPokemonTouchTransform != null)
                            {
                                RootScript.TargetPokemonTouchTransform.position = SkillMetas[0].ReferencePokemon.GetPokemonModel().GetComponent<PokemonReceiver>().TouchHitTransform.transform.position;
                            }
                            if(SkillMetas[0].ReferencePokemon && RootScript.TargetPokemonFloorTransform != null)
                            {
                                Vector3 position = SkillMetas[0].ReferencePokemon.GetPokemonModel().transform.position;
                                position.y = 0;
                                RootScript.TargetPokemonFloorTransform.position = position;
                            }
                            if(RootScript.SourcePokemonFloorTransform != null)
                            {
                                Vector3 position = SourcePokemon.GetPokemonModel().transform.position;
                                position.y = 0;
                                RootScript.SourcePokemonFloorTransform.position = position;
                            }
                            AutoScaler[] Scalers = SkillRootObject.GetComponentsInChildren<AutoScaler>();
                            foreach(var Scaler in Scalers)
                            {
                                Scaler.targetCharacter = SourcePokemon.GetPokemonModel().transform;
                            }

                            if(SkillMeta.Hit)
                            {
                                TimelineAnimation SkillAnimation = new TimelineAnimation(Skill.GetSkillAnimation());
                                SkillAnimation.SetSignalParameter("BattleUI", "DamageSignal", "Damage", SkillMeta.Damage.ToString());
                                GameObject Cameras = GameObject.Find("Cameras");
                                GameObject EnemyCamera = Cameras.GetComponent<SubObjects>().SubObject1;
                                GameObject PlayerCamera = Cameras.GetComponent<SubObjects>().SubObject2;
                                if(SourcePokemon.GetIsEnemy())
                                {
                                    SkillAnimation.SetTrackObject("SourceCamera", EnemyCamera);
                                }
                                else
                                {
                                    SkillAnimation.SetTrackObject("SourceCamera", PlayerCamera);
                                }
                                if(SkillMetas[0].ReferencePokemon)
                                {
                                    if(SkillMetas[0].ReferencePokemon.GetIsEnemy())
                                    {
                                        SkillAnimation.SetTrackObject("TargetCamera", EnemyCamera);
                                    }
                                    else
                                    {
                                        SkillAnimation.SetTrackObject("TargetCamera", PlayerCamera);
                                    }
                                }
                                if(Skill.IsDamageSkill())
                                {
                                    if(SkillMeta.ReferencePokemon.GetIsEnemy())
                                    {
                                        SkillAnimation.SetSignalParameter("BattleUI", "DamageSignal", "Target", "Enemy1");
                                    }                  
                                    else
                                    {
                                        SkillAnimation.SetSignalParameter("BattleUI", "DamageSignal", "Target", "Player1");
                                    }      
                                }
                                SkillAnimation.SetSignalReceiver("SourcePokemon", SourcePokemon.GetPokemonModel());
                                if(SkillMetas[0].ReferencePokemon)
                                    SkillAnimation.SetSignalReceiver("TargetPokemon", SkillMetas[0].ReferencePokemon.GetPokemonModel());
                                AddAnimation(SkillAnimation);
                                if(Skill.IsDamageSkill())
                                {
                                    if(SkillMeta.CT)
                                    {
                                        TimelineAnimation CTMessage = new TimelineAnimation(MessageDirector);
                                        CTMessage.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", "击中了要害！");
                                        AddAnimation(CTMessage);
                                    }
                                    if(SkillMeta.EffectiveFactor == 0.5 || SkillMeta.EffectiveFactor == 0.25)
                                    {
                                        SkillAnimation.SetSignalParameter("BattleUI", "DamageSignal", "Effective", "Not");
                                        TimelineAnimation EffectiveMessage = new TimelineAnimation(MessageDirector);
                                        EffectiveMessage.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", "这不是很有效！");
                                        AddAnimation(EffectiveMessage);
                                    }
                                    else if(SkillMeta.EffectiveFactor == 1.0)
                                    {
                                        SkillAnimation.SetSignalParameter("BattleUI", "DamageSignal", "Effective", "Normal");
                                    }
                                    else
                                    {
                                        SkillAnimation.SetSignalParameter("BattleUI", "DamageSignal", "Effective", "Super");
                                        TimelineAnimation EffectiveMessage = new TimelineAnimation(MessageDirector);
                                        EffectiveMessage.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", "这非常有效！");
                                        AddAnimation(EffectiveMessage);                            
                                    }
                                }
                            }
                            else
                            {
                                if(HitIndex == 0)
                                {
                                    TimelineAnimation MissMessage = new TimelineAnimation(MessageDirector);
                                    MissMessage.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", "没有命中目标！");
                                    AddAnimation(MissMessage); 
                                }
                                else
                                {
                                    TimelineAnimation MissMessage = new TimelineAnimation(MessageDirector);
                                    MissMessage.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", "命中了" + HitIndex.ToString() + "次！");
                                    AddAnimation(MissMessage); 
                                }
                            }

                        }
                    }
                }
            }
        }
        SkillAnimIndex++;
    }

    public int GetSkillCount()
    {
        System.Random rnd = new System.Random();
        return rnd.Next(Skill.GetSkillMinCount(), Skill.GetSkillMaxCount() + 1);
    }
    public bool JudgeAccuracy(BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        if(Skill.GetReferenceSkill().GetAlwaysHit(GetReferenceManager(), SourcePokemon, TargetPokemon))
        {
            return true;
        }
        if(TargetPokemon.GetGlaiveRushState())
        {
            return true;
        }
        if(GetReferenceManager().HasSpecialRule("特殊规则(帕琦拉)"))
        {
            return true;    
        }
        if(SourcePokemon.HasAbility("无防守", null, SourcePokemon, TargetPokemon) == true ||
        TargetPokemon.HasAbility("无防守", null, SourcePokemon, TargetPokemon) == true)
        {
            return true;
        }
        int SkillAccuracy = Skill.GetSkillAccuracy(GetReferenceManager(), SourcePokemon, TargetPokemon);
        int AttackAccuracyLevel = Skill.GetAttackAccuracyChangeLevel(ReferenceBattleManager, SourcePokemon, TargetPokemon);
        int EvasionRateLevel = Skill.GetTargetEvasionChangeLevel(ReferenceBattleManager, SourcePokemon, TargetPokemon);
        int F = AttackAccuracyLevel - EvasionRateLevel;
        if(F >= 6) F = 6;
        if(F <= -6) F = -6;
        float[] Table = new float[13]{0.33f, 0.38f, 0.43f, 0.50f, 0.60f, 0.75f, 1.00f, 1.33f, 1.67f, 2.00f, 2.33f, 2.67f, 3.00f};
        
        float fF = Table[F + 6];
        float C = GetSourceItemAccuracy(SourcePokemon) * GetTargetItemEvasion(TargetPokemon);
        float G = GetOtherAccuracy(TargetPokemon);
        if(TargetPokemon.HasAbility("雪隐", GetReferenceManager(), SourcePokemon, TargetPokemon) && GetReferenceManager().GetWeatherType() == EWeather.Snow)
        {
            G = G * 0.8f;
        }
        float A = SkillAccuracy;
        A = A * fF * C * G;
        int iA = Mathf.RoundToInt(A);
        System.Random rnd = new System.Random();
        int Random = rnd.Next(0, 100);
        return Random < iA;
    }

    public void Process(BattleManager InManager)
    {
        if(!ShouldProcess(InManager)) return;
        InManager.TranslateTimePoint(ETimePoint.JudgeConfusion, this);
        ResetSkillMetas();
        InManager.TranslateTimePoint(ETimePoint.BeforeActivateSkill, this);
        UseSkillMessageEvent MessageEvent = new UseSkillMessageEvent(Skill, SourcePokemon, SkillForbidden, SkillForbiddenReason);
        MessageEvent.Process(InManager);
        bool HasPressureEffect = false;
        if(!SkillForbidden)
        {
            EditorLog.DebugLog(SourcePokemon.GetName() + " Use Skill：" + Skill.GetSkillName());
            SourcePokemon.SetFirstSkill(Skill.GetReferenceSkill());
            InManager.TranslateTimePoint(ETimePoint.BeforeGetSkillCount, this);
            int SkillCount = GetSkillCount();
            bool SkillHasEffect = false;
            for(int SkillCountIndex = 0; SkillCountIndex < SkillCount; SkillCountIndex++)
            {
                bool ShouldActivateEffect = false;
                CurrentMetaIndex = SkillCountIndex;
                SkillMetasHistory.Add(this.SkillMetas);
                for(int TargetIndex = 0; TargetIndex < SkillMetas.Count; TargetIndex++)
                {
                    CurrentProcessTargetPokemon = SkillMetas[TargetIndex].ReferencePokemon;
                    CurrentPokemonInMetaIndex = TargetIndex;
                    InManager.TranslateTimePoint(ETimePoint.BeforeJudgeSkillIsEffective, this);
                    if(SkillMetas[TargetIndex].NoEffect == false)
                    {
                        if(SkillMetas[TargetIndex].ReferencePokemon && SkillMetas[TargetIndex].ReferencePokemon.IsDead())
                        {
                            SkillMetas[TargetIndex].NoEffect = true;
                        }
                        string NoEffectReason = "";
                        bool Effective = Skill.JudgeIsEffective(InManager, SourcePokemon, CurrentProcessTargetPokemon, ref NoEffectReason);
                        if(NoEffectReason != "")
                        {
                            SkillMetas[TargetIndex].NoEffectReason = NoEffectReason;
                        }
                        if(!Effective)
                        {
                            SkillMetas[TargetIndex].NoEffect = true;
                        }
                    }
                }

                for(int TargetIndex = 0; TargetIndex < SkillMetas.Count; TargetIndex++)
                {
                    if(!SkillMetas[TargetIndex].NoEffect)
                    {
                        CurrentProcessTargetPokemon = SkillMetas[TargetIndex].ReferencePokemon;
                        InManager.TranslateTimePoint(ETimePoint.BeforeJudgeAccuracy, this);
                        // Judge ..
                        bool Hit = Skill.GetSkillRange() == ERange.None || JudgeAccuracy(SourcePokemon, CurrentProcessTargetPokemon);
                        SkillMetas[TargetIndex].Hit = Hit;
                        if(Hit)
                        {
                            ShouldActivateEffect = true;
                        }
                    }
                }
                if(ShouldActivateEffect)
                {
                    InManager.TranslateTimePoint(ETimePoint.BeforeSkillEffectAnimFake, this);
                }
                InManager.AddAnimationEvent(this);
                if(ShouldActivateEffect)
                {
                    EditorLog.DebugLog(SourcePokemon.GetName()  + " Activate Skill：" + Skill.GetSkillName());
                    for(int TargetIndex = 0; TargetIndex < SkillMetas.Count; TargetIndex++)
                    {
                        CurrentProcessTargetPokemon = SkillMetas[TargetIndex].ReferencePokemon;
                        if(CurrentProcessTargetPokemon != null && CurrentProcessTargetPokemon.HasAbility("压迫感", ReferenceBattleManager, SourcePokemon, CurrentProcessTargetPokemon))
                        {
                            HasPressureEffect = true;
                        }
                        InManager.TranslateTimePoint(ETimePoint.BeforeSkillEffect, this);
                        if(!SkillMetas[TargetIndex].NoEffect)
                        {
                            if(SkillMetas[TargetIndex].Hit)
                            {
                                SkillHasEffect = true;
                                if(CurrentProcessTargetPokemon)
                                    EditorLog.DebugLog(SourcePokemon.GetName()  + " Skill：" + Skill.GetSkillName() + "Hit: " + CurrentProcessTargetPokemon.GetName() );
                                if(Skill.IsDamageSkill())
                                {
                                    SkillMetas[TargetIndex].CT = Skill.JudgeCT(InManager, SourcePokemon, CurrentProcessTargetPokemon);
                                    SkillMetas[TargetIndex].Damage = Skill.DamagePhase(InManager, SourcePokemon, CurrentProcessTargetPokemon, SkillMetas[TargetIndex].CT, out SkillMetas[TargetIndex].EffectiveFactor);
                                }
                                else
                                {
                                    Skill.ProcessStatusEffect(InManager, SourcePokemon, CurrentProcessTargetPokemon);
                                }
                            }
                        }
                    }
                    if(Skill.IsDamageSkill())
                    {
                        for(int TargetIndex = 0; TargetIndex < SkillMetas.Count; TargetIndex++)
                        {
                            CurrentProcessTargetPokemon = SkillMetas[TargetIndex].ReferencePokemon;
                            InManager.TranslateTimePoint(ETimePoint.BeforeTakenDamage, this);
                            EditorLog.DebugLog(CurrentProcessTargetPokemon.GetName()  + " Taken Damage:" + SkillMetas[TargetIndex].Damage);
                            bool Dead = CurrentProcessTargetPokemon.TakenDamage(SkillMetas[TargetIndex].Damage);
                            CurrentProcessTargetPokemon.SetDamagedBySkill();
                            Skill.AfterDamageEvent(InManager, SourcePokemon, CurrentProcessTargetPokemon, SkillMetas[TargetIndex].Damage);
                            if(Dead)
                            {
                                EditorLog.DebugLog(CurrentProcessTargetPokemon.GetName()  + " Defeated!");                            
                                PokemonDefeatedEvent defeatedEvent = new PokemonDefeatedEvent(CurrentProcessTargetPokemon, SourcePokemon, Skill);
                                defeatedEvent.Process(InManager);
                            }                        
                            InManager.TranslateTimePoint(ETimePoint.AfterTakenDamage, this);
                        }
                        int Probablity = Skill.GetAfterSkillEffectEventProbablity(InManager, SourcePokemon, CurrentProcessTargetPokemon);
                        if(Skill.GetReferenceSkill().IsAfterSkillEffectToTargetPokemon() == true && 
                        CurrentProcessTargetPokemon.HasAbility("鳞粉", InManager, SourcePokemon, CurrentProcessTargetPokemon))
                        {
                            Probablity = 0;
                        }
                        if(SourcePokemon.HasAbility("天恩", InManager, SourcePokemon, CurrentProcessTargetPokemon))
                        {
                            Probablity = Probablity * 2;
                        }
                        System.Random rnd = new System.Random();
                        int Random = rnd.Next(0, 100);
                        if(Random < Probablity)
                        {
                            Skill.AfterSkillEffectEvent(InManager, SourcePokemon, CurrentProcessTargetPokemon);
                        }
                    }
                    InManager.TranslateTimePoint(ETimePoint.AfterSkillEffect, this);
                }
                else
                {
                    break;
                }
            }
            if(SkillHasEffect)
            {
                if(Skill.GetSkillName() != "巨剑突击")
                {
                    SourcePokemon.RemoveGlaiveRushState();
                }
                InManager.TranslateTimePoint(ETimePoint.AfterActivateSkill, this);
            }
            if(Skill.GetSkillName() == "大爆炸")
            {
                DamageEvent damageEvent = new DamageEvent(SourcePokemon, SourcePokemon.GetMaxHP(), "大爆炸");
                damageEvent.Process(InManager);   
            }
            
            SourcePokemon.ReducePP(Skill, HasPressureEffect);
        }
        SourcePokemon.SetActivated();
    }

    public int GetCurrentDamage()
    {
        return SkillMetasHistory[CurrentMetaIndex][CurrentPokemonInMetaIndex].Damage;
    }

    public void SetCurrentDamage(int Damage)
    {
        SkillMetasHistory[CurrentMetaIndex][CurrentPokemonInMetaIndex].Damage = Damage;
    }

    public BattlePokemon GetCurrentProcessTargetPokemon()
    {
        return CurrentProcessTargetPokemon;
    }

    public EventType GetEventType()
    {
        return EventType.UseSkill;
    }

    public void ForbidSkill(string Reason)
    {
        SkillForbidden = true;
        SkillForbiddenReason = Reason;
    }

    public BattleSkill GetSkill()
    {
        return Skill;
    }

    public void MakeCurrentTargetNoEffect(string Reason)
    {
        if(SkillMetasHistory[CurrentMetaIndex][CurrentPokemonInMetaIndex].NoEffect == false)
        {
            SkillMetasHistory[CurrentMetaIndex][CurrentPokemonInMetaIndex].NoEffect = true;
            SkillMetasHistory[CurrentMetaIndex][CurrentPokemonInMetaIndex].NoEffectReason = Reason;
        }
    }

    public BattlePokemon GetSourcePokemon()
    {
        return SourcePokemon;
    }
    
    public BattleManager GetReferenceManager()
    {
        return ReferenceBattleManager;
    }

    public void ReplaceConfusionSkill()
    {
        Skill = new BattleSkill(ReferenceBattleManager.GetConfusionSkill(), EMasterSkill.None, SourcePokemon);
        TargetPokemon[0] = SourcePokemon.GetIsEnemy() ? ETarget.E0 : ETarget.P0; 
    }

    public bool IsSkillForbidden()
    {
        return SkillForbidden;
    }

    public bool IsSkillEffective()
    {
        if(SkillMetasHistory.Count == 1)
        {
            if(SkillMetasHistory[0].Count == 1)
            {
                return !SkillMetasHistory[0][0].NoEffect;
            }
        }
        return true;
    }
    public List<ETarget> GetTargetPokemon()
    {
        return TargetPokemon;
    }
}
