using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Technician : BaseAbility
{
    protected override double ChangeSkillPowerWhenAttack(BattleManager InManager, BaseSkill InSkill, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, double Power)
    {
        double Result = Power;
        int OriginPower = InSkill.GetPower();
        if(OriginPower <= 60)
        {
            Result = (int)Math.Floor(Result * 1.5);
        }
        return Result;
    }
}
