using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ToughClaws : BaseAbility
{
    protected override double ChangeSkillPowerWhenAttack(BattleManager InManager, BaseSkill InSkill, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, double Power)
    {
        double Result = Power;
        if(BattleSkillMetaInfo.IsTouchingSkill(InSkill.GetSkillName()))
        {
            Result = (int)Math.Floor(Result * 1.3);
        }
        return Result;
    }
}
