using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatonPass : StatusSkill
{
    public override bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, out string Reason)
    {
        Reason = "";
        if(SourcePokemon.GetIsEnemy())
        {
            return InManager.GetEnemyTrainer().GetRemainPokemonNum() > 1;
        }
        return InManager.GetPlayerTrainer().GetRemainPokemonNum() > 1;
    }
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        if(SourcePokemon.GetIsEnemy())
        {
            SwitchAfterSkillUseEvent switchEvent = new SwitchAfterSkillUseEvent(InManager, SourcePokemon, true);
            switchEvent.Process(InManager);
        }
        else
        {
            SwitchAfterSkillUseEvent switchEvent = new SwitchAfterSkillUseEvent(InManager, SourcePokemon, true);
            switchEvent.Process(InManager);
        }     
    }
}