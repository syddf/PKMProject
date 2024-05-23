using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TintedLens : BaseAbility
{
    protected override double ChangeSkillPowerWhenAttack(BattleManager InManager, BaseSkill InSkill, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, double Power)
    {
        double Result = Power;
        DamageSkill CastSkill = (DamageSkill)InSkill;
        if(CastSkill.GetTypeEffectiveFactor(InManager, SourcePokemon, TargetPokemon) < 1.0)
        {
            Result = Result * 2;
        }
        return Result;
    }
}
