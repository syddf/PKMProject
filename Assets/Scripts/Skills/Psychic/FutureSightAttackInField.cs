using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FutureSightAttackInField : DamageSkill
{
    public override bool CanBeProtected()
    {
        return false;
    }

    protected override int GetSkillPower(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        if(SourcePokemon.GetReferenceTrainer().TrainerSkill.GetSkillName() == "预言家")
        {
            return Power / 2;
        }
        return Power;
    }
}
