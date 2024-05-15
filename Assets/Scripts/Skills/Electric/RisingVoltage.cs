using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingVoltage : DamageSkill
{
    protected override int GetSkillPower(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        if(InManager.GetTerrainType() == EBattleFieldTerrain.Electric && TargetPokemon.IsGroundPokemon())
        {
            return Power * 2;
        }
        return Power;
    }
}
