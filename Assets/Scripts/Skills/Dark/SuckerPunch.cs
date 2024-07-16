using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuckerPunch : DamageSkill
{
    public override bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, out string Reason)
    {
        Reason = "使用突袭失败了!突袭只能先于对手攻击招式使用!";
        return InManager.IsPokemonUseDamageSkillThisTurn(TargetPokemon) && TargetPokemon.GetActivated() == false;
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
