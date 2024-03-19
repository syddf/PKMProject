using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafStorm : DamageSkill
{
    public override void AfterSkillEffectEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        StatChangeEvent defeatedEvent = new StatChangeEvent(SourcePokemon, "SAtk", -2);
        defeatedEvent.Process(InManager);
    }
}
