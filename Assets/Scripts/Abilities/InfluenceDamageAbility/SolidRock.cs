using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidRock : BaseAbility
{
    protected override double ChangeSkillDamageWhenDefense(BattleManager InManager, BaseSkill InSkill, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, double Damage)
    {
        double Result = Damage;
        DamageSkill CastSkill = (DamageSkill)InSkill;
        if(CastSkill.GetTypeEffectiveFactor(InManager, SourcePokemon, TargetPokemon) > 1.0)
        {
            Result = Result * 0.75;
        }
        return Result;
    }
}
