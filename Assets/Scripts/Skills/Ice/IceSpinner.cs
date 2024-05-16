using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpinner : DamageSkill
{
    public override void AfterSkillEffectEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        if(InManager.GetTerrainType() != EBattleFieldTerrain.None)
        {
            TerrainChangeEvent resetTerrain = new TerrainChangeEvent(null, InManager, EBattleFieldTerrain.None);
            resetTerrain.Process(InManager);
        }
    }
}
