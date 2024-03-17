using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSkill : BaseSkill
{
    public virtual int GetPower(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        return Power;
    }

    public virtual bool IsSameType(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        return SourcePokemon.GetType1() == this.SkillType || SourcePokemon.GetType2() == this.SkillType;        
    }

    public virtual float GetSameTypePowerFactor(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        return 1.5f;
    }

    public virtual int GetSourceAtk(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        if(this.SkillClass ==  ESkillClass.PhysicalMove)
        {
            return SourcePokemon.GetAtk();
        }
        return SourcePokemon.GetSAtk();
    }

    public virtual int GetTargetDef(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        if(this.SkillClass ==  ESkillClass.PhysicalMove)
        {
            return TargetPokemon.GetDef();
        }
        return TargetPokemon.GetSDef();
    }
}
