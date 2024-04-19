using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagPokemonSkillAI : MonoBehaviour
{
    public virtual double GetSkillPriorityFactor(BaseSkill InSkill, BattleManager InManager, BattlePokemon ReferencePokemon)
    {
        return 1.0;
    }
}
