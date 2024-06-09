using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThickFat : BaseAbility
{
    protected override double ChangeSkillDamageWhenDefense(BattleManager InManager, BaseSkill InSkill, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, double Damage)
    {
        double Result = Damage;
        if(InSkill.GetSkillType(SourcePokemon) == EType.Fire || InSkill.GetSkillType(SourcePokemon) == EType.Ice)
        {
            Result = (int)Math.Floor(Result * 0.5);
        }
        return Result;
    }
}
