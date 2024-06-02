using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poltergeist : DamageSkill
{
    public override bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, out string Reason)
    {
        Reason = "使用灵骚失败了!灵骚只能在目标持有道具时才可使用!";
        return TargetPokemon.HasItem();
    }
}
