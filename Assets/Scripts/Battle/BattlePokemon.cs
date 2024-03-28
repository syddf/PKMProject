using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public enum ECaclStatsMode
{
    Normal,
    IgnoreBuf,
    IgnoreDebuf,
    IgnoreDebufAndBuf
}

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
    private GameObject PokemonModelObj;
    [SerializeField]
    private BagPokemon ReferenceBasePokemon;
    private BattlePokemonStat PokemonStats;
    private BattleItem Item;
    private static double[] StatLevelFactor = new double[13]{0.25, 0.29, 0.33, 0.40, 0.50, 0.67, 1.00, 1.50, 2.00, 2.50, 3.00, 3.50, 4.00};

    public BattlePokemonStat CloneBattlePokemonStats()
    {
        return PokemonStats;
    }
    public BaseAbility GetAbility() => Ability;
    public bool GetIsEnemy() => IsEnemy;
    public string GetName() => Name;

    public int GetHP() => PokemonStats.HP;
    public int GetMaxHP() => PokemonStats.MaxHP;

    private int AdjustChangeLevel(ECaclStatsMode Mode, int SourceChangeLevel)
    {
        int ChangeLevel = SourceChangeLevel;
        if((Mode == ECaclStatsMode.IgnoreBuf || Mode == ECaclStatsMode.IgnoreDebufAndBuf) && ChangeLevel > 0)
        {
            ChangeLevel = 0;
        }
        if((Mode == ECaclStatsMode.IgnoreDebuf || Mode == ECaclStatsMode.IgnoreDebufAndBuf) && ChangeLevel < 0)
        {
            ChangeLevel = 0;
        }
        return ChangeLevel;
    }
    public int GetAtk(ECaclStatsMode Mode) 
    { 
        int ChangeLevel = AdjustChangeLevel(Mode, PokemonStats.AtkChangeLevel);        
        return (int)Math.Floor((double)PokemonStats.Atk * StatLevelFactor[ChangeLevel + 6]);
    }
    public int GetDef(ECaclStatsMode Mode)
    {
        int ChangeLevel = AdjustChangeLevel(Mode, PokemonStats.DefChangeLevel);  
        return (int)Math.Floor((double)PokemonStats.Def * StatLevelFactor[ChangeLevel + 6]);
    }
    public int GetSAtk(ECaclStatsMode Mode)
    {
        int ChangeLevel = AdjustChangeLevel(Mode, PokemonStats.SAtkChangeLevel);  
        return (int)Math.Floor((double)PokemonStats.SAtk * StatLevelFactor[ChangeLevel + 6]);
    }
    public int GetSDef(ECaclStatsMode Mode)
    {
        int ChangeLevel = AdjustChangeLevel(Mode, PokemonStats.SDefChangeLevel);  
        return (int)Math.Floor((double)PokemonStats.SDef * StatLevelFactor[ChangeLevel + 6]);
    }    
    public int GetSpeed(ECaclStatsMode Mode)
    {
        int ChangeLevel = AdjustChangeLevel(Mode, PokemonStats.SpeedChangeLevel);  
        return (int)Math.Floor((double)PokemonStats.Speed * StatLevelFactor[ChangeLevel + 6]);
    }
    public int GetAtkChangeLevel() => PokemonStats.AtkChangeLevel;
    public int GetDefChangeLevel() => PokemonStats.DefChangeLevel;
    public int GetSAtkChangeLevel() => PokemonStats.SAtkChangeLevel;
    public int GetSDefChangeLevel() => PokemonStats.SDefChangeLevel;
    public int GetSpeedChangeLevel() => PokemonStats.SpeedChangeLevel;
    public int GetAccuracyrateLevel(ECaclStatsMode Mode) => AdjustChangeLevel(Mode, PokemonStats.AccuracyrateLevel);
    public int GetEvasionrateLevel(ECaclStatsMode Mode) => AdjustChangeLevel(Mode, PokemonStats.EvasionrateLevel);
    public int GetLevel() => PokemonStats.Level;
    public int GetCTLevel() => PokemonStats.CTLevel;
    public PokemonGender GetGender() { return ReferenceBasePokemon.GetGender();}
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

    [SerializeField]
    private BaseSkill[] ReferenceSkill = new BaseSkill[4];
    [SerializeField]
    private int[] SkillPP = new int[4];

    public bool IsGroundPokemon()
    {
        if(Item && Item.GetItemName() == "黑色铁球")
        {
            return true;
        }
        if(GetType1() == EType.Flying || GetType2() == EType.Flying)
        {
            return false;
        }
        if(Ability.name == "飘浮")
        {
            return false;
        }
        if(Item && Item.GetItemName() == "气球")
        {
            return false;
        }        
        return true;
    }
    public bool TakenDamage(int Damage)
    {
        PokemonStats.HP -= Damage;
        if(PokemonStats.HP <= 0)
        {
            PokemonStats.Dead = true;
        }
        return PokemonStats.Dead;
    }

    public int HealHP(int HealHPVal)
    {
        HealHPVal = Math.Max(1, HealHPVal);
        int prevHP = PokemonStats.HP;
        PokemonStats.HP = Math.Min(PokemonStats.HP + HealHPVal, PokemonStats.MaxHP);
        return PokemonStats.HP - prevHP;
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

        for(int Index = 0; Index < 4; Index++)
        {
            if(ReferenceSkill[Index] != null)
            {
                SkillPP[Index] = ReferenceSkill[Index].GetPP();
            }
        }
    }

    public void ReducePP(BattleSkill InSkill)
    {
        for(int Index = 0; Index < 4; Index++)
        {
            if(ReferenceSkill[Index] != null && ReferenceSkill[Index].GetSkillName() == InSkill.GetSkillName())
            {
                SkillPP[Index] = SkillPP[Index] - 1;
            }
        }
    }

    public BaseSkill[] GetReferenceSkill()
    {
        return ReferenceSkill;
    } 
    public int GetSkillPP(BaseSkill InSkill)
    {
        int Index = 0;
        foreach(var Skill in ReferenceSkill)
        {
            if(Skill == InSkill)
            {
                return SkillPP[Index];
            }
            Index++;
        }

        return 0;
    }

    public int GetIndexInPKDex()
    {
        return ReferenceBasePokemon.GetIndexInPKDex();
    }
    public void Start()
    {
        LoadBasePokemonStats();
    }

    public GameObject GetPokemonModel()
    {
        return PokemonModelObj;
    }

    public int GetBattleSkillIndex(BaseSkill InSkill)
    {
        for(int Index = 0; Index < 4; Index++)
        {
            if(InSkill == ReferenceSkill[Index])
            {
                return Index;
            }
        }
        return -1;
    }

    public bool HasAbility(string AbilityName)
    {
        return Ability && Ability.GetAbilityName() == AbilityName;
    }
}
