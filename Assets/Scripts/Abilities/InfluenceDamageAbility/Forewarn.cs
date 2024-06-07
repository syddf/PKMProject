using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Forewarn : BaseAbility
{
    protected override double ChangeSkillPowerWhenAttack(BattleManager InManager, BaseSkill InSkill, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, double Power)
    {
        double Result = Power;
        if(TargetPokemon.HasStatusChange(EStatusChange.Drowsy))
        {
            Result = (int)Math.Floor(Result * 1.2);
        }
        return Result;
    }
}
