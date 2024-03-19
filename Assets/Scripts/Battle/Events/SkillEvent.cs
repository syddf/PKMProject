using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class SkillEventMetaInfo
{
    public bool Hit;
    public int Damage;
    public float OtherAccuracy;
    public BattlePokemon ReferencePokemon;
    public bool NoEffect;
}

public class SkillEvent : Event
{
    private BattleSkill Skill;
    private BattlePokemon SourcePokemon;
    private List<BattlePokemon> TargetPokemon;
    private BattlePokemon CurrentProcessTargetPokemon;
    private List<SkillEventMetaInfo> SkillMetas;
    private bool SkillForbidden;
    private List<List<SkillEventMetaInfo>> SkillMetasHistory;

    private void ResetSkillMetas()
    {
        SkillMetas = new List<SkillEventMetaInfo>();
        foreach(var TargetPokemon in TargetPokemon)
        {
            SkillEventMetaInfo Result = new SkillEventMetaInfo();
            Result.Hit = false;
            Result.Damage = 0;
            Result.OtherAccuracy = 1.0f;
            Result.ReferencePokemon = TargetPokemon;
            Result.NoEffect = false;
            SkillMetas.Add(Result);
        }
    }
    public SkillEvent(BattleSkill InSkill, BattlePokemon InSourcePokemon, List<BattlePokemon> InTargetPokemon)
    {
        Skill = InSkill;
        SourcePokemon = InSourcePokemon;
        TargetPokemon = InTargetPokemon;
        CurrentProcessTargetPokemon = null;
        SkillForbidden = false;
        SkillMetasHistory = new List<List<SkillEventMetaInfo>>();
        ResetSkillMetas();
    }

    public void PlayAnimation()
    {

    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        return true;
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

    public int GetSkillCount()
    {
        System.Random rnd = new System.Random();
        return rnd.Next(Skill.GetSkillMinCount(), Skill.GetSkillMaxCount() + 1);
    }
    public bool JudgeAccuracy(BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        int SkillAccuracy = Skill.GetSkillAccuracy();
        int AttackAccuracyLevel = SourcePokemon.GetAccuracyrateLevel();
        int EvasionRateLevel = TargetPokemon.GetEvasionrateLevel();
        int F = AttackAccuracyLevel - EvasionRateLevel;
        if(F >= 6) F = 6;
        if(F <= -6) F = -6;
        float[] Table = new float[13]{0.33f, 0.38f, 0.43f, 0.50f, 0.60f, 0.75f, 1.00f, 1.33f, 1.67f, 2.00f, 2.33f, 2.67f, 3.00f};
        
        float fF = Table[F + 6];
        float C = GetSourceItemAccuracy(SourcePokemon) * GetTargetItemEvasion(TargetPokemon);
        float G = GetOtherAccuracy(TargetPokemon);

        float A = SkillAccuracy;
        A = A * fF * C * G;
        int iA = Mathf.RoundToInt(A);
        System.Random rnd = new System.Random();
        int Random = rnd.Next(0, 100);
        return Random < iA;
    }

    public void Process(BattleManager InManager)
    {
        ResetSkillMetas();
        InManager.TranslateTimePoint(ETimePoint.BeforeActivateSkill, this);
        if(!SkillForbidden)
        {
            EditorLog.DebugLog(SourcePokemon.GetName() + " Use Skill：" + Skill.GetSkillName());
            InManager.TranslateTimePoint(ETimePoint.BeforeGetSkillCount, this);
            int SkillCount = GetSkillCount();
            for(int SkillCountIndex = 0; SkillCountIndex < SkillCount; SkillCountIndex++)
            {
                bool ShouldActivateEffect = false;
                for(int TargetIndex = 0; TargetIndex < SkillMetas.Count; TargetIndex++)
                {
                    CurrentProcessTargetPokemon = SkillMetas[TargetIndex].ReferencePokemon;
                    InManager.TranslateTimePoint(ETimePoint.BeforeJudgeSkillIsEffective, this);
                    if(SkillMetas[TargetIndex].ReferencePokemon.IsDead())
                    {
                        SkillMetas[TargetIndex].NoEffect = true;
                    }
                    bool Effective = Skill.JudgeIsEffective(InManager, SourcePokemon, CurrentProcessTargetPokemon);
                    if(!Effective)
                    {
                        SkillMetas[TargetIndex].NoEffect = true;
                    }
                }
                for(int TargetIndex = 0; TargetIndex < SkillMetas.Count; TargetIndex++)
                {
                    if(!SkillMetas[TargetIndex].NoEffect)
                    {
                        CurrentProcessTargetPokemon = SkillMetas[TargetIndex].ReferencePokemon;
                        InManager.TranslateTimePoint(ETimePoint.BeforeJudgeAccuracy, this);
                        // Judge ..
                        bool Hit = JudgeAccuracy(SourcePokemon, CurrentProcessTargetPokemon);
                        SkillMetas[TargetIndex].Hit = Hit;
                        if(Hit)
                        {
                            ShouldActivateEffect = true;
                        }
                    }
                }
                if(ShouldActivateEffect)
                {
                    EditorLog.DebugLog(SourcePokemon.GetName()  + " Activate Skill：" + Skill.GetSkillName());
                    for(int TargetIndex = 0; TargetIndex < SkillMetas.Count; TargetIndex++)
                    {
                        CurrentProcessTargetPokemon = SkillMetas[TargetIndex].ReferencePokemon;
                        InManager.TranslateTimePoint(ETimePoint.BeforeSkillEffect, this);
                        if(!SkillMetas[TargetIndex].NoEffect)
                        {
                            if(SkillMetas[TargetIndex].Hit)
                            {
                                EditorLog.DebugLog(SourcePokemon.GetName()  + " Skill：" + Skill.GetSkillName() + "Hit: " + CurrentProcessTargetPokemon.GetName() );
                                if(Skill.IsDamageSkill())
                                {
                                    SkillMetas[TargetIndex].Damage = Skill.DamagePhase(InManager, SourcePokemon, CurrentProcessTargetPokemon);
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
                            Skill.AfterDamageEvent(InManager, SourcePokemon, CurrentProcessTargetPokemon);
                            if(Dead)
                            {
                                EditorLog.DebugLog(CurrentProcessTargetPokemon.GetName()  + " Defeated!");                            
                                PokemonDefeatedEvent defeatedEvent = new PokemonDefeatedEvent(CurrentProcessTargetPokemon, SourcePokemon, Skill);
                                defeatedEvent.Process(InManager);
                            }                        
                            InManager.TranslateTimePoint(ETimePoint.AfterTakenDamage, this);
                        }
                        Skill.AfterSkillEffectEvent(InManager, SourcePokemon, CurrentProcessTargetPokemon);
                    }
                }
                else
                {
                    break;
                }
                SkillMetasHistory.Add(this.SkillMetas);
            }
        }
    }

    public BattlePokemon GetCurrentProcessTargetPokemon()
    {
        return CurrentProcessTargetPokemon;
    }

    public EventType GetEventType()
    {
        return EventType.UseSkill;
    }

    public void ForbidSkill()
    {
        SkillForbidden = true;
    }
}
