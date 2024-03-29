using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafStorm : DamageSkill
{
    public override void AfterSkillEffectEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        StatChangeEvent statChangeEvent = new StatChangeEvent(SourcePokemon, "SAtk", -2);
        statChangeEvent.Process(InManager);
    }
}
