using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryDance : StatusSkill
{
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        List<string> ChangeStatList = new List<string>(){ "Atk", "Def", "Speed" };
        List<int> ChangeLevel = new List<int>(){ 1, 1, 1 }; 
        StatChangeEvent stat1ChangeEvent = new StatChangeEvent(SourcePokemon, SourcePokemon, ChangeStatList, ChangeLevel);
        stat1ChangeEvent.Process(InManager);
    }
}
