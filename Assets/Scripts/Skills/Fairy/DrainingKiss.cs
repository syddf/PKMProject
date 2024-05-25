using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DrainingKiss : DamageSkill
{
    public override void AfterDamageEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, int Damage)
    {
        int selfHeal = Math.Min(SourcePokemon.GetMaxHP() - SourcePokemon.GetHP(), (int)(Damage * 0.75));
        HealEvent healEvent = new HealEvent(SourcePokemon, selfHeal, "吸取之吻");
        healEvent.Process(InManager);   
    }

    public override bool HasHealEffect(BattleManager InManager)
    {
        return true;
    }
}
