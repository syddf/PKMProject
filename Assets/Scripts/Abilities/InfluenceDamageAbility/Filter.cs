using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Filter : BaseAbility
{
    protected override double ChangeSkillDamageWhenDefense(BattleManager InManager, BaseSkill InSkill, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, double Damage)
    {
        DamageSkill CastSkill = (DamageSkill)InSkill;
        double TypeEffectiveFactor = CastSkill.GetTypeEffectiveFactor(InManager, SourcePokemon, TargetPokemon);
        double Result = Damage;
        if(TypeEffectiveFactor > 1.0)
        {
            Result = (int)Math.Floor(Result * 0.75);
        }
        return Result;
    }
}
