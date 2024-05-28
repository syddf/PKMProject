using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brine : DamageSkill
{
    protected override int GetSkillPower(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        if(TargetPokemon.GetHP() <= (TargetPokemon.GetMaxHP() / 2))
        {
            return Power * 2;
        }
        return Power;
    }
}
