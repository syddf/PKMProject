using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PunkRock : BaseAbility
{
    protected override double ChangeSkillPowerWhenAttack(BattleManager InManager, BaseSkill InSkill, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, double Power)
    {
        double Result = Power;
        if(BattleSkillMetaInfo.IsSoundSkill(InSkill.GetSkillName()))
        {
            Result = (int)Math.Floor(Result * 1.3);
        }
        return Result;
    }

    protected override double ChangeSkillDamageWhenDefense(BattleManager InManager, BaseSkill InSkill, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, double Damage)
    {
        double Result = Damage;
        if(BattleSkillMetaInfo.IsSoundSkill(InSkill.GetSkillName()))
        {
            Result = (int)Math.Floor(Result * 0.5);
        }
        return Result;
    }
}
