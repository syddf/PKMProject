using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPokemonAI : BagPokemonSkillAI
{
    public override double GetSkillPriorityFactor(BaseSkill InSkill, BattleManager InManager, BattlePokemon ReferencePokemon)
    {
        if(BattleSkillMetaInfo.IsSoundSkill(InSkill.GetSkillName()))
        {
            return 5.0;
        }
        return 1.0;
    }
}
