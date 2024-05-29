using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerArm : DamageSkill
{
    public override void AfterSkillEffectEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        List<string> ChangeStatList = new List<string>(){ "Speed" };
        List<int> ChangeLevel = new List<int>(){ -1}; 
        StatChangeEvent stat1ChangeEvent = new StatChangeEvent(SourcePokemon, SourcePokemon, ChangeStatList, ChangeLevel);
        stat1ChangeEvent.Process(InManager);
    }
}

