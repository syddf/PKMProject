using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FutureSight : StatusSkill
{
    public override bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, out string Reason)
    {
        Reason = "对方场上已经存在未来攻击状态了！";
        if(SourcePokemon.GetReferenceTrainer().TrainerSkill.GetSkillName() == "预言家")
        {
            return InManager.HasBattleFieldStatus(!TargetPokemon.GetIsEnemy(), EBattleFieldStatus.FutureAttackEnhanced) == false;
        }
        return InManager.HasBattleFieldStatus(!TargetPokemon.GetIsEnemy(), EBattleFieldStatus.FutureAttack) == false;
    }
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        if(SourcePokemon.GetReferenceTrainer().TrainerSkill.GetSkillName() == "预言家")
        {
            SetBattleFieldStatusChangeEvent NewEvent = new SetBattleFieldStatusChangeEvent(SourcePokemon, InManager, EBattleFieldStatus.FutureAttackEnhanced, 3, true, !TargetPokemon.GetIsEnemy());
            NewEvent.Process(InManager);
        }
        else
        {
            SetBattleFieldStatusChangeEvent NewEvent = new SetBattleFieldStatusChangeEvent(SourcePokemon, InManager, EBattleFieldStatus.FutureAttack, 3, true, !TargetPokemon.GetIsEnemy());
            NewEvent.Process(InManager);
        }
    }
}
