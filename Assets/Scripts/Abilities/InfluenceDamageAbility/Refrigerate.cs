using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Refrigerate : BaseAbility
{
    protected override double ChangeSkillPowerWhenAttack(BattleManager InManager, BaseSkill InSkill, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, double Power)
    {
        double Result = Power;
        if(InSkill.GetSkillType(SourcePokemon) == EType.Ice && InSkill.GetOriginSkillType() == EType.Normal)
        {
            Result = (int)Math.Floor(Result * 1.2);
        }
        return Result;
    }
}
