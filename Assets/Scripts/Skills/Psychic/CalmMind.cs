using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalmMind : StatusSkill
{
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        List<string> ChangeStatList = new List<string>(){ "SAtk", "SDef" };
        List<int> ChangeLevel = new List<int>(){ 1, 1 }; 
        StatChangeEvent stat1ChangeEvent = new StatChangeEvent(SourcePokemon, SourcePokemon, ChangeStatList, ChangeLevel);
        stat1ChangeEvent.Process(InManager);
    }
}
