using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseCombat : DamageSkill
{
    public override void AfterSkillEffectEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        List<string> ChangeStatList = new List<string>(){ "Def", "SDef" };
        List<int> ChangeLevel = new List<int>(){ -1, -1 }; 
        StatChangeEvent stat1ChangeEvent = new StatChangeEvent(SourcePokemon, SourcePokemon, ChangeStatList, ChangeLevel);
        stat1ChangeEvent.Process(InManager);
    }
}
