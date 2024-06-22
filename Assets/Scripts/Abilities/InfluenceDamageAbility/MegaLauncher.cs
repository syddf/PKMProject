using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MegaLauncher : BaseAbility
{
    protected override double ChangeSkillPowerWhenAttack(BattleManager InManager, BaseSkill InSkill, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, double Power)
    {
        double Result = Power;
        if(InSkill.GetSkillName() == "水之波动" ||
           InSkill.GetSkillName() == "波导弹" ||
           InSkill.GetSkillName() == "恶之波动" ||
           InSkill.GetSkillName() == "龙之波动" ||
           InSkill.GetSkillName() == "大地波动"
        )
        {
            Result = (int)Math.Floor(Result * 1.5);
        }
        return Result;
    }

}
