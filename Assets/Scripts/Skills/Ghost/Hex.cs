using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Hex : DamageSkill
{
    protected override int GetSkillPower(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        double Result = Power;
        if(TargetPokemon.HasStatusChange(EStatusChange.Paralysis) ||
           TargetPokemon.HasStatusChange(EStatusChange.Poison) ||
           TargetPokemon.HasStatusChange(EStatusChange.Drowsy) ||
           TargetPokemon.HasStatusChange(EStatusChange.Frostbite) ||
           TargetPokemon.HasStatusChange(EStatusChange.Burn))
        {
            Result = Result * 2.0;
        }
        return (int)Math.Floor(Result);
    }
}