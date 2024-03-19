using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

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

    public int DamagePhase(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        DamageSkill CastSkill = (DamageSkill)ReferenceBaseSkill;
        double Power = CastSkill.GetPower(InManager, SourcePokemon, TargetPokemon);
        double Atk = CastSkill.GetSourceAtk(InManager, SourcePokemon, TargetPokemon);
        double Def = CastSkill.GetTargetDef(InManager, SourcePokemon, TargetPokemon);
        double Level = SourcePokemon.GetLevel();
        double DamageWithOutFactor = ((2.0 * Level + 10.0) / 250.0) * (Atk / Def) * Power + 2;
        double Factor = 1.0;
        if(CastSkill.IsSameType(InManager, SourcePokemon, TargetPokemon))
        {
            Factor *= CastSkill.GetSameTypePowerFactor(InManager, SourcePokemon, TargetPokemon);
        }
        double TypeEffectiveFactor = CastSkill.GetTypeEffectiveFactor(InManager, SourcePokemon, TargetPokemon);
        Factor *= TypeEffectiveFactor;

        System.Random rnd = new System.Random();
        double RandomFactor = (double)rnd.Next(85, 101) / 100.0;
        Factor *= RandomFactor;

        int Damage = (int)Math.Floor(DamageWithOutFactor * Factor);
        return Mathf.Max(1, Damage);
    }

    public void ProcessStatusEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {

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
    public virtual bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        return true;
    }

    public string GetSkillName() { return ReferenceBaseSkill.GetSkillName(); }
}
