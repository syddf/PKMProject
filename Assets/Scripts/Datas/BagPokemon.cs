using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PokemonNature
{
    Hardy,
    Lonely,
    Adamant,
    Naughty,
    Brave,
    Bold,
    Docile,
    Impish,
    Lax,
    Relaxed,
    Modest,
    Mild,
    Bashful,
    Rash,
    Quiet,
    Calm,
    Gentle,
    Careful,
    Quirky,
    Sassy,
    Timid,
    Hasty,
    Jolly,
    Naive,
    Serious
}

public class BagPokemon : MonoBehaviour
{
    private static double[,] NatureFactor = new double[,] 
    {
        //ATK DEF SATK SDEF SPEED
        {1, 1, 1, 1, 1},
        {1.1, 0.9, 1, 1, 1},
        {1.1, 1, 0.9, 1, 1},
        {1.1, 1, 1, 0.9, 1},
        {1.1, 1, 1, 1, 0.9},
        {0.9, 1.1, 1, 1, 1},
        {1, 1, 1, 1, 1},
        {1, 1.1, 0.9, 1, 1},
        {1, 1.1, 1, 0.9, 1},
        {1, 1.1, 1, 1, 0.9},
        {0.9, 1, 1.1, 1, 1},
        {1, 0.9, 1.1, 1, 1},
        {1, 1, 1, 1, 1},
        {1, 1, 1.1, 0.9, 1},
        {1, 1, 1.1, 1, 0.9},
        {0.9, 1, 1, 1.1, 1},
        {1, 0.9, 1, 1.1, 1},
        {1, 1, 0.9, 1.1, 1},
        {1, 1, 1, 1, 1},
        {1, 1, 1, 1.1, 0.9},
        {0.9, 1, 1, 1, 1.1},
        {1, 0.9, 1, 1, 1.1},
        {1, 1, 0.9, 1, 1.1},
        {1, 1, 1, 0.9, 1.1},
        {1, 1, 1, 1, 1},
    };
    private PokemonData SourcePokemonData;
    [SerializeField]
    private PokemonDataCollection PkmDataCollections;
    [SerializeField]
    private string Name;
    [SerializeField]
    private int Level;
    [SerializeField]
    private PokemonNature Nature;
    [SerializeField]
    private int[] IVs = new int[6]{ 0, 0, 0, 0, 0, 0};
    [SerializeField]
    private int[] BasePoints = new int[6]{ 0, 0, 0, 0, 0, 0};

    private int CaclStatus(int SpeciesStrength, int Index)
    {
        // HP
        if(Index == 0)
        {
            int A = SpeciesStrength * 2 + IVs[0] + BasePoints[0] / 4;
            float fA = (float)A * (float)Level / 100.0f + 10 + Level;
            return (int)fA;
        }
        else
        {
            int A = SpeciesStrength * 2 + IVs[Index] + BasePoints[Index] / 4;
            float fA = ((float)A * (float)Level / 100.0f + 5.0f) * (float)NatureFactor[(int)Nature, Index - 1]; 
            return (int)fA;
        }
    }

    public int GetAtk()
    {
        return CaclStatus(SourcePokemonData.Atk, 1);
    }

    public int GetSAtk()
    {
        return CaclStatus(SourcePokemonData.SAtk, 3);
    }

    public int GetDef()
    {
        return CaclStatus(SourcePokemonData.Def, 2);
    }

    public int GetSDef()
    {
        return CaclStatus(SourcePokemonData.Spd, 4);
    }

    public int GetSpeed()
    {
        return CaclStatus(SourcePokemonData.Spe, 5);
    }

    public int GetMaxHP()
    {
        return CaclStatus(SourcePokemonData.HP, 0);
    }

    public void Start()
    {
        SourcePokemonData = PkmDataCollections.GetPokemonData(Name);

        Debug.Log("Atk" + GetAtk());
        Debug.Log("Def" + GetDef());
        Debug.Log("SAtk" + GetSAtk());
        Debug.Log("SDef" + GetSDef());
        Debug.Log("Speed" + GetSpeed());
        Debug.Log("MaxHP" + GetMaxHP());
    }
}
