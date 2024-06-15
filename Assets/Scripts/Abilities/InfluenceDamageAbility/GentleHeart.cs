using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GentleHeart : BaseAbility
{
    protected override double ChangeSkillDamageWhenAttack(BattleManager InManager, BaseSkill InSkill, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, double Damage)
    {
        double Result = Damage;
        Result = (int)Math.Floor(Result * 0.8);
        return Result;
    }
    protected override double ChangeSkillDamageWhenDefense(BattleManager InManager, BaseSkill InSkill, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, double Damage)
    {
        double Result = Damage;
        Result = (int)Math.Floor(Result * 0.8);
        return Result;
    }
}
