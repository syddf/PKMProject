using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SandPower : BaseAbility
{
    protected override double ChangeSkillPowerWhenAttack(BattleManager InManager, BaseSkill InSkill, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, double Power)
    {
        double Result = Power;
        if(InManager.GetWeatherType() == EWeather.Sand)
        {
            if(InSkill.GetSkillType(SourcePokemon) == EType.Rock || 
            InSkill.GetSkillType(SourcePokemon) == EType.Ground ||
            InSkill.GetSkillType(SourcePokemon) == EType.Steel
            )
            {
                Result = (int)Math.Floor(Result * 1.3);
            }
        }
        return Result;
    }

}
