using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wish : StatusSkill
{
    public override bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, out string Reason)
    {
        Reason = "";
        return InManager.HasBattleFieldStatus(!SourcePokemon.GetIsEnemy(), EBattleFieldStatus.Wish) == false;
    }
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        SetBattleFieldStatusChangeEvent NewEvent = new SetBattleFieldStatusChangeEvent(SourcePokemon, InManager, EBattleFieldStatus.Wish, 2, true, !SourcePokemon.GetIsEnemy());
        NewEvent.Process(InManager);
    }
}
