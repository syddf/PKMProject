using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockResonance : DamageSkill
{
    public override void AfterSkillEffectEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        StatChangeEvent statChangeEvent = new StatChangeEvent(SourcePokemon, "SDef", 1);
        statChangeEvent.Process(InManager);
    }
}
