using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MistyTerrain : StatusSkill
{
    public override bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, out string Reason)
    {
        Reason = "";
        return InManager.GetTerrainType() != EBattleFieldTerrain.Misty;
    }
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        TerrainChangeEvent newEvent = new TerrainChangeEvent(SourcePokemon, InManager, EBattleFieldTerrain.Misty);
        newEvent.Process(InManager);
    }
}