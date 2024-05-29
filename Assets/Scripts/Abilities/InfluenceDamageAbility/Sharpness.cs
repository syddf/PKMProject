using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
    
public class Sharpness : BaseAbility
{
    bool IsClawSkill(string InSkillName)
    {
        return 
        InSkillName == "劈开" ||
        InSkillName == "连斩" ||
        InSkillName == "空气利刃" ||
        InSkillName == "燕返" ||
        InSkillName == "叶刃" ||
        InSkillName == "暗袭要害" ||
        InSkillName == "空气之刃" ||
        InSkillName == "精神利刃" ||
        InSkillName == "圣剑" ||
        InSkillName == "贝壳刃" ||
        InSkillName == "日光刃" ||
        InSkillName == "岩斧" ||
        InSkillName == "秘剑・千重涛" ||
        InSkillName == "水波刀";
    }
    protected override double ChangeSkillPowerWhenAttack(BattleManager InManager, BaseSkill InSkill, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, double Power)
    {
        double Result = Power;
        if(IsClawSkill(InSkill.GetSkillName()))
        {
            Result = (int)Math.Floor(Result * 1.5);
        }
        return Result;
    }
}
