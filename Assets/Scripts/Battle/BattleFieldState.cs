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


public enum EWeather
{
    None,
    SunLight,
    Rain,
    Snow,
    Sand
}


public struct BattleFiledState
{
    public EBattleFieldTerrain FieldTerrain;
    public int TerrainRemainTime;

    public EWeather WeatherType;
    public int WeatherRemainTime;
}