using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endeavor : DamageSkill
{
    public override bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, out string Reason)
    {
        Reason = "";
        return TargetPokemon.GetHP() > SourcePokemon.GetHP();
    }
    public override bool IsConstantDamage(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        return true;
    }

    public override int GetConstantDamage(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        return TargetPokemon.GetHP() - SourcePokemon.GetHP();
    }
}
