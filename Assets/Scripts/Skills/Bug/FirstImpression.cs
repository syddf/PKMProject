using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstImpression : DamageSkill
{    
    public override bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, out string Reason)
    {
        if(base.JudgeIsEffective(InManager, SourcePokemon, TargetPokemon, out Reason) == false)
        {
            return false;
        }
        Reason = "使用迎头一击失败了！迎头一击只能在登场回合使用！";
        return InManager.GetCurrentTurnIndex() == 0 || InManager.IsPokemonInLastTurn(SourcePokemon);
    }
}
