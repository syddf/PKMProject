using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPunch : DamageSkill
{
    public override void AfterSkillEffectEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        StatChangeEvent statChangeEvent = new StatChangeEvent(SourcePokemon, SourcePokemon, "Atk", 1);
        statChangeEvent.Process(InManager);
    }

    public override bool IsAfterSkillEffectToTargetPokemon()
    {
        return true;
    }
}