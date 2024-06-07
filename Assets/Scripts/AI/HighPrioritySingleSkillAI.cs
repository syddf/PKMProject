using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighPrioritySingleSkillAI : BagPokemonSkillAI
{
    public string SkillName;
    public override double GetSkillPriorityFactor(BaseSkill InSkill, BattleManager InManager, BattlePokemon ReferencePokemon, Event InPlayerAction)
    {
        if(InSkill.GetSkillName() == SkillName)
        {
            return 1000.0;
        }
        return 1.0;
    }
}