using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossPoison : DamageSkill
{
    public override void AfterSkillEffectEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        SetPokemonStatusChangeEvent setStatChangeEvent = new SetPokemonStatusChangeEvent(TargetPokemon, SourcePokemon, InManager, EStatusChange.Poison, 1, false);
        setStatChangeEvent.Process(InManager);
    }

    public override int AfterSkillEffectEventProbablity(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        return 10;
    }

    public override bool IsAfterSkillEffectToTargetPokemon()
    {
        return true;
    }
    public override int GetCTRatio()
    {
        return 1;
    }
}
