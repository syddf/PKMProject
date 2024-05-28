using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingSwipe : DamageSkill
{
    public override void AfterSkillEffectEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        StatChangeEvent statChangeEvent = new StatChangeEvent(TargetPokemon, SourcePokemon, "Atk", -1);
        statChangeEvent.Process(InManager);
    }

    public override bool IsAfterSkillEffectToTargetPokemon()
    {
        return true;
    }
}

