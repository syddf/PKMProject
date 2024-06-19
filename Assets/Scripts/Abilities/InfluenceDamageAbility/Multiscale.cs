using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiscale : BaseAbility
{
    protected override double ChangeSkillDamageWhenDefense(BattleManager InManager, BaseSkill InSkill, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, double Damage)
    {
        double Result = Damage;
        DamageSkill CastSkill = (DamageSkill)InSkill;
        if(SourcePokemon.GetHP() == SourcePokemon.GetMaxHP())
        {
            Result = Result * 0.5;
        }
        return Result;
    }
}
