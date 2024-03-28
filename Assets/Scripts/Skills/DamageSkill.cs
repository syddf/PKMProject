using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class DamageSkill : BaseSkill
{
    private static double[,] typeEffectiveness = new double[,] 
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

    public double ApplyTerrainPowerFactor(BattleManager InManager, BattlePokemon SourcePokemon, double SourcePower)
    {
        if(InManager.GetTerrainType() == EBattleFieldTerrain.Grass && GetSkillType(SourcePokemon) == EType.Grass)
        {
            EditorLog.DebugLog(SkillName + "因青草场地威力提高了!");
            return (int)Math.Floor(SourcePower * 1.3);
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
        Result = ApplyTerrainPowerFactor(InManager, SourcePokemon, Result);
        if(SourcePokemon.GetAbility())
        {
            Result = SourcePokemon.GetAbility().ChangeSkillPower(InManager, this, SourcePokemon, TargetPokemon, Result);
        }
        if(TargetPokemon.GetAbility())
        {
            Result = TargetPokemon.GetAbility().ChangeSkillPower(InManager, this, SourcePokemon, TargetPokemon, Result);
        }
        return (int)Math.Floor(Result);
    }

    private ECaclStatsMode GetSourceAtkCaclMode(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, bool CT)
    {
        bool IgnorBuf = false;
        bool IgnorDebuff = false;
        if(CT)
        {
            IgnorDebuff = true;
        }
        if(TargetPokemon.HasAbility("纯朴"))
        {
            IgnorBuf = true;
            IgnorDebuff = true;
        }
        if(IgnorDebuff && IgnorBuf) return ECaclStatsMode.IgnoreDebufAndBuf;
        if(IgnorDebuff) return ECaclStatsMode.IgnoreDebuf;
        if(IgnorBuf) return ECaclStatsMode.IgnoreBuf;
        return ECaclStatsMode.Normal;
    }
    private ECaclStatsMode GetTargetDefCaclMode(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, bool CT)
    {
        bool IgnorBuf = false;
        bool IgnorDebuff = false;
        if(CT)
        {
            IgnorBuf = true;
        }
        if(SourcePokemon.HasAbility("纯朴"))
        {
            IgnorBuf = true;
            IgnorDebuff = true;
        }
        if(IgnorDebuff && IgnorBuf) return ECaclStatsMode.IgnoreDebufAndBuf;
        if(IgnorDebuff) return ECaclStatsMode.IgnoreDebuf;
        if(IgnorBuf) return ECaclStatsMode.IgnoreBuf;
        return ECaclStatsMode.Normal;
    }

    public virtual void AfterDamageEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {

    }

    public override bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        bool J2 = TargetPokemon.GetType2() == EType.None;
        return typeEffectiveness[(int)GetSkillType(SourcePokemon), (int)TargetPokemon.GetType1()] != 0 
        && (J2 || typeEffectiveness[(int)GetSkillType(SourcePokemon), (int)TargetPokemon.GetType2()] != 0);
    }

    public virtual bool IsSameType(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        return SourcePokemon.GetType1() == GetSkillType(SourcePokemon) || SourcePokemon.GetType2() == GetSkillType(SourcePokemon);        
    }

    public virtual double GetSameTypePowerFactor(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        return 1.5;
    }

    public virtual double GetTypeEffectiveFactor(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        double Factor2 = TargetPokemon.GetType2() == EType.None ? 1.0 : typeEffectiveness[(int)GetSkillType(SourcePokemon), (int)TargetPokemon.GetType2()];
        return typeEffectiveness[(int)GetSkillType(SourcePokemon), (int)TargetPokemon.GetType1()] * Factor2;
    }

    public virtual int GetSourceAtk(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, bool CT)
    {
        ECaclStatsMode Mode = GetSourceAtkCaclMode(InManager, SourcePokemon, TargetPokemon, CT);
        if(this.SkillClass ==  ESkillClass.PhysicalMove)
        {
            return SourcePokemon.GetAtk(Mode);
        }
        return SourcePokemon.GetSAtk(Mode);
    }

    public virtual int GetTargetDef(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, bool CT)
    {
        ECaclStatsMode Mode = GetTargetDefCaclMode(InManager, SourcePokemon, TargetPokemon, CT);
        if(this.SkillClass ==  ESkillClass.PhysicalMove)
        {
            return TargetPokemon.GetDef(Mode);
        }
        return TargetPokemon.GetSDef(Mode);
    }

    public virtual double GetCTFactor(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        return 1.5;
    }
}
