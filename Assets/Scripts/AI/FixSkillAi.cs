using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixSkillAi : BagPokemonSkillAI
{
    public int FixSkillIndex;
    public override double GetSkillPriorityFactor(BaseSkill InSkill, BattleManager InManager, BattlePokemon ReferencePokemon, Event InPlayerAction)
    {
        if(ReferencePokemon.GetBattleSkillIndex(InSkill) == FixSkillIndex)
        {
            return 1.0;
        }
        return 0.0;
    }
}