using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BattlePokemonStat
{
    public int HP;
    public int MaxHP;
    public int Atk;
    public int Def;
    public int SAtk;
    public int SDef;
    public int Speed;
    public int AtkChangeLevel;
    public int DefChangeLevel;
    public int SAtkChangeLevel;
    public int SDefChangeLevel;
    public int SpeedChangeLevel;
    public int AccuracyrateLevel;
    public int EvasionrateLevel;
    public int CTLevel;
    public int Level;
    public bool Dead;
}

public class BattlePokemon : MonoBehaviour
{
    [SerializeField]
    private string Name;
    [SerializeField]
    private BaseAbility Ability;
    [SerializeField]
    private bool IsEnemy;
    [SerializeField]
    private EType Type1;
    [SerializeField]
    private EType Type2;
    [SerializeField]
    private BagPokemon ReferenceBasePokemon;
    private BattlePokemonStat PokemonStats;
    private static double[] StatLevelFactor = new double[13]{0.25, 0.29, 0.33, 0.40, 0.50, 0.67, 1.00, 1.50, 2.00, 2.50, 3.00, 3.50, 4.00};
    public BaseAbility GetAbility() => Ability;
    public bool GetIsEnemy() => IsEnemy;
    public string GetName() => Name;

    public int GetHP() => PokemonStats.HP;
    public int GetMaxHP() => PokemonStats.MaxHP;
    public int GetAtk() 
    { 
        return (int)Math.Floor((double)PokemonStats.Atk * StatLevelFactor[PokemonStats.AtkChangeLevel + 6]);
    }
    public int GetDef()
    {
        return (int)Math.Floor((double)PokemonStats.Def * StatLevelFactor[PokemonStats.DefChangeLevel + 6]);
    }
    public int GetSAtk()
    {
        return (int)Math.Floor((double)PokemonStats.SAtk * StatLevelFactor[PokemonStats.SAtkChangeLevel + 6]);
    }
    public int GetSDef()
    {
        return (int)Math.Floor((double)PokemonStats.SDef * StatLevelFactor[PokemonStats.SDefChangeLevel + 6]);
    }    
    public int GetSpeed()
    {
        return (int)Math.Floor((double)PokemonStats.Speed * StatLevelFactor[PokemonStats.SpeedChangeLevel + 6]);
    }
    public int GetAtkChangeLevel() => PokemonStats.AtkChangeLevel;
    public int GetDefChangeLevel() => PokemonStats.DefChangeLevel;
    public int GetSAtkChangeLevel() => PokemonStats.SAtkChangeLevel;
    public int GetSDefChangeLevel() => PokemonStats.SDefChangeLevel;
    public int GetSpeedChangeLevel() => PokemonStats.SpeedChangeLevel;
    public int GetAccuracyrateLevel() => PokemonStats.AccuracyrateLevel;
    public int GetEvasionrateLevel() => PokemonStats.EvasionrateLevel;
    public int GetLevel() => PokemonStats.Level;
    public int GetCTLevel() => PokemonStats.CTLevel;

    public bool ChangeStat(string StatName, int Level)
    {
        int ChangeLevel = Level;
        bool Result = false;
        if(StatName == "Atk")
        {
            int NewValue = Mathf.Min(Mathf.Max(-6, PokemonStats.AtkChangeLevel + Level), 6);
            if(NewValue != PokemonStats.AtkChangeLevel)
            {
                Result = true;
            }
            PokemonStats.AtkChangeLevel = NewValue;
        }
        if(StatName == "Def")
        {
            int NewValue = Mathf.Min(Mathf.Max(-6, PokemonStats.DefChangeLevel + Level), 6);
            if(NewValue != PokemonStats.DefChangeLevel)
            {
                Result = true;
            }
            PokemonStats.DefChangeLevel = NewValue;
        }
        if(StatName == "SAtk")
        {
            int NewValue = Mathf.Min(Mathf.Max(-6, PokemonStats.SAtkChangeLevel + Level), 6);
            if(NewValue != PokemonStats.SAtkChangeLevel)
            {
                Result = true;
            }
            PokemonStats.SAtkChangeLevel = NewValue;
        }
        if(StatName == "SDef")
        {
            int NewValue = Mathf.Min(Mathf.Max(-6, PokemonStats.SDefChangeLevel + Level), 6);
            if(NewValue != PokemonStats.SDefChangeLevel)
            {
                Result = true;
            }
            PokemonStats.SDefChangeLevel = Mathf.Min(Mathf.Max(-6, PokemonStats.SDefChangeLevel + Level), 6);
        }
        if(StatName == "Speed")
        {
            int NewValue = Mathf.Min(Mathf.Max(-6, PokemonStats.SpeedChangeLevel + Level), 6);
            if(NewValue != PokemonStats.SpeedChangeLevel)
            {
                Result = true;
            }
            PokemonStats.SpeedChangeLevel = Mathf.Min(Mathf.Max(-6, PokemonStats.SpeedChangeLevel + Level), 6);
        }
        if(StatName == "Accuracyrate")
        {
            int NewValue = Mathf.Min(Mathf.Max(-3, PokemonStats.AccuracyrateLevel + Level), 3);
            if(NewValue != PokemonStats.AccuracyrateLevel)
            {
                Result = true;
            }
            PokemonStats.AccuracyrateLevel = Mathf.Min(Mathf.Max(-3, PokemonStats.AccuracyrateLevel + Level), 3);
        }
        if(StatName == "Evasionrate")
        {
            int NewValue = Mathf.Min(Mathf.Max(-3, PokemonStats.EvasionrateLevel + Level), 3);
            if(NewValue != PokemonStats.EvasionrateLevel)
            {
                Result = true;
            }
            PokemonStats.EvasionrateLevel = Mathf.Min(Mathf.Max(-3, PokemonStats.EvasionrateLevel + Level), 3);
        }
        if(StatName == "CT")
        {            
            int NewValue = Mathf.Min(Mathf.Max(0, PokemonStats.CTLevel + Level), 3);
            if(NewValue != PokemonStats.CTLevel)
            {
                Result = true;
            }
            PokemonStats.CTLevel = Mathf.Min(Mathf.Max(0, PokemonStats.CTLevel + Level), 3);
        }
        return Result;
    }

    public EType GetType1() { return Type1;}
    public EType GetType2() { return Type2;}


    public bool TakenDamage(int Damage)
    {
        PokemonStats.HP -= Damage;
        if(PokemonStats.HP <= 0)
        {
            PokemonStats.Dead = true;
        }
        return PokemonStats.Dead;
    }

    public bool IsDead()
    {
        return PokemonStats.Dead;
    }

    public void LoadBasePokemonStats()
    {
        PokemonStats.Atk = ReferenceBasePokemon.GetAtk();
        PokemonStats.SAtk = ReferenceBasePokemon.GetSAtk();
        PokemonStats.Def = ReferenceBasePokemon.GetDef();
        PokemonStats.SDef = ReferenceBasePokemon.GetSDef();
        PokemonStats.MaxHP = ReferenceBasePokemon.GetMaxHP();
        PokemonStats.Speed = ReferenceBasePokemon.GetSpeed();
        PokemonStats.Level = ReferenceBasePokemon.GetLevel();
        PokemonStats.HP = ReferenceBasePokemon.GetHP();
        
        Type1 = ReferenceBasePokemon.GetType0();
        Type2 = ReferenceBasePokemon.GetType1();
    }
}
