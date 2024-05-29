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


public struct BattleFieldState
{
    public EBattleFieldTerrain FieldTerrain;
    public int TerrainRemainTime;

    public EWeather WeatherType;
    public int WeatherRemainTime;

    public bool IsTrickRoomActive;
    public int TrickRoomRemainTime;
}