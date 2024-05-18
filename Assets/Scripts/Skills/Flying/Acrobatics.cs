using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Acrobatics : DamageSkill
{
    protected override int GetSkillPower(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        BattleItem Item = SourcePokemon.GetBattleItem();
        double Result = Power;
        if(!Item.HasItem())
        {
            Result = Result * 2.0;
        }
        return (int)Math.Floor(Result);
    }
}