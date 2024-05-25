using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockTomb : DamageSkill
{
    public override void AfterSkillEffectEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        StatChangeEvent statChangeEvent = new StatChangeEvent(TargetPokemon, SourcePokemon, "Speed", -1);
        statChangeEvent.Process(InManager);
    }

    public override bool IsAfterSkillEffectToTargetPokemon()
    {
        return true;
    }
}
