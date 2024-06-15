using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SupremeOverlord : BaseAbility
{
    protected override double ChangeSkillPowerWhenAttack(BattleManager InManager, BaseSkill InSkill, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, double Power)
    {
        double Result = Power;
        int Count = InManager.GetTeammatesDeadCount(SourcePokemon);
        double Ratio = 1.0 + (Count * 0.1);
        Result = (int)Math.Floor(Result * Ratio);
        return Result;
    }
}
