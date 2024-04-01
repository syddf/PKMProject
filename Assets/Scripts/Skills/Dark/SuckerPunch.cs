using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuckerPunch : DamageSkill
{
    public override bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        return InManager.IsPokemonUseDamageSkillThisTurn(TargetPokemon);
    }
    public override int GetSkillPriority(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        if(InManager.IsPokemonUseDamageSkillThisTurn(TargetPokemon))
        {
            return 1;
        }
        return 0;
    }
}
