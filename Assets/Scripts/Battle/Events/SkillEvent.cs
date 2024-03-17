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
    public SkillEvent(BattleSkill InSkill, BattlePokemon InSourcePokemon, List<BattlePokemon> InTargetPokemon)
    {
        Skill = InSkill;
        SourcePokemon = InSourcePokemon;
        TargetPokemon = InTargetPokemon;
        CurrentProcessTargetPokemon = null;
        SkillForbidden = false;
        foreach(var TargetPokemon in InTargetPokemon)
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
        InManager.TranslateTimePoint(ETimePoint.BeforeActivateSkill, this);
        if(!SkillForbidden)
        {
            InManager.TranslateTimePoint(ETimePoint.BeforeGetSkillCount, this);
            int SkillCount = GetSkillCount();
            for(int SkillCountIndex = 0; SkillCountIndex < SkillCount; SkillCountIndex++)
            {
                bool AllMissed = true;
                for(int TargetIndex = 0; TargetIndex < SkillMetas.Count; TargetIndex++)
                {
                    CurrentProcessTargetPokemon = SkillMetas[TargetIndex].ReferencePokemon;
                    InManager.TranslateTimePoint(ETimePoint.BeforeJudgeAccuracy, this);
                    // Judge ..
                    bool Hit = JudgeAccuracy(SourcePokemon, CurrentProcessTargetPokemon);
                    SkillMetas[TargetIndex].Hit = Hit;
                    if(Hit)
                    {
                        AllMissed = false;
                    }
                }
                if(!AllMissed)
                {
                    for(int TargetIndex = 0; TargetIndex < SkillMetas.Count; TargetIndex++)
                    {
                        CurrentProcessTargetPokemon = SkillMetas[TargetIndex].ReferencePokemon;
                        InManager.TranslateTimePoint(ETimePoint.BeforeSkillEffect, this);
                        if(!SkillMetas[TargetIndex].NoEffect)
                        {
                            if(SkillMetas[TargetIndex].Hit)
                            {
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
                            EditorLog.DebugLog(CurrentProcessTargetPokemon.name + " Taken Damage:" + SkillMetas[TargetIndex].Damage);
                            if(!CurrentProcessTargetPokemon.TakenDamage(SkillMetas[TargetIndex].Damage))
                            {
                                InManager.TranslateTimePoint(ETimePoint.BeforePokemonDefeated, this);
                                EditorLog.DebugLog(CurrentProcessTargetPokemon.name + " Defeated.");
                                InManager.AddDefeatedPokemon(CurrentProcessTargetPokemon);
                                InManager.TranslateTimePoint(ETimePoint.AfterPokemonDefeated, this);
                            }                        
                            InManager.TranslateTimePoint(ETimePoint.AfterTakenDamage, this);
                        }
                    }
                }
                else
                {
                    break;
                }
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
