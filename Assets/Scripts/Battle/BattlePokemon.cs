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
    Frostbite,
    LeechSeed
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
        if(StatusChangeType == EStatusChange.LeechSeed)
        {
            return new LeechSeedStatusChange(InPokemon);
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
    private GameObject PokemonMegaModelObj;
    [SerializeField]
    private BagPokemon ReferenceBasePokemon;
    private BattlePokemonStat PokemonStats;
    private BattleItem Item;
    private static double[] StatLevelFactor = new double[13]{0.25, 0.29, 0.33, 0.40, 0.50, 0.67, 1.00, 1.50, 2.00, 2.50, 3.00, 3.50, 4.00};

    private bool LostItem = false;
    private BaseSkill FirstSkill = null;

    public BagPokemonSkillAI GetSkillAI()
    {
        return this.ReferenceBasePokemon.gameObject.GetComponent<BagPokemonSkillAI>();
    }
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

    private bool Mega = false;

    public void SwitchIn()
    {
        FirstIn = false;
    }
    public bool GetFirstIn()
    {
        return FirstIn;
    }

    public string GetEnName() => ReferenceBasePokemon.GetName();

    public string GetMaxStat()
    {
        int Atk = (int)(PokemonStats.Atk * StatLevelFactor[PokemonStats.AtkChangeLevel + 6]);
        int Def = (int)(PokemonStats.Def * StatLevelFactor[PokemonStats.DefChangeLevel + 6]);
        int SAtk = (int)(PokemonStats.SAtk * StatLevelFactor[PokemonStats.SAtkChangeLevel + 6]);
        int SDef = (int)(PokemonStats.SDef * StatLevelFactor[PokemonStats.SDefChangeLevel + 6]);
        int Speed = (int)(PokemonStats.Speed * StatLevelFactor[PokemonStats.SpeedChangeLevel + 6]);

        if(Atk >= Def && Atk >= SAtk && Atk >= SDef && Atk >= Speed)
            return "Atk";
        if(Def > Atk && Def >= SAtk && Def >= SDef && Def >= Speed)
            return "Def";
        if(SAtk > Atk && SAtk > Def && SAtk >= SDef && SAtk >= Speed)
            return "SAtk";
        if(SDef > Atk && SDef > Def && SDef > SAtk && SDef >= Speed)
            return "SDef";
        return "Speed";
    }
    
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
    public int GetAtk(ECaclStatsMode Mode, BattleManager InManager) 
    { 
        int ChangeLevel = AdjustChangeLevel(Mode, PokemonStats.AtkChangeLevel);
        double ItemFactor = 1.0;
        double AbilityFactor = 1.0;
        if(HasItem("讲究头带"))
        {
            ItemFactor = 1.5;
        }
        if(HasItem("驱劲能量") && GetMaxStat() == "Atk" && Item.IsConsumedState())
        {
            ItemFactor = 1.3;
        }
        else if(GetMaxStat() == "Atk" && HasAbility("古代活性", null, null, this) && InManager.GetWeatherType() == EWeather.SunLight)
        {
            AbilityFactor *= 1.3;
        }        
        if(HasAbility("活力", InManager, null, this))
        {
            AbilityFactor *= 1.5;
        }
        if(InManager.GetTerrainType() == EBattleFieldTerrain.Electric && HasAbility("科学助手", null, null, this))
        {
            AbilityFactor *= 2.0;
        }
        return (int)Math.Floor((double)PokemonStats.Atk * StatLevelFactor[ChangeLevel + 6] * ItemFactor * AbilityFactor);
    }
    public int GetDef(ECaclStatsMode Mode, BattleManager InManager)
    {
        int ChangeLevel = AdjustChangeLevel(Mode, PokemonStats.DefChangeLevel);
        double WeatherFactor = 1.0;
        double AbilityFactor = 1.0;
        double ItemFactor = 1.0;
        if(HasItem("驱劲能量") && GetMaxStat() == "Def" && Item.IsConsumedState())
        {
            ItemFactor = 1.3;
        }
        else if(GetMaxStat() == "Def" && HasAbility("古代活性", null, null, this) && InManager.GetWeatherType() == EWeather.SunLight)
        {
            AbilityFactor *= 1.3;
        }
        if(InManager.GetWeatherType() == EWeather.Snow && HasType(EType.Ice))
        {
            WeatherFactor = 1.5;
        }        
        return (int)Math.Floor((double)PokemonStats.Def * StatLevelFactor[ChangeLevel + 6] * WeatherFactor * AbilityFactor * ItemFactor);
    }
    public int GetSAtk(ECaclStatsMode Mode, BattleManager InManager)
    {
        int ChangeLevel = AdjustChangeLevel(Mode, PokemonStats.SAtkChangeLevel);
        double ItemFactor = 1.0;
        double AbilityFactor = 1.0;
        if(HasItem("讲究眼镜"))
        {
            ItemFactor = 1.5;
        }
        if(HasItem("驱劲能量") && GetMaxStat() == "SAtk" && Item.IsConsumedState())
        {
            ItemFactor = 1.3;
        }
        else if(GetMaxStat() == "SAtk" && HasAbility("古代活性", null, null, this) && InManager.GetWeatherType() == EWeather.SunLight)
        {
            AbilityFactor *= 1.3;
        }
        if(InManager.GetWeatherType() == EWeather.SunLight && HasAbility("太阳之力", InManager, null, this))
        {
            AbilityFactor *= 1.5;
        }
        if(InManager.GetTerrainType() == EBattleFieldTerrain.Electric && HasAbility("科学助手", null, null, this))
        {
            AbilityFactor *= 2.0;
        }  
        return (int)Math.Floor((double)PokemonStats.SAtk * StatLevelFactor[ChangeLevel + 6] * ItemFactor * AbilityFactor);
    }
    public int GetSDef(ECaclStatsMode Mode, BattleManager InManager)
    {
        int ChangeLevel = AdjustChangeLevel(Mode, PokemonStats.SDefChangeLevel);
        double ItemFactor = 1.0;
        double AbilityFactor = 1.0;
        if(HasItem("突击背心"))
        {
            ItemFactor = 1.5;
        }
        if(HasItem("驱劲能量") && GetMaxStat() == "SDef" && Item.IsConsumedState())
        {
            ItemFactor = 1.3;
        }
        else if(GetMaxStat() == "SDef" && HasAbility("古代活性", null, null, this) && InManager.GetWeatherType() == EWeather.SunLight)
        {
            AbilityFactor *= 1.3;
        } 
        double WeatherFactor = 1.0;
        if(InManager.GetWeatherType() == EWeather.Sand && HasType(EType.Rock))
        {
            WeatherFactor = 1.5;
        }

        return (int)Math.Floor((double)PokemonStats.SDef * StatLevelFactor[ChangeLevel + 6] * ItemFactor * WeatherFactor * AbilityFactor);
    }    
    public int GetSpeed(ECaclStatsMode Mode, BattleManager InManager)
    {
        int ChangeLevel = AdjustChangeLevel(Mode, PokemonStats.SpeedChangeLevel);
        double ParalysisFactor = 1.0;
        if(this.HasStatusChange(EStatusChange.Paralysis))
        {
            ParalysisFactor = 0.5;
        }
        double ItemFactor = 1.0;
        double AbilityFactor = 1.0;
        if(HasItem("讲究围巾"))
        {
            ItemFactor = 1.5;
        }
        if(HasItem("驱劲能量") && GetMaxStat() == "Speed" && Item.IsConsumedState())
        {
            ItemFactor = 1.5;
        }
        else if(GetMaxStat() == "Speed" && HasAbility("古代活性", null, null, this) && InManager.GetWeatherType() == EWeather.SunLight)
        {
            AbilityFactor *= 1.5;
        }
        if(HasAbility("轻装", null, null, this) && GetLostItem())
        {
            AbilityFactor *= 2.0;
        }         
        return (int)Math.Floor((double)PokemonStats.Speed * StatLevelFactor[ChangeLevel + 6] * ParalysisFactor * ItemFactor * AbilityFactor);
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

    public void SetBattlePokemonData(BagPokemon InBagPokemon, PokemonTrainer InTrainer, GameObject ModelObject, GameObject MegaModelObject)
    {
        ReferenceBasePokemon = InBagPokemon;
        IsEnemy = !InTrainer.IsPlayer;
        Ability = InBagPokemon.GetAbility(false);
        if(Ability)
        {
            Ability.SetReferencePokemon(this);
        }
        PokemonModelObj = ModelObject;
        PokemonMegaModelObj = MegaModelObject;
        LoadBasePokemonStats();
    }

    private void LoadBasePokemonStats()
    {
        Name = ReferenceBasePokemon.GetPokemonName();
        PokemonStats.Atk = ReferenceBasePokemon.GetAtk(false);
        PokemonStats.SAtk = ReferenceBasePokemon.GetSAtk(false);
        PokemonStats.Def = ReferenceBasePokemon.GetDef(false);
        PokemonStats.SDef = ReferenceBasePokemon.GetSDef(false);
        PokemonStats.MaxHP = ReferenceBasePokemon.GetMaxHP();
        PokemonStats.Speed = ReferenceBasePokemon.GetSpeed(false);
        PokemonStats.Level = ReferenceBasePokemon.GetLevel();
        PokemonStats.HP = ReferenceBasePokemon.GetHP();
        
        Type1 = ReferenceBasePokemon.GetType0(false);
        Type2 = ReferenceBasePokemon.GetType1(false);



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

    public GameObject GetOriginPokemonModel()
    {
        return PokemonModelObj;
    }
    public GameObject GetMegaPokemonModel()
    {
        return PokemonMegaModelObj;
    }


    public GameObject GetPokemonModel()
    {
        if(Mega)
        {
            return PokemonMegaModelObj;
        }
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

    private static readonly HashSet<string> SpecialAbilities = new HashSet<string>
    {
        "战斗盔甲", "恒净之躯", "湿气", "引火", "怪力钳", "精神力", "锐利目光", "飘浮", "神奇鳞片",
        "沙隐", "硬壳盔甲", "鳞粉", "隔音", "黏着", "结实", "吸盘", "厚脂肪", "蓄电", "储水",
        "白色烟雾", "神奇守护", "避雷针", "免疫", "迟钝", "干劲", "不眠", "柔软", "我行我素",
        "熔岩铠甲", "水幕", "干燥皮肤", "过滤", "耐热", "叶子防守", "电气引擎", "单纯", "雪隐",
        "坚硬岩石", "蹒跚", "纯朴", "引水", "花之礼", "健壮胸肌", "唱反调", "友情防守", "重金属",
        "轻金属", "魔法镜", "多重鳞片", "食草", "心灵感应", "奇迹皮肤", "防弹", "毛皮大衣",
        "防尘", "芳香幕", "甜幕", "花幕", "草之毛皮", "鲜艳之躯", "画皮", "女王的威严",
        "毛茸茸", "水泡", "镜甲", "庞克摇滚", "冰鳞粉", "结冻头", "粉彩护幕", "热交换",
        "洁净之盐", "焦香之躯", "乘风", "看门犬", "黄金之躯", "尾甲", "食土", "发光",
        "心眼", "太晶甲壳"
    };

    public static bool IsSpecialAbility(string AbilityName)
    {
        return SpecialAbilities.Contains(AbilityName);
    }

    public bool HasAbility(string AbilityName, BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        if(IsSpecialAbility(AbilityName) && SourcePokemon != null)
        {
            if(SourcePokemon.GetAbility().GetAbilityName() == "破格")
            {
                return false;
            }
        }
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
        Ability.ResetState();
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
    public bool GetLostItem()
    {
        return LostItem;
    } 

    public bool CanMega()
    {
        return ReferenceBasePokemon.GetCanMega() && Mega == false;
    }

    public bool IsMega()
    {
        return Mega;
    }

    public void MegaEvolution()
    {
        Mega = true;
        PokemonStats.Atk = ReferenceBasePokemon.GetAtk(Mega);
        PokemonStats.SAtk = ReferenceBasePokemon.GetSAtk(Mega);
        PokemonStats.Def = ReferenceBasePokemon.GetDef(Mega);
        PokemonStats.SDef = ReferenceBasePokemon.GetSDef(Mega);
        PokemonStats.Speed = ReferenceBasePokemon.GetSpeed(Mega);
        Ability = ReferenceBasePokemon.GetAbility(Mega);
        if(Ability)
        {
            Ability.SetReferencePokemon(this);
        }
        Type1 = ReferenceBasePokemon.GetType0(Mega);
        Type2 = ReferenceBasePokemon.GetType1(Mega);
    }
}
