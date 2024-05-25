using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsychicNoise : DamageSkill
{
    public override void AfterSkillEffectEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        SetPokemonStatusChangeEvent setStatChangeEvent = new SetPokemonStatusChangeEvent(TargetPokemon, SourcePokemon, InManager, EStatusChange.ForbidHeal, 2, true);
        setStatChangeEvent.Process(InManager);
    }

    public override bool IsAfterSkillEffectToTargetPokemon()
    {
        return true;
    }
}
