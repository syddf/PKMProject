using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScreen : StatusSkill
{
    public override bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        return InManager.HasBattleFieldStatus(!SourcePokemon.GetIsEnemy(), EBattleFieldStatus.LightScreenStatus) == false;
    }
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        SetBattleFieldStatusChangeEvent NewEvent = new SetBattleFieldStatusChangeEvent(InManager, EBattleFieldStatus.LightScreenStatus, 5, true, !SourcePokemon.GetIsEnemy());
        NewEvent.Process(InManager);
    }
}
