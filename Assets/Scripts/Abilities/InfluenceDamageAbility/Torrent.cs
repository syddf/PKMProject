using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Torrent : BaseAbility
{
    protected override double ChangeSkillPowerWhenAttack(BattleManager InManager, BaseSkill InSkill, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, double Power)
    {
        double Result = Power;
        if(InSkill.GetSkillType(SourcePokemon) == EType.Water && SourcePokemon.GetHP() <= (SourcePokemon.GetMaxHP() / 3))
        {
            Result = (int)Math.Floor(Result * 1.5);
        }
        return Result;
    }
}
