using System;
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

public enum PokemonGender
{
    None,
    Male,
    Female
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
    private int HP;
    [SerializeField]
    private PokemonNature Nature;
    [SerializeField]
    private int[] IVs = new int[6]{ 0, 0, 0, 0, 0, 0};
    [SerializeField]
    private int[] BasePoints = new int[6]{ 0, 0, 0, 0, 0, 0};
    [SerializeField]
    PokemonGender Gender;
    [SerializeField]
    private BaseItem Item;
    [SerializeField]
    private BaseAbility Ability;
    [SerializeField]
    private BaseSkill[] ReferenceSkill = new BaseSkill[4];
    [SerializeField]
    private string PokemonName;
    [SerializeField]
    private bool MultiSpecies;
    [SerializeField]
    private int SpecieIndex;
    [SerializeField]
    private bool CanMega;
    
    public PokemonGender GetGender()
    {
        return Gender;
    }
    private int CaclStatus(int SpeciesStrength, int Index)
    {
        // HP
        if(Index == 0)
        {
            double A = SpeciesStrength * 2 + IVs[0] + (double)BasePoints[0] / 4;
            double fA = A * (double)Level / 100 + 10 + Level;
            return (int)Math.Floor(fA);
        }
        else
        {
            double A = SpeciesStrength * 2 + IVs[Index] + (double)BasePoints[Index] / 4;
            double Top = (double)Math.Floor(((double)A * (double)Level / 100.0f + 5.0f));
            double fA = Top * NatureFactor[(int)Nature, Index - 1]; 
            return (int)Math.Floor(fA);
        }
    }

    public string GetName()
    {
        return Name;
    }
    public int GetLevel()
    {
        return Level;
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
    public int GetHP()
    {
        return HP;
    }

    public EType GetType0()
    {
        if(SourcePokemonData.Type0 == "undefined")
        {
            return EType.None;
        }
        return (EType)Enum.Parse(typeof(EType), SourcePokemonData.Type0);
    }

    public EType GetType1()
    {
        if(SourcePokemonData.Type1 == "undefined")
        {
            return EType.None;
        }
        return (EType)Enum.Parse(typeof(EType), SourcePokemonData.Type1);
    }

    public BaseItem GetItem()
    {  
        return Item;
    }

    public void SetItem(BaseItem InItem)
    {
        Item = InItem;
    }

    public int GetIndexInPKDex()
    {
        return SourcePokemonData.Number;
    }

    public void SetBaseSkill(int Index, BaseSkill InSkill)
    {
        ReferenceSkill[Index] = InSkill;
    }
    public BaseSkill GetBaseSkill(int Index)
    {
        return ReferenceSkill[Index];
    }

    public string GetPokemonName()
    {
        return PokemonName;
    }

    public BaseAbility GetAbility()
    {
        return Ability;
    }

    public void SetAbility(BaseAbility InAbility)
    {
        Ability = InAbility;
    }
    public void Awake() 
    {
        SourcePokemonData = PkmDataCollections.GetPokemonData(Name);
        HP = GetMaxHP();
        /*
        Debug.Log(Name + "Atk" + GetAtk());
        Debug.Log(Name + "Def" + GetDef());
        Debug.Log(Name + "SAtk" + GetSAtk());
        Debug.Log(Name + "SDef" + GetSDef());
        Debug.Log(Name + "Speed" + GetSpeed());
        Debug.Log(Name + "MaxHP" + GetMaxHP());
        */
    }

    public bool GetCanMega()
    {
        return CanMega;
    }
}
