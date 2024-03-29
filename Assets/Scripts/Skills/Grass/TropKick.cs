using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TropKick : DamageSkill
{
    public override void AfterSkillEffectEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        StatChangeEvent defeatedEvent = new StatChangeEvent(TargetPokemon, "Atk", -1);
        defeatedEvent.Process(InManager);
    }
}
