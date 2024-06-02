using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartingShot : StatusSkill
{
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        List<string> ChangeStatList = new List<string>(){ "Atk", "SAtk" };
        List<int> ChangeLevel = new List<int>(){ -1, -1 }; 
        StatChangeEvent stat1ChangeEvent = new StatChangeEvent(TargetPokemon, SourcePokemon, ChangeStatList, ChangeLevel);
        stat1ChangeEvent.Process(InManager);

        if(SourcePokemon.GetIsEnemy())
        {
            if(InManager.GetEnemyTrainer().GetRemainPokemonNum() > 1)
            {
                SwitchAfterSkillUseEvent switchEvent = new SwitchAfterSkillUseEvent(InManager, SourcePokemon);
                switchEvent.Process(InManager);
            }
        }
        else
        {
            if(InManager.GetPlayerTrainer().GetRemainPokemonNum() > 1)
            {
                SwitchAfterSkillUseEvent switchEvent = new SwitchAfterSkillUseEvent(InManager, SourcePokemon);
                switchEvent.Process(InManager);
            }
        }     
    }
}