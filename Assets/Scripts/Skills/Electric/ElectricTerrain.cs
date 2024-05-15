using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricTerrain : StatusSkill
{
    public override bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, out string Reason)
    {
        Reason = "";
        return InManager.GetTerrainType() != EBattleFieldTerrain.Electric;
    }
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        TerrainChangeEvent newEvent = new TerrainChangeEvent(SourcePokemon, InManager, EBattleFieldTerrain.Electric);
        newEvent.Process(InManager);
    }
}