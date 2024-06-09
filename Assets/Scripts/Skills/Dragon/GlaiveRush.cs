using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlaiveRush : DamageSkill
{
    public override void AfterSkillEffectEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        SourcePokemon.SetGlaiveRushState();
    }
}
