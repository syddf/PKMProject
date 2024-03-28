using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusSkill : BaseSkill
{
    public virtual bool CanActivateEffect()
    {
        return true;
    }
    public virtual void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
    }
}
