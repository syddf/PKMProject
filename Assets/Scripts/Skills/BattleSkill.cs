using System.Collections;
using System.Collections.Generic;
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
        float Power = CastSkill.GetPower(InManager, SourcePokemon, TargetPokemon);
        float Atk = CastSkill.GetSourceAtk(InManager, SourcePokemon, TargetPokemon);
        float Def = CastSkill.GetTargetDef(InManager, SourcePokemon, TargetPokemon);
        float Level = SourcePokemon.GetLevel();
        float DamageWithOutFactor = ((2.0f * Level + 10.0f) / 250.0f) * (Atk / Def) * Power + 2;
        float Factor = 1.0f;
        if(CastSkill.IsSameType(InManager, SourcePokemon, TargetPokemon))
        {
            Factor *= CastSkill.GetSameTypePowerFactor(InManager, SourcePokemon, TargetPokemon);
        }
        int Damage = Mathf.FloorToInt(DamageWithOutFactor * Factor);
        return Mathf.Max(1, Damage);
    }

    public void ProcessStatusEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {

    }
}
