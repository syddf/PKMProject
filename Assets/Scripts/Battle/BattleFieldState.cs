using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EBattleFieldTerrain
{
    None,
    Grass,
    Psychic,
    Electric,
    Misty
}

public struct BattleFiledState
{
    public EBattleFieldTerrain FieldTerrain;
    public int TerrainRemainTime;
}