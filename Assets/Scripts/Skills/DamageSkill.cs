using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class DamageSkill : BaseSkill
{
    public static double[,] typeEffectiveness = new double[,] 
    {
         // 一般, 格斗, 飞行, 毒, 地面, 岩石, 虫, 幽灵, 钢, 火, 水, 草, 电, 超能力, 冰, 龙, 恶, 妖精
        {1, 1, 1, 1, 1, 0.5, 1, 0, 0.5, 1, 1, 1, 1, 1, 1, 1, 1, 1}, // 一般
        {2, 1, 0.5, 0.5, 1, 2, 0.5, 0, 2, 1, 1, 1, 1, 0.5, 2, 1, 2, 0.5}, // 格斗
        {1, 2, 1, 1, 1, 0.5, 2, 1, 0.5, 1, 1, 2, 0.5, 1, 1, 1, 1, 1}, // 飞行
        {1, 1, 1, 0.5, 0.5, 0.5, 1, 0.5, 0, 1, 1, 2, 1, 1, 1, 1, 1, 2}, // 毒
        {1, 1, 0, 2, 1, 2, 0.5, 1, 2, 2, 1, 0.5, 2, 1, 1, 1, 1, 1}, // 地面
        {1, 0.5, 2, 1, 0.5, 1, 2, 1, 0.5, 2, 1, 1, 1, 1, 2, 1, 1, 1}, // 岩石
        {1, 0.5, 0.5, 0.5, 1, 1, 1, 0.5, 0.5, 0.5, 1, 2, 1, 2, 1, 1, 2, 0.5}, // 虫
        {0, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 2, 1, 1, 0.5, 1}, // 幽灵
        {1, 1, 1, 1, 1, 2, 1, 1, 0.5, 0.5, 0.5, 1, 0.5, 1, 2, 1, 1, 2}, // 钢
        {1, 1, 1, 1, 1, 0.5, 2, 1, 2, 0.5, 0.5, 2, 1, 1, 2, 0.5, 1, 1}, // 火
        {1, 1, 1, 1, 2, 2, 1, 1, 1, 2, 0.5, 0.5, 1, 1, 1, 0.5, 1, 1}, // 水
        {1, 1, 0.5, 0.5, 2, 2, 0.5, 1, 0.5, 0.5, 2, 0.5, 1, 1, 1, 0.5, 1, 1}, // 草
        {1, 1, 2, 1, 0, 1, 1, 1, 1, 1, 2, 0.5, 0.5, 1, 1, 0.5, 1, 1}, // 电
        {1, 2, 1, 2, 1, 1, 1, 1, 0.5, 1, 1, 1, 1, 0.5, 1, 1, 0, 1}, // 超能力
        {1, 1, 2, 1, 2, 1, 1, 1, 0.5, 0.5, 0.5, 2, 1, 1, 0.5, 2, 1, 1}, // 冰
        {1, 1, 1, 1, 1, 1, 1, 1, 0.5, 1, 1, 1, 1, 1, 1, 2, 1, 0}, // 龙
        {1, 0.5, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 2, 1, 1, 0.5, 0.5}, // 恶
        {1, 2, 1, 0.5, 1, 1, 1, 1, 0.5, 0.5, 1, 1, 1, 1, 1, 2, 2, 1} // 妖精
    };

    public double ApplyTerrainPowerFactor(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, double SourcePower)
    {
        if(InManager.GetTerrainType() == EBattleFieldTerrain.Grass && GetSkillType(SourcePokemon) == EType.Grass && SourcePokemon.IsGroundPokemon(InManager))
        {
            EditorLog.DebugLog(SkillName + "因青草场地威力提高了!");
            return (int)Math.Floor(SourcePower * 1.3);
        }
        if(InManager.GetTerrainType() == EBattleFieldTerrain.Electric && GetSkillType(SourcePokemon) == EType.Electric && SourcePokemon.IsGroundPokemon(InManager))
        {
            EditorLog.DebugLog(SkillName + "因电气场地威力提高了!");
            return (int)Math.Floor(SourcePower * 1.3);
        }
        if(InManager.GetTerrainType() == EBattleFieldTerrain.Misty && GetSkillType(SourcePokemon) == EType.Dragon && TargetPokemon.IsGroundPokemon(InManager))
        {
            EditorLog.DebugLog(SkillName + "因薄雾场地威力降低了!");
            return (int)Math.Floor(SourcePower * 0.5);
        }
        return SourcePower;
    }
    protected virtual int GetSkillPower(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        return Power;
    }
    public int GetPower(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        double Result = GetSkillPower(InManager, SourcePokemon, TargetPokemon);
        Result = ApplyTerrainPowerFactor(InManager, SourcePokemon, TargetPokemon, Result);
        if(SourcePokemon.GetAbility())
        {
            Result = SourcePokemon.GetAbility().ChangeSkillPower(InManager, this, SourcePokemon, TargetPokemon, Result);
        }
        if(TargetPokemon.GetAbility())
        {
            Result = TargetPokemon.GetAbility().ChangeSkillPower(InManager, this, SourcePokemon, TargetPokemon, Result);
        }

        PokemonTrainer PlayerTrainer = InManager.GetPlayerTrainer();
        PokemonTrainer EnemyTrainer = InManager.GetEnemyTrainer();

        double TrainerFactor = 1.0;
        if(SourcePokemon.GetIsEnemy())
        {
            TrainerFactor = TrainerFactor * EnemyTrainer.TrainerSkill.GetPowerFactorWhenAttack(InManager, SourcePokemon, TargetPokemon, this);
            TrainerFactor = TrainerFactor * PlayerTrainer.TrainerSkill.GetPowerFactorWhenDefense(InManager, SourcePokemon, TargetPokemon, this);
        }
        else
        {
            TrainerFactor = TrainerFactor * EnemyTrainer.TrainerSkill.GetPowerFactorWhenDefense(InManager, SourcePokemon, TargetPokemon, this);
            TrainerFactor = TrainerFactor * PlayerTrainer.TrainerSkill.GetPowerFactorWhenAttack(InManager, SourcePokemon, TargetPokemon, this);
        }
        Result = Result * TrainerFactor;

        double ItemFactor = 1.0;
        if(SourcePokemon.HasItem("生命宝珠"))
        {
            ItemFactor *= 1.3;
        }
        if(SourcePokemon.HasItem("奇迹种子") && GetSkillType(SourcePokemon) == EType.Grass)
        {
            ItemFactor *= 1.2;
        }
        if(SourcePokemon.HasItem("硬石头") && GetSkillType(SourcePokemon) == EType.Rock)
        {
            ItemFactor *= 1.2;
        }
        if(SourcePokemon.HasConsumedThisTurn() && SourcePokemon.GetBattleItem() != null && SourcePokemon.GetBattleItem().GetBaseItem().IsGem())
        {
            Gem CastItem = (Gem)SourcePokemon.GetBattleItem().GetBaseItem();
            if(CastItem.GemType == GetSkillType(SourcePokemon))
            {
                ItemFactor *= 1.3;
            }
        }
        if(TargetPokemon.HasConsumedThisTurn() && TargetPokemon.GetBattleItem() != null && TargetPokemon.GetBattleItem().GetBaseItem().IsResistBerry())
        {
            ItemFactor *= 0.5;
        }
        Result = Result * ItemFactor;

        double WeatherFactor = 1.0;
        if(TargetPokemon.HasItem("万能伞") == false)
        {
            if(InManager.GetWeatherType() == EWeather.SunLight)
            {
                if(GetSkillType(SourcePokemon) == EType.Fire)
                {
                    WeatherFactor *= 1.5;
                }   
                if(GetSkillType(SourcePokemon) == EType.Water)
                {
                    WeatherFactor *= 0.5;
                }   
            }

            if(InManager.GetWeatherType() == EWeather.Rain)
            {
                if(GetSkillType(SourcePokemon) == EType.Water)
                {
                    WeatherFactor *= 1.5;
                }   
                if(GetSkillType(SourcePokemon) == EType.Fire)
                {
                    WeatherFactor *= 0.5;
                }   
            }
        }
        Result = Result * WeatherFactor;

        return (int)Math.Floor(Result);
    }

    protected ECaclStatsMode GetSourceAtkCaclMode(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, bool CT)
    {
        bool IgnorBuf = false;
        bool IgnorDebuff = false;
        if(CT)
        {
            IgnorDebuff = true;
        }
        if(TargetPokemon.HasAbility("纯朴", InManager, SourcePokemon, TargetPokemon))
        {
            IgnorBuf = true;
            IgnorDebuff = true;
        }
        if(IgnorDebuff && IgnorBuf) return ECaclStatsMode.IgnoreDebufAndBuf;
        if(IgnorDebuff) return ECaclStatsMode.IgnoreDebuf;
        if(IgnorBuf) return ECaclStatsMode.IgnoreBuf;
        return ECaclStatsMode.Normal;
    }
    protected ECaclStatsMode GetTargetDefCaclMode(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, bool CT)
    {
        bool IgnorBuf = false;
        bool IgnorDebuff = false;
        if(CT)
        {
            IgnorBuf = true;
        }
        if(SourcePokemon.HasAbility("纯朴", InManager, SourcePokemon, TargetPokemon))
        {
            IgnorBuf = true;
            IgnorDebuff = true;
        }
        if(IgnorDebuff && IgnorBuf) return ECaclStatsMode.IgnoreDebufAndBuf;
        if(IgnorDebuff) return ECaclStatsMode.IgnoreDebuf;
        if(IgnorBuf) return ECaclStatsMode.IgnoreBuf;
        return ECaclStatsMode.Normal;
    }

    public virtual void AfterDamageEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, int Damage)
    {

    }

    public override bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, out string Reason)
    {
        Reason = "";
        if(GetSkillType(SourcePokemon) == EType.None)
        {
            return true;
        }
        if(TargetPokemon == null)
        {
            return false;
        }
        bool J2 = TargetPokemon.GetType2(InManager, SourcePokemon, TargetPokemon) == EType.None;
        return typeEffectiveness[(int)GetSkillType(SourcePokemon), (int)TargetPokemon.GetType1(InManager, SourcePokemon, TargetPokemon)] != 0 
        && (J2 || typeEffectiveness[(int)GetSkillType(SourcePokemon), (int)TargetPokemon.GetType2(InManager, SourcePokemon, TargetPokemon)] != 0);
    }

    public virtual bool IsSameType(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        if(SourcePokemon.GetReferenceTrainer().TrainerSkill.GetSkillName() == "冰之摄影棚")
        {
            if(SourcePokemon.HasType(EType.Bug, InManager, SourcePokemon, TargetPokemon))
            {
                if(GetOriginSkillType() == EType.Bug)
                {
                    return true;
                }
            }
        }
        return SourcePokemon.GetType1(InManager, SourcePokemon, TargetPokemon) == GetSkillType(SourcePokemon) || SourcePokemon.GetType2(InManager, SourcePokemon, TargetPokemon) == GetSkillType(SourcePokemon);        
    }

    public virtual double GetSameTypePowerFactor(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        if(SourcePokemon.HasAbility("适应力", InManager, SourcePokemon, TargetPokemon))
        {
            return 2.0;
        }
        return 1.5;
    }

    public virtual double GetTypeEffectiveFactor(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        if(GetSkillType(SourcePokemon) == EType.None)
        {
            return 1.0;
        }
        double Factor2 = TargetPokemon.GetType2(InManager, SourcePokemon, TargetPokemon) == EType.None ? 1.0 : typeEffectiveness[(int)GetSkillType(SourcePokemon), (int)TargetPokemon.GetType2(InManager, SourcePokemon, TargetPokemon)];
        return typeEffectiveness[(int)GetSkillType(SourcePokemon), (int)TargetPokemon.GetType1(InManager, SourcePokemon, TargetPokemon)] * Factor2;
    }

    public virtual int GetSourceAtk(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, bool CT)
    {
        ECaclStatsMode Mode = GetSourceAtkCaclMode(InManager, SourcePokemon, TargetPokemon, CT);
        if(this.SkillClass ==  ESkillClass.PhysicalMove)
        {
            return SourcePokemon.GetAtk(Mode, InManager);
        }
        return SourcePokemon.GetSAtk(Mode, InManager);
    }

    public virtual int GetTargetDef(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, bool CT)
    {
        ECaclStatsMode Mode = GetTargetDefCaclMode(InManager, SourcePokemon, TargetPokemon, CT);
        if(this.SkillClass ==  ESkillClass.PhysicalMove)
        {
            return TargetPokemon.GetDef(Mode, InManager);
        }
        return TargetPokemon.GetSDef(Mode, InManager);
    }

    public virtual double GetCTFactor(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        return 1.5;
    }

    public virtual bool IsPhysicalMove(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        return this.SkillClass == ESkillClass.PhysicalMove;
    }

    public virtual bool IsConstantDamage(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        return false;
    }

    public virtual int GetConstantDamage(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        return 0;
    }
}

public class DamageAndSelfDamageSkill : DamageSkill
{
    public virtual int SelfDamageRatio()
    {
        return 2;
    }

    protected override int GetSkillPower(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        double Result = Power;
        if(SourcePokemon.HasAbility("舍身", InManager, SourcePokemon, TargetPokemon))
        {
            Result = (int)Math.Floor(Result * 1.2);
        }
        return (int)Result;
    }

    public override void AfterDamageEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, int Damage)
    {
        if(SourcePokemon.HasAbility("坚硬脑袋", InManager, SourcePokemon, TargetPokemon) || SourcePokemon.HasAbility("魔法防守", InManager, SourcePokemon, TargetPokemon))
        {
            return;
        }
        int selfDamage = Math.Min(SourcePokemon.GetHP(), Math.Max(1, Damage / SelfDamageRatio()));
        DamageEvent damageEvent = new DamageEvent(SourcePokemon, selfDamage, "反作用力");
        damageEvent.Process(InManager);   
    }
}