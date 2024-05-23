using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthRock : StatusSkill
{
    public override bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, out string Reason)
    {
        Reason = "";
        return InManager.HasBattleFieldStatus(!TargetPokemon.GetIsEnemy(), EBattleFieldStatus.StealthRock) == false;
    }
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        SetBattleFieldStatusChangeEvent NewEvent = new SetBattleFieldStatusChangeEvent(SourcePokemon, InManager, EBattleFieldStatus.StealthRock, 1, false, !TargetPokemon.GetIsEnemy());
        NewEvent.Process(InManager);
    }
}
