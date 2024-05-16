using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonJab : DamageSkill
{
    public override void AfterSkillEffectEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        EStatusChange NewStatus = EStatusChange.Poison;
        SetPokemonStatusChangeEvent setStatChangeEvent = new SetPokemonStatusChangeEvent(TargetPokemon, SourcePokemon, InManager, NewStatus, 1, false);
        setStatChangeEvent.Process(InManager);
    }

    public override int AfterSkillEffectEventProbablity(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        return 30;
    }
}
