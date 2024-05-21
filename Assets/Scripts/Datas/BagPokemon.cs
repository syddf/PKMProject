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
    public static string GetChineseNameWithCorrection(PokemonNature nature)
    {
        string chineseName = "";
        string correction = "";

        switch (nature)
        {
            case PokemonNature.Hardy:
                chineseName = "勤奋";
                correction = "不修正";
                break;
            case PokemonNature.Lonely:
                chineseName = "怕寂寞";
                correction = "攻击↑, 防御↓";
                break;
            case PokemonNature.Adamant:
                chineseName = "固执";
                correction = "攻击↑, 特攻↓";
                break;
            case PokemonNature.Naughty:
                chineseName = "顽皮";
                correction = "攻击↑, 防御↓";
                break;
            case PokemonNature.Brave:
                chineseName = "勇敢";
                correction = "攻击↑, 速度↓";
                break;
            case PokemonNature.Bold:
                chineseName = "大胆";
                correction = "防御↑, 攻击↓";
                break;
            case PokemonNature.Docile:
                chineseName = "坦率";
                correction = "不修正";
                break;
            case PokemonNature.Impish:
                chineseName = "淘气";
                correction = "防御↑, 特攻↓";
                break;
            case PokemonNature.Lax:
                chineseName = "乐天";
                correction = "防御↑, 特防↓";
                break;
            case PokemonNature.Relaxed:
                chineseName = "悠闲";
                correction = "防御↑, 速度↓";
                break;
            case PokemonNature.Modest:
                chineseName = "内敛";
                correction = "特攻↑, 攻击↓";
                break;
            case PokemonNature.Mild:
                chineseName = "慢吞吞";
                correction = "特攻↑, 防御↓";
                break;
            case PokemonNature.Bashful:
                chineseName = "害羞";
                correction = "不修正";
                break;
            case PokemonNature.Rash:
                chineseName = "马虎";
                correction = "特攻↑, 特防↓";
                break;
            case PokemonNature.Quiet:
                chineseName = "冷静";
                correction = "特攻↑, 速度↓";
                break;
            case PokemonNature.Calm:
                chineseName = "温和";
                correction = "特防↑, 攻击↓";
                break;
            case PokemonNature.Gentle:
                chineseName = "温顺";
                correction = "特防↑, 防御↓";
                break;
            case PokemonNature.Careful:
                chineseName = "慎重";
                correction = "特防↑, 特攻↓";
                break;
            case PokemonNature.Quirky:
                chineseName = "浮躁";
                correction = "不修正";
                break;
            case PokemonNature.Sassy:
                chineseName = "自大";
                correction = "特防↑, 速度↓";
                break;
            case PokemonNature.Timid:
                chineseName = "胆小";
                correction = "速度↑, 攻击↓";
                break;
            case PokemonNature.Hasty:
                chineseName = "急躁";
                correction = "速度↑, 防御↓";
                break;
            case PokemonNature.Jolly:
                chineseName = "爽朗";
                correction = "速度↑, 特攻↓";
                break;
            case PokemonNature.Naive:
                chineseName = "天真";
                correction = "速度↑, 特防↓";
                break;
            case PokemonNature.Serious:
                chineseName = "认真";
                correction = "不修正";
                break;
            default:
                chineseName = "未知";
                correction = "未知";
                break;
        }

        return $"{chineseName} ({correction})";
    }
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
    private PokemonData SourcePokemonMegaData;
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
    private BaseAbility MegaAbility;
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
    [SerializeField]
    private BaseSkill[] SkillPool = new BaseSkill[8];
    private bool UseOverrideNatureForUI;
    private PokemonNature OverrideNatureForUI;

    public BaseSkill GetSkillPoolSkill(int Index)
    {
        return SkillPool[Index];
    }

    public void SetOverrideNature(PokemonNature InNature)
    {
        UseOverrideNatureForUI = true;
        OverrideNatureForUI = InNature;
    }

    public void DisableOverrideNature()
    {
        UseOverrideNatureForUI = false;
    }
    
    public PokemonNature GetNature()
    {
        return Nature;
    }
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
            if(UseOverrideNatureForUI)
            {
                fA = Top * NatureFactor[(int)OverrideNatureForUI, Index - 1]; 
            }
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

    public int GetAtk(bool Mega)
    {
        if(Mega)
        {
            return CaclStatus(SourcePokemonMegaData.Atk, 1);
        }
        return CaclStatus(SourcePokemonData.Atk, 1);
    }

    public int GetSAtk(bool Mega)
    {
        if(Mega)
        {
            return CaclStatus(SourcePokemonMegaData.SAtk, 3);
        }
        return CaclStatus(SourcePokemonData.SAtk, 3);
    }

    public int GetDef(bool Mega)
    {
        if(Mega)
        {
            return CaclStatus(SourcePokemonMegaData.Def, 2);
        }
        return CaclStatus(SourcePokemonData.Def, 2);
    }

    public int GetSDef(bool Mega)
    {
        if(Mega)
        {
            return CaclStatus(SourcePokemonMegaData.Spd, 4);
        }
        return CaclStatus(SourcePokemonData.Spd, 4);
    }

    public int GetSpeed(bool Mega)
    {
        if(Mega)
        {
            return CaclStatus(SourcePokemonMegaData.Spe, 5);
        }
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

    public EType GetType0(bool Mega)
    {
        if(Mega)
        {
            if(SourcePokemonMegaData.Type0 == "undefined")
            {
                return EType.None;
            }
            return (EType)Enum.Parse(typeof(EType), SourcePokemonMegaData.Type0);
        }
        if(SourcePokemonData.Type0 == "undefined")
        {
            return EType.None;
        }
        return (EType)Enum.Parse(typeof(EType), SourcePokemonData.Type0);
    }

    public EType GetType1(bool Mega)
    {        
        if(Mega)
        {
            if(SourcePokemonMegaData.Type1 == "undefined")
            {
                return EType.None;
            }
            return (EType)Enum.Parse(typeof(EType), SourcePokemonMegaData.Type1);
        }
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
        if(ReferenceSkill[Index] == null)
        {
            return SkillPool[Index];
        }
        return ReferenceSkill[Index];
    }

    public string GetPokemonName()
    {
        return PokemonName;
    }

    public BaseAbility GetAbility(bool Mega)
    {
        if(Mega)
        {
            return MegaAbility;
        }
        return Ability;
    }

    public void SetAbility(BaseAbility InAbility, bool Mega)
    {
        if(Mega)
        {
            MegaAbility = InAbility;
            return;
        }
        Ability = InAbility;
    }
    public void Awake() 
    {
        SourcePokemonData = PkmDataCollections.GetPokemonData(Name);
        if(CanMega)
        {
            SourcePokemonMegaData = PkmDataCollections.GetPokemonData(Name + "-Mega");
        }
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

    public void UpdateByOverrideData(SavedData InData, string TrainerName, string PkmName)
    {
        PokemonTrainer TargetTrainer = GameObject.Find("SingleBattle/AllTrainers/" + TrainerName).GetComponent<PokemonTrainer>();
        BagPokemon ReferencePkm = null;
        foreach(var Pkm in TargetTrainer.BagPokemons)
        {
            if(Pkm.GetPokemonName() == PkmName)
            {
                ReferencePkm = Pkm;
                break;
            }
        }

        int SkillIndex0 = 0;
        int SkillIndex1 = 1;
        int SkillIndex2 = 2;
        int SkillIndex3 = 3;
        BaseItem OverrideItem = null;
        PokemonNature OverrideNature = ReferencePkm.Nature;
        if(InData.SavedPlayerData.OverrideData.ContainsKey(TrainerName))
        {
            if(InData.SavedPlayerData.OverrideData[TrainerName].ContainsKey(PkmName))
            {
                if(InData.SavedPlayerData.OverrideData[TrainerName][PkmName].Overrided)
                {
                    string OverrideTrainerName = InData.SavedPlayerData.OverrideData[TrainerName][PkmName].ReplaceTrainerName;
                    string OverridePokemonName = InData.SavedPlayerData.OverrideData[TrainerName][PkmName].ReplacePokemonName;
                    SkillIndex0 = InData.SavedPlayerData.OverrideData[TrainerName][PkmName].SkillIndex0;
                    SkillIndex1 = InData.SavedPlayerData.OverrideData[TrainerName][PkmName].SkillIndex1;
                    SkillIndex2 = InData.SavedPlayerData.OverrideData[TrainerName][PkmName].SkillIndex2;
                    SkillIndex3 = InData.SavedPlayerData.OverrideData[TrainerName][PkmName].SkillIndex3;
                    int ItemOverrideIndex = InData.SavedPlayerData.OverrideData[TrainerName][PkmName].ItemIndex;
                    OverrideNature = InData.SavedPlayerData.OverrideData[TrainerName][PkmName].Nature;
                    GameObject TrainerObj = GameObject.Find("SingleBattle/AllTrainers/" + OverrideTrainerName);
                    PokemonTrainer Trainer = TrainerObj.GetComponent<PokemonTrainer>();
                    foreach(var BagPkm in Trainer.BagPokemons)
                    {
                        if(BagPkm.GetPokemonName() == OverridePokemonName)
                        {
                            ReferencePkm = BagPkm;
                            break;
                        }
                    }
                    OverrideItem = Trainer.BagPokemons[ItemOverrideIndex].Item;
                    
                }
            }
        }

        SourcePokemonData = ReferencePkm.SourcePokemonData;
        SourcePokemonMegaData = ReferencePkm.SourcePokemonMegaData;
        PkmDataCollections = ReferencePkm.PkmDataCollections;
        Name = ReferencePkm.Name;
        Level = ReferencePkm.Level;
        HP = ReferencePkm.HP;
        Nature = OverrideNature;

        IVs[0] = ReferencePkm.IVs[0];
        IVs[1] = ReferencePkm.IVs[1];
        IVs[2] = ReferencePkm.IVs[2];
        IVs[3] = ReferencePkm.IVs[3];
        IVs[4] = ReferencePkm.IVs[4];
        IVs[5] = ReferencePkm.IVs[5];

        BasePoints[0] = ReferencePkm.BasePoints[0];
        BasePoints[1] = ReferencePkm.BasePoints[1];
        BasePoints[2] = ReferencePkm.BasePoints[2];
        BasePoints[3] = ReferencePkm.BasePoints[3];
        BasePoints[4] = ReferencePkm.BasePoints[4];
        BasePoints[5] = ReferencePkm.BasePoints[5];

        Gender = ReferencePkm.Gender;
        Item = ReferencePkm.Item;
        if(OverrideItem)
        {
            Item = OverrideItem;
        }
        Ability = ReferencePkm.Ability;
        MegaAbility = ReferencePkm.MegaAbility;

        ReferenceSkill[0] = ReferencePkm.SkillPool[SkillIndex0];
        ReferenceSkill[1] = ReferencePkm.SkillPool[SkillIndex1];
        ReferenceSkill[2] = ReferencePkm.SkillPool[SkillIndex2];
        ReferenceSkill[3] = ReferencePkm.SkillPool[SkillIndex3];
        if(ReferenceSkill[0] == null)
        {
            ReferenceSkill[0] = ReferencePkm.ReferenceSkill[0];
        }
        if(ReferenceSkill[1] == null)
        {
            ReferenceSkill[1] = ReferencePkm.ReferenceSkill[1];
        }
        if(ReferenceSkill[2] == null)
        {
            ReferenceSkill[2] = ReferencePkm.ReferenceSkill[2];
        }
        if(ReferenceSkill[3] == null)
        {
            ReferenceSkill[3] = ReferencePkm.ReferenceSkill[3];
        }
        PokemonName = ReferencePkm.PokemonName;
        MultiSpecies = ReferencePkm.MultiSpecies;
        SpecieIndex = ReferencePkm.SpecieIndex;
        CanMega = ReferencePkm.CanMega;

        SkillPool[0] = ReferencePkm.SkillPool[0];
        SkillPool[1] = ReferencePkm.SkillPool[1];
        SkillPool[2] = ReferencePkm.SkillPool[2];
        SkillPool[3] = ReferencePkm.SkillPool[3];
        SkillPool[4] = ReferencePkm.SkillPool[4];
        SkillPool[5] = ReferencePkm.SkillPool[5];
        SkillPool[6] = ReferencePkm.SkillPool[6];
        SkillPool[7] = ReferencePkm.SkillPool[7];
    }
}
