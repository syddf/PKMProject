using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BaseTrainerSkill : MonoBehaviour
{
    [SerializeField]
    protected string Name;
    [SerializeField]
    protected string Description;
    [SerializeField]
    protected PokemonTrainer ReferenceTrainer;

    private bool Processing;
    public virtual bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        return false;
    }
    public virtual List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        return null;
    }
    public virtual double GetPowerFactorWhenAttack(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, DamageSkill InSkill)
    {
        return 1.0;
    }
    public virtual double GetPowerFactorWhenDefense(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, DamageSkill InSkill)
    {
        return 1.0;
    }
    public void SetIsProcessing(bool IsProcessing) { Processing = IsProcessing;} 
    public bool GetIsProcessing() { return Processing;} 

    public string GetSkillName()
    {
        return Name;
    }

    public string GetSkillDescription()
    {
        return Description;
    }
    public PokemonTrainer GetReferenceTrainer()
    {
        return ReferenceTrainer;
    }
}
