using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassyGlide : DamageSkill
{
    public override int GetSkillPriority(BattleManager InManager,BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        if(InManager.GetTerrainType() == EBattleFieldTerrain.Grass)
        {
            return 1;
        }
        return 0;
    }
}
