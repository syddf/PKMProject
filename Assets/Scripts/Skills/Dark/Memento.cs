using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Memento : StatusSkill
{
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        List<string> ChangeStatList = new List<string>(){ "Atk", "SAtk" };
        List<int> ChangeLevel = new List<int>(){ -2, -2 }; 
        StatChangeEvent stat1ChangeEvent = new StatChangeEvent(TargetPokemon, SourcePokemon, ChangeStatList, ChangeLevel);
        stat1ChangeEvent.Process(InManager);

        int selfDamage = Math.Min(SourcePokemon.GetHP(), Math.Max(1, SourcePokemon.GetHP()));
        DamageEvent damageEvent = new DamageEvent(SourcePokemon, selfDamage, "临别礼物的效果");
        damageEvent.Process(InManager);   
    }
}
