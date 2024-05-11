using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HugePower : BaseAbility
{
    protected override double ChangeSkillPowerWhenAttack(BattleManager InManager, BaseSkill InSkill, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, double Power)
    {
        double Result = Power;
        if(InSkill.GetSkillClass() == ESkillClass.PhysicalMove)
        {
            Result = (int)Math.Floor(Result * 2.0);
        }
        return Result;
    }
}
