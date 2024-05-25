using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BaseAbility : MonoBehaviour
{
    [SerializeField]
    protected string Name;
    [SerializeField]
    protected string Description;
    [SerializeField]
    protected BattlePokemon ReferencePokemon;
    protected bool HasTriggerd = false;

    private bool Processing = false;

    public bool GetTriggered()
    {
        return HasTriggerd;
    }
    public void ResetState()
    {
        HasTriggerd = false;
    }
    public virtual bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        return false;
    }

    public string GetAbilityName()
    {
        return this.Name;
    }

    public string GetAbilityDesc()
    {
        return this.Description;
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

    public BattlePokemon GetReferencePokemon() => ReferencePokemon;
    public void SetReferencePokemon(BattlePokemon InPokemon) { ReferencePokemon = InPokemon;}
    public void SetIsProcessing(bool IsProcessing) { Processing = IsProcessing;} 
    public bool GetIsProcessing() { return Processing;} 
}

public class AbilityComparer : IComparer<BaseAbility>
{
    public int Compare(BaseAbility x, BaseAbility y)
    {
        if(x.GetReferencePokemon().GetSpeed(ECaclStatsMode.Normal, BattleManager.StaticManager) < y.GetReferencePokemon().GetSpeed(ECaclStatsMode.Normal, BattleManager.StaticManager))
        {
            return 1;
        }
        else if(x.GetReferencePokemon().GetSpeed(ECaclStatsMode.Normal, BattleManager.StaticManager) > y.GetReferencePokemon().GetSpeed(ECaclStatsMode.Normal, BattleManager.StaticManager))
        {
            return -1;
        }
        return 0;
    }
}
