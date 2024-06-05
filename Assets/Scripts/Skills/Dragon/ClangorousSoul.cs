using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClangorousSoul : StatusSkill
{    
    public override bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, out string Reason)
    {
        Reason = "HP过低无法使用魂舞烈音爆!";
        int selfDamage = Math.Max(SourcePokemon.GetMaxHP() / 3, 1);
        return SourcePokemon.GetHP() > selfDamage;
    }

    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        int selfDamage = Math.Max(SourcePokemon.GetMaxHP() / 3, 1);
        DamageEvent damageEvent = new DamageEvent(SourcePokemon, selfDamage, "魂舞烈音爆");
        damageEvent.Process(InManager);   

        List<string> ChangeStatList = new List<string>(){ "Atk", "Def", "SAtk", "SDef", "Speed" };
        List<int> ChangeLevel = new List<int>(){ 1, 1, 1, 1, 1 }; 
        StatChangeEvent stat1ChangeEvent = new StatChangeEvent(SourcePokemon, SourcePokemon, ChangeStatList, ChangeLevel);
        stat1ChangeEvent.Process(InManager);
    }
}
