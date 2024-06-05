using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FutureSight : StatusSkill
{
    public override bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, out string Reason)
    {
        Reason = "对方场上已经存在未来攻击状态了！";
        return InManager.HasBattleFieldStatus(!TargetPokemon.GetIsEnemy(), EBattleFieldStatus.FutureAttack) == false;
    }
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        SetBattleFieldStatusChangeEvent NewEvent = new SetBattleFieldStatusChangeEvent(SourcePokemon, InManager, EBattleFieldStatus.FutureAttack, 2, true, !TargetPokemon.GetIsEnemy());
        NewEvent.Process(InManager);
    }
}
