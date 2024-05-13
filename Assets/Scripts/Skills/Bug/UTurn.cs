using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UTurn : DamageSkill
{
    public override void AfterSkillEffectEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
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
