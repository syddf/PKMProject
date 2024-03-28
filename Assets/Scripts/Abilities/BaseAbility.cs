using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAbility : MonoBehaviour
{
    [SerializeField]
    protected string Name;
    [SerializeField]
    protected string Description;
    [SerializeField]
    protected BattlePokemon ReferencePokemon;

    public virtual bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        return false;
    }

    public string GetAbilityName()
    {
        return this.Name;
    }

    public virtual List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        return null;
    }

    protected virtual double ChangeSkillPowerWhenAttack(BattleManager InManager, BaseSkill InSkill, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, double Power)
    {
        return Power;
    }
    protected virtual double ChangeSkillPowerWhenDefense(BattleManager InManager, BaseSkill InSkill, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, double Power)
    {
        return Power;
    }
    public double ChangeSkillPower(BattleManager InManager, BaseSkill InSkill, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, double Power)
    {
        double Result = Power;
        if(SourcePokemon.GetAbility() == this)
        {
            Result = ChangeSkillPowerWhenAttack(InManager, InSkill, SourcePokemon, TargetPokemon, Result);            
        }
        if(TargetPokemon.GetAbility() == this)
        {
            Result = ChangeSkillPowerWhenDefense(InManager, InSkill, SourcePokemon, TargetPokemon, Result); 
        }
        return Result;
    }

    protected virtual double ChangeSkillDamageWhenAttack(BattleManager InManager, BaseSkill InSkill, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, double Damage)
    {
        return Damage;
    }
    protected virtual double ChangeSkillDamageWhenDefense(BattleManager InManager, BaseSkill InSkill, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, double Damage)
    {
        return Damage;
    }

    public double ChangeSkillDamage(BattleManager InManager, BaseSkill InSkill, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, double Damage)
    {
        double Result = Damage;
        if(SourcePokemon.GetAbility() == this)
        {
            Result = ChangeSkillDamageWhenAttack(InManager, InSkill, SourcePokemon, TargetPokemon, Result);            
        }
        if(TargetPokemon.GetAbility() == this)
        {
            Result = ChangeSkillDamageWhenDefense(InManager, InSkill, SourcePokemon, TargetPokemon, Result); 
        }
        return Result;
    }

    public virtual int GetAbilitySkillPriority(BaseSkill InSkill)
    {
        return 0;
    }

    public BattlePokemon GetReferencePokemon() => ReferencePokemon;
}

public class AbilityComparer : IComparer<BaseAbility>
{
    public int Compare(BaseAbility x, BaseAbility y)
    {
        if(x.GetReferencePokemon().GetSpeed(ECaclStatsMode.Normal) < y.GetReferencePokemon().GetSpeed(ECaclStatsMode.Normal))
        {
            return 1;
        }
        else if(x.GetReferencePokemon().GetSpeed(ECaclStatsMode.Normal) > y.GetReferencePokemon().GetSpeed(ECaclStatsMode.Normal))
        {
            return -1;
        }
        return 0;
    }
}
