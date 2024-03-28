using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Playables;

public class BattleSkill
{
    protected BaseSkill ReferenceBaseSkill;
    protected EMasterSkill MasterSkillType;
    protected BattlePokemon ReferencePokemon;

    protected int MinCount;
    protected int MaxCount;

    public BattleSkill(BaseSkill InReferenceBaseSkill, EMasterSkill InMasterSkillType, BattlePokemon InReferencePokemon)
    {
        ReferenceBaseSkill = InReferenceBaseSkill;
        ReferencePokemon = InReferencePokemon;
        MasterSkillType = InMasterSkillType;

        MinCount = ReferenceBaseSkill.GetMinCount();
        MaxCount = ReferenceBaseSkill.GetMaxCount();
    }

    public BattlePokemon GetReferencePokemon()
    {
        return ReferencePokemon;
    }

    public int GetSkillAccuracy()
    {
        return ReferenceBaseSkill.GetAccuracy();
    }

    public bool IsDamageSkill()
    {
        return ReferenceBaseSkill.GetSkillClass() == ESkillClass.SpecialMove || ReferenceBaseSkill.GetSkillClass() == ESkillClass.PhysicalMove;
    }

    public int GetSkillMinCount()
    {
        return MinCount;
    }
    public int GetSkillMaxCount()
    {
        return MaxCount;
    }

    public void SetSkillMaxCount(int InMaxCount)
    {
        MaxCount = InMaxCount;
    }

    public void SetSkillMinCount(int InMinCount)
    {
        MinCount = InMinCount;
    }

    public virtual int GetCTRatio()
    {
        return 0;
    }

    public bool JudgeCT(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        int CTRatio = GetCTRatio() + SourcePokemon.GetCTLevel();
        CTRatio = Math.Min(3, CTRatio);
        CTRatio = Math.Max(0, CTRatio);
        int[] CTNumber = new int[4]{24, 8, 2, 1};
        System.Random rnd = new System.Random();
        int randNumber = rnd.Next(0, CTNumber[CTRatio]);
        return randNumber == 0;
    }

    public double ApplyTerrainDamageFactor(BattleManager InManager, DamageSkill CastSkill, double SourceDamage)
    {
        if(InManager.GetTerrainType() == EBattleFieldTerrain.Grass)
        {
            if(CastSkill.GetSkillName() == "地震" || CastSkill.GetSkillName() == "震级" || CastSkill.GetSkillName() == "重踏")
            {
                EditorLog.DebugLog(CastSkill.GetSkillName() + "因青草场地伤害减半了!");
                return (int)Math.Floor(SourceDamage * 0.5);
            }
        }
        return SourceDamage;
    }

    public int DamagePhase(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, bool CT, out double EffectiveFactor)
    {
        DamageSkill CastSkill = (DamageSkill)ReferenceBaseSkill;
        double Power = CastSkill.GetPower(InManager, SourcePokemon, TargetPokemon);
        double Atk = CastSkill.GetSourceAtk(InManager, SourcePokemon, TargetPokemon, CT);
        double Def = CastSkill.GetTargetDef(InManager, SourcePokemon, TargetPokemon, CT);
        double Level = SourcePokemon.GetLevel();
        double DamageWithOutFactor = ((2.0 * Level + 10.0) / 250.0) * (Atk / Def) * Power + 2;
        double Damage = DamageWithOutFactor;

        if(CT)
        {
            double CTFactor = CastSkill.GetCTFactor(InManager, SourcePokemon, TargetPokemon);
            Damage = (int)Math.Floor(Damage * CTFactor);
        }

        System.Random rnd = new System.Random();
        double RandomFactor = (double)rnd.Next(85, 101) / 100.0;
        Damage = (int)Math.Floor(Damage * RandomFactor);

        if(CastSkill.IsSameType(InManager, SourcePokemon, TargetPokemon))
        {
            double SameTypeFactor = CastSkill.GetSameTypePowerFactor(InManager, SourcePokemon, TargetPokemon);
            Damage = (int)Math.Floor(Damage * SameTypeFactor);
        }
        double TypeEffectiveFactor = CastSkill.GetTypeEffectiveFactor(InManager, SourcePokemon, TargetPokemon);
        EffectiveFactor = TypeEffectiveFactor;
        Damage = (int)Math.Floor(Damage * TypeEffectiveFactor);

        Damage = ApplyTerrainDamageFactor(InManager, CastSkill, Damage);

        if(SourcePokemon.GetAbility())
        {
            Damage = SourcePokemon.GetAbility().ChangeSkillDamage(InManager, CastSkill, SourcePokemon, TargetPokemon, Damage);
        }
        if(TargetPokemon.GetAbility())
        {
            Damage = TargetPokemon.GetAbility().ChangeSkillDamage(InManager, CastSkill, SourcePokemon, TargetPokemon, Damage);
        }

        int IntDamage = (int)Math.Floor(Damage);
        IntDamage = Math.Min(IntDamage, TargetPokemon.GetHP());
        return Mathf.Max(1, IntDamage);
    }

    public void ProcessStatusEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        StatusSkill CastSkill = (StatusSkill)ReferenceBaseSkill;
        CastSkill.ProcessStatusSkillEffect(InManager, SourcePokemon, TargetPokemon);
    }

    public void AfterDamageEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        DamageSkill CastSkill = (DamageSkill)ReferenceBaseSkill;
        CastSkill.AfterDamageEvent(InManager, SourcePokemon, TargetPokemon);
    }
    public void AfterSkillEffectEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        ReferenceBaseSkill.AfterSkillEffectEvent(InManager, SourcePokemon, TargetPokemon);
    }
    public bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        if(!TargetPokemon)
            return true;
        return ReferenceBaseSkill.JudgeIsEffective(InManager, SourcePokemon, TargetPokemon);
    }

    public int GetAttackAccuracyChangeLevel(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        return ReferenceBaseSkill.GetAttackAccuracyChangeLevel(InManager, SourcePokemon, TargetPokemon);
    }

    public int GetTargetEvasionChangeLevel(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
         return ReferenceBaseSkill.GetTargetEvasionChangeLevel(InManager, SourcePokemon, TargetPokemon);
    }

    public int GetSkillPriority()
    {
        int Priority = ReferenceBaseSkill.GetSkillPriority(ReferencePokemon);
        if(ReferencePokemon.GetAbility())
        {
            Priority = Priority + ReferencePokemon.GetAbility().GetAbilitySkillPriority(ReferenceBaseSkill);
        }
        return Priority;
    }

    public string GetSkillName() { return ReferenceBaseSkill.GetSkillName(); }
    public ERange GetSkillRange() { return ReferenceBaseSkill.GetSkillRange();}
    public PlayableDirector GetSkillAnimation(){ return ReferenceBaseSkill.GetSkillAnimation();}
}


public class BattleSkillMetaInfo
{
    public static bool IsSoundSkill(string SkillName)
    {
        if(SkillName == "叫声") return true;
        if(SkillName == "吼叫") return true;
        if(SkillName == "唱歌") return true;
        if(SkillName == "超音波") return true;
        if(SkillName == "刺耳声") return true;
        if(SkillName == "打鼾") return true;
        if(SkillName == "终焉之歌") return true;
        if(SkillName == "治愈铃声") return true;
        if(SkillName == "黑暗恐慌") return true;
        if(SkillName == "吵闹") return true;
        if(SkillName == "巨声") return true;
        if(SkillName == "金属音") return true;
        if(SkillName == "草笛") return true;
        if(SkillName == "长嚎") return true;
        if(SkillName == "虫鸣") return true;
        if(SkillName == "喋喋不休") return true;
        if(SkillName == "轮唱") return true;
        if(SkillName == "回声") return true;
        if(SkillName == "古老之歌") return true;
        if(SkillName == "大声咆哮") return true;
        if(SkillName == "战吼") return true;
        if(SkillName == "魅惑之声") return true;
        if(SkillName == "抛下狠话") return true;
        if(SkillName == "爆音波") return true;
        if(SkillName == "密语") return true;
        if(SkillName == "泡影的咏叹调") return true;
        if(SkillName == "鳞片噪音") return true;
        if(SkillName == "炽魂热舞烈音爆") return true;
        if(SkillName == "魂舞烈音爆") return true;
        if(SkillName == "破音") return true;
        if(SkillName == "诡异咒语") return true;
        if(SkillName == "闪焰高歌") return true;
        if(SkillName == "魅诱之声") return true;
        if(SkillName == "精神噪音") return true;
        return false;
    }
}