using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FlareBlitz : DamageSkill
{
    public override void AfterDamageEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, int Damage)
    {
        int selfDamage = Math.Min(SourcePokemon.GetHP(), Math.Max(1, Damage / 3));
        DamageEvent damageEvent = new DamageEvent(SourcePokemon, selfDamage, "反作用力");
        damageEvent.Process(InManager);   
    }
}
