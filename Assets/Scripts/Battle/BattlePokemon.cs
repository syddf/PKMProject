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

    private BattlePokemonStat PokemonStats;

    public BaseAbility GetAbility() => Ability;
    public bool GetIsEnemy() => IsEnemy;
    public string GetName() => Name;

    public int GetHP() => PokemonStats.HP;
    public void SetHP(int value) => PokemonStats.HP = value;

    public int GetMaxHP() => PokemonStats.MaxHP;
    public void SetMaxHP(int value) => PokemonStats.MaxHP = value;

    public int GetAtk() => PokemonStats.Atk;
    public void SetAtk(int value) => PokemonStats.Atk = value;

    public int GetDef() => PokemonStats.Def;
    public void SetDef(int value) => PokemonStats.Def = value;

    public int GetSAtk() => PokemonStats.SAtk;
    public void SetSAtk(int value) => PokemonStats.SAtk = value;

    public int GetSDef() => PokemonStats.SDef;
    public void SetSDef(int value) => PokemonStats.SDef = value;

    public int GetSpeed() => PokemonStats.Speed;
    public void SetSpeed(int value) => PokemonStats.Speed = value;

    public int GetAtkChangeLevel() => PokemonStats.AtkChangeLevel;
    public int GetDefChangeLevel() => PokemonStats.DefChangeLevel;
    public int GetSAtkChangeLevel() => PokemonStats.SAtkChangeLevel;
    public int GetSDefChangeLevel() => PokemonStats.SDefChangeLevel;
    public int GetSpeedChangeLevel() => PokemonStats.SpeedChangeLevel;
    public int GetAccuracyrateLevel() => PokemonStats.AccuracyrateLevel;
    public int GetEvasionrateLevel() => PokemonStats.EvasionrateLevel;
    public int GetLevel() => PokemonStats.Level;

    public void ChangeStat(string StatName, int Level)
    {
        int ChangeLevel = Level;
        if(StatName == "Atk")
        {
            PokemonStats.AtkChangeLevel = Mathf.Min(Mathf.Max(-6, PokemonStats.AtkChangeLevel + Level), 6);
        }
        if(StatName == "Def")
        {
            PokemonStats.DefChangeLevel = Mathf.Min(Mathf.Max(-6, PokemonStats.DefChangeLevel + Level), 6);
        }
        if(StatName == "SAtk")
        {
            PokemonStats.SAtkChangeLevel = Mathf.Min(Mathf.Max(-6, PokemonStats.SAtkChangeLevel + Level), 6);
        }
        if(StatName == "SDef")
        {
            PokemonStats.SDefChangeLevel = Mathf.Min(Mathf.Max(-6, PokemonStats.SDefChangeLevel + Level), 6);
        }
        if(StatName == "Speed")
        {
            PokemonStats.SpeedChangeLevel = Mathf.Min(Mathf.Max(-6, PokemonStats.SpeedChangeLevel + Level), 6);
        }
        if(StatName == "Accuracyrate")
        {
            PokemonStats.AccuracyrateLevel = Mathf.Min(Mathf.Max(-3, PokemonStats.AccuracyrateLevel + Level), 3);
        }
        if(StatName == "Evasionrate")
        {
            PokemonStats.EvasionrateLevel = Mathf.Min(Mathf.Max(-3, PokemonStats.EvasionrateLevel + Level), 3);
        }
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
}
