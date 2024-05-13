using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperPower : DamageSkill
{
    public override void AfterSkillEffectEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        StatChangeEvent statChangeEvent = new StatChangeEvent(SourcePokemon, SourcePokemon, "Atk", -1);
        statChangeEvent.Process(InManager);
        statChangeEvent = new StatChangeEvent(SourcePokemon, SourcePokemon, "Def", -1);
        statChangeEvent.Process(InManager);
    }
}