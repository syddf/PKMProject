using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FlareBlitz : DamageSkill
{
    public override void AfterDamageEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, int Damage)
    {        
        if(SourcePokemon.HasAbility("坚硬脑袋", InManager, SourcePokemon, TargetPokemon) || SourcePokemon.HasAbility("魔法防守", InManager, SourcePokemon, TargetPokemon))
        {
            return;
        }
        int selfDamage = Math.Min(SourcePokemon.GetHP(), Math.Max(1, Damage / 3));
        DamageEvent damageEvent = new DamageEvent(SourcePokemon, selfDamage, "反作用力");
        damageEvent.Process(InManager);   
    }
}
