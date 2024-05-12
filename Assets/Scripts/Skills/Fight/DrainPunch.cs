using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DrainPunch : DamageSkill
{
    public override void AfterDamageEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, int Damage)
    {
        int selfHeal = Math.Min(SourcePokemon.GetMaxHP() - SourcePokemon.GetHP(), (int)(Damage * 0.5));
        HealEvent healEvent = new HealEvent(SourcePokemon, selfHeal, "吸取拳");
        healEvent.Process(InManager);   
    }
}
