using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Struggle : DamageSkill
{
    public override void AfterDamageEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, int Damage)
    {
        int selfDamage = Math.Min(SourcePokemon.GetHP(), Math.Max(1, SourcePokemon.GetMaxHP() / 4));
        DamageEvent damageEvent = new DamageEvent(SourcePokemon, selfDamage, "挣扎的效果");
        damageEvent.Process(InManager);   
    }
}
