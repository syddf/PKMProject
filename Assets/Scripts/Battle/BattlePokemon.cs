using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum ECaclStatsMode
{
    Normal,
    IgnoreBuf,
    IgnoreDebuf,
    IgnoreDebufAndBuf
}

public enum EStatusChange
{
    None,
    ThroatChop,
    Protect,
    ForbidHeal,
    Flinch,
    Poison,
    Paralysis,
    Drowsy,
    Burn,
    Frostbite
}

public struct StatusChange
{
    public EStatusChange StatusChangeType;
    public bool HasLimitedTime;
    public int RemainTime;
    public BaseStatusChange ReferenceBaseStatusChange;
    public static BaseStatusChange GetBaseStatusChange(EStatusChange StatusChangeType, BattlePokemon InPokemon)
    {
        if(StatusChangeType == EStatusChange.ThroatChop)
        {
            return new ThroatChopStatusChange(InPokemon);
        }
        if(StatusChangeType == EStatusChange.Protect)
        {
            return new ProtectStatusChange(InPokemon);
        }
        if(StatusChangeType == EStatusChange.Poison)
        {
            return new PoisonStatusChange(InPokemon);
        }
        if(StatusChangeType == EStatusChange.ForbidHeal)
        {
            return new ForbidHealStatusChange(InPokemon);
        }
        if(StatusChangeType == EStatusChange.Flinch)
        {
            return new FlinchStatusChange(InPokemon);
        }
        if(StatusChangeType == EStatusChange.Paralysis)
        {
            return new ParalysisStatusChange(InPokemon);
        }
        if(StatusChangeType == EStatusChange.Burn)
        {
            return new BurnStatusChange(InPokemon);
        }
        if(StatusChangeType == EStatusChange.Frostbite)
        {
            return new FrostbiteStatusChange(InPokemon);
        } 
        if(StatusChangeType == EStatusChange.Drowsy)
        {
            return new DrowsyStatusChange(InPokemon);
        }       
        return null;
    }

    public static bool IsStatusChange(EStatusChange InStatusChangeType)
    {  
        return InStatusChangeType == EStatusChange.Poison || 
        InStatusChangeType == EStatusChange.Paralysis ||
        InStatusChangeType == EStatusChange.Drowsy ||
        InStatusChangeType == EStatusChange.Burn ||
        InStatusChangeType == EStatusChange.Frostbite;    
    }
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

    public List<StatusChange> StatusChangeList;
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

    private bool LostItem = false;
    private BaseSkill FirstSkill = null;
    public BattlePokemonStat CloneBattlePokemonStats()
    {
        BattlePokemonStat NewStats = PokemonStats;
        if(PokemonStats.StatusChangeList != null)
        {
            NewStats.StatusChangeList = PokemonStats.StatusChangeList.ToList();
        }
        return NewStats;
    }
    public BaseAbility GetAbility() => Ability;
    public bool GetIsEnemy() => IsEnemy;
    public string GetName() => Name;

    public int GetHP() => PokemonStats.HP;
    public int GetMaxHP() => PokemonStats.MaxHP;

    private bool FirstIn = true;

    public void SwitchIn()
    {
        FirstIn = false;
    }
    public bool GetFirstIn()
    {
        return FirstIn;
    }

    public string GetEnName() => ReferenceBasePokemon.GetName();
    
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
        double ItemFactor = 1.0;
        if(HasItem("讲究头带"))
        {
            ItemFactor = 1.5;
        }        
        return (int)Math.Floor((double)PokemonStats.Atk * StatLevelFactor[ChangeLevel + 6] * ItemFactor);
    }
    public int GetDef(ECaclStatsMode Mode)
    {
        int ChangeLevel = AdjustChangeLevel(Mode, PokemonStats.DefChangeLevel);  
        return (int)Math.Floor((double)PokemonStats.Def * StatLevelFactor[ChangeLevel + 6]);
    }
    public int GetSAtk(ECaclStatsMode Mode)
    {
        int ChangeLevel = AdjustChangeLevel(Mode, PokemonStats.SAtkChangeLevel);
        double ItemFactor = 1.0;
        if(HasItem("讲究眼镜"))
        {
            ItemFactor = 1.5;
        }  
        return (int)Math.Floor((double)PokemonStats.SAtk * StatLevelFactor[ChangeLevel + 6] * ItemFactor);
    }
    public int GetSDef(ECaclStatsMode Mode)
    {
        int ChangeLevel = AdjustChangeLevel(Mode, PokemonStats.SDefChangeLevel);
        double ItemFactor = 1.0;
        if(HasItem("突击背心"))
        {
            ItemFactor = 1.5;
        }  
        return (int)Math.Floor((double)PokemonStats.SDef * StatLevelFactor[ChangeLevel + 6] * ItemFactor);
    }    
    public int GetSpeed(ECaclStatsMode Mode)
    {
        int ChangeLevel = AdjustChangeLevel(Mode, PokemonStats.SpeedChangeLevel);
        double ParalysisFactor = 1.0;
        if(this.HasStatusChange(EStatusChange.Paralysis))
        {
            ParalysisFactor = 0.5;
        }
        double ItemFactor = 1.0;
        if(HasItem("讲究围巾"))
        {
            ItemFactor = 1.5;
        }         
        return (int)Math.Floor((double)PokemonStats.Speed * StatLevelFactor[ChangeLevel + 6] * ParalysisFactor * ItemFactor);
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
        if(HasItem() && Item.GetItemName() == "黑色铁球")
        {
            return true;
        }
        if(GetType1() == EType.Flying || GetType2() == EType.Flying)
        {
            return false;
        }
        if(Ability && Ability.name == "飘浮")
        {
            return false;
        }
        if(HasItem() && Item.GetItemName() == "气球")
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

    public void SetBattlePokemonData(BagPokemon InBagPokemon, PokemonTrainer InTrainer, GameObject ModelObject)
    {
        ReferenceBasePokemon = InBagPokemon;
        IsEnemy = !InTrainer.IsPlayer;
        Ability = InBagPokemon.GetAbility();
        if(Ability)
        {
            Ability.SetReferencePokemon(this);
        }
        PokemonModelObj = ModelObject;
        LoadBasePokemonStats();
    }

    private void LoadBasePokemonStats()
    {
        Name = ReferenceBasePokemon.GetPokemonName();
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
            ReferenceSkill[Index] = ReferenceBasePokemon.GetBaseSkill(Index);
            if(ReferenceSkill[Index] != null)
            {
                SkillPP[Index] = ReferenceSkill[Index].GetPP();
            }
        }

        Item = new BattleItem(ReferenceBasePokemon.GetItem(), this);
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

    public bool HasType(EType Type)
    {
        return GetType1() == Type || GetType2() == Type;
    }

    public bool AddStatusChange(EStatusChange StatusType, bool HasLimitedTime, int RemainTime)
    {
        if(PokemonStats.StatusChangeList == null)
        {
            PokemonStats.StatusChangeList = new List<StatusChange>();
        }

        for(int Index = 0; Index < PokemonStats.StatusChangeList.Count; Index++)
        {
            if(PokemonStats.StatusChangeList[Index].StatusChangeType == StatusType ||                
               ( StatusChange.IsStatusChange(StatusType) && StatusChange.IsStatusChange(PokemonStats.StatusChangeList[Index].StatusChangeType)) )
            {
                StatusChange OldStatus = PokemonStats.StatusChangeList[Index];
                OldStatus.HasLimitedTime = HasLimitedTime;
                OldStatus.RemainTime = Math.Max(OldStatus.RemainTime, RemainTime);
                OldStatus.StatusChangeType = StatusType;
                OldStatus.ReferenceBaseStatusChange = StatusChange.GetBaseStatusChange(StatusType, this);
                PokemonStats.StatusChangeList[Index] = OldStatus;
                return true;
            }
        }

        StatusChange NewStatus;
        NewStatus.HasLimitedTime = HasLimitedTime;
        NewStatus.RemainTime = RemainTime;
        NewStatus.StatusChangeType = StatusType;
        NewStatus.ReferenceBaseStatusChange = StatusChange.GetBaseStatusChange(StatusType, this);
        PokemonStats.StatusChangeList.Add(NewStatus);
        return false;
    }

    public void RemoveStatusChange(EStatusChange StatusType)
    {
        if(PokemonStats.StatusChangeList == null)
        {
            return;
        }

        for(int Index = 0; Index < PokemonStats.StatusChangeList.Count; Index++)
        {
            if(PokemonStats.StatusChangeList[Index].StatusChangeType == StatusType)
            {
                PokemonStats.StatusChangeList.RemoveAt(Index);
                return;
            }
        }
    }

    public bool HasStatusChange(EStatusChange StatusType)
    {
        if(PokemonStats.StatusChangeList == null)
        {
            return false;
        }

        for(int Index = 0; Index < PokemonStats.StatusChangeList.Count; Index++)
        {
            if(PokemonStats.StatusChangeList[Index].StatusChangeType == StatusType)
            {
                return true;
            }
        }

        return false;
    }

    public int GetStatusChangeRemainTime(EStatusChange StatusType)
    {
        if(PokemonStats.StatusChangeList == null)
        {
            return 0;
        }

        for(int Index = 0; Index < PokemonStats.StatusChangeList.Count; Index++)
        {
            if(PokemonStats.StatusChangeList[Index].StatusChangeType == StatusType)
            {
                return PokemonStats.StatusChangeList[Index].RemainTime;
            }
        }

        return 0;
    }

    public BaseStatusChange GetBaseStatusChange(EStatusChange StatusType)
    {
        if(PokemonStats.StatusChangeList == null)
        {
            return null;
        }

        for(int Index = 0; Index < PokemonStats.StatusChangeList.Count; Index++)
        {
            if(PokemonStats.StatusChangeList[Index].StatusChangeType == StatusType)
            {
                return PokemonStats.StatusChangeList[Index].ReferenceBaseStatusChange;
            }
        }

        return null;
    }

    public HashSet<BaseSkill> GetForbiddenBattleSkills(BattleManager InManager)
    {
        HashSet<BaseSkill> Result = new HashSet<BaseSkill>();
        for(int SkillIndex = 0; SkillIndex < 4; SkillIndex++)
        {
            if(SkillPP[SkillIndex] == 0)
            {
                Result.Add(ReferenceSkill[SkillIndex]);
            }

            if(BattleSkillMetaInfo.IsSoundSkill(ReferenceSkill[SkillIndex].GetSkillName()) && HasStatusChange(EStatusChange.ThroatChop))
            {
                Result.Add(ReferenceSkill[SkillIndex]);
            }

            if(ReferenceSkill[SkillIndex].HasHealEffect(InManager) && HasStatusChange(EStatusChange.ForbidHeal))
            {
                Result.Add(ReferenceSkill[SkillIndex]);
            }

            if(HasItem("讲究头带") || HasItem("讲究眼睛") || HasItem("讲究围巾"))
            {
                if(FirstSkill != null && ReferenceSkill[SkillIndex] != FirstSkill)
                {
                    Result.Add(ReferenceSkill[SkillIndex]);
                }
            }

            if(HasItem("突击背心"))
            {
                if(ReferenceSkill[SkillIndex].GetSkillClass() == ESkillClass.StatusMove)
                {
                    Result.Add(ReferenceSkill[SkillIndex]);
                }
            }
        }
        return Result;   
    }

    public List<EStatusChange> ReduceAllStatusChangeRemainTime()
    {
        List<EStatusChange> RemoveStatus = new List<EStatusChange>();
        if(PokemonStats.StatusChangeList == null)
        {
            return RemoveStatus;
        }

        for(int Index = PokemonStats.StatusChangeList.Count - 1; Index >= 0; Index--)
        {
            if(PokemonStats.StatusChangeList[Index].HasLimitedTime)
            {
                StatusChange OldStatus = PokemonStats.StatusChangeList[Index];
                OldStatus.RemainTime = OldStatus.RemainTime - 1;
                PokemonStats.StatusChangeList[Index] = OldStatus;
                if(PokemonStats.StatusChangeList[Index].RemainTime == 0)
                {
                    RemoveStatus.Add(PokemonStats.StatusChangeList[Index].StatusChangeType);
                }
            }
        }

        return RemoveStatus;
    }

    public void ClearStatusChange()
    {
        LostItem = false;
        FirstSkill = null;
        if(PokemonStats.StatusChangeList == null)
        {
            return;
        }
        for(int Index = PokemonStats.StatusChangeList.Count - 1; Index >= 0; Index--)
        {
            if(!StatusChange.IsStatusChange(PokemonStats.StatusChangeList[Index].StatusChangeType))
            {
                PokemonStats.StatusChangeList.RemoveAt(Index);
            }
        }
    }

    public void SetFirstSkill(BaseSkill ReferenceSkill)
    {
        FirstSkill = ReferenceSkill;
    }

    public List<BaseStatusChange> GetAllStatusChange()
    {
        List<BaseStatusChange> Result = new List<BaseStatusChange>();
        if(PokemonStats.StatusChangeList != null)
        {
            foreach(var statusChange in PokemonStats.StatusChangeList)
            {
                if(statusChange.ReferenceBaseStatusChange != null)
                {
                    Result.Add(statusChange.ReferenceBaseStatusChange);
                }
            }
        }
        return Result;
    }

    public BattleItem GetBattleItem()
    {
        return Item;
    }   

    public bool HasItem(string ItemName)
    {
        if(HasItem() && Item.GetItemName() == ItemName)
        {
            return true;
        }
        return false;
    }
    public bool HasItem()
    {
        return Item.HasItem();
    }

    public void SetLostItem()
    {
        LostItem = true;
    } 
    public void GetLostItem()
    {
        LostItem = true;
    }   
}
