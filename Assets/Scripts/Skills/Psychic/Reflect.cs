using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflect : StatusSkill
{
    public override bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, out string Reason)
    {
        Reason = "";
        return InManager.HasBattleFieldStatus(!SourcePokemon.GetIsEnemy(), EBattleFieldStatus.ReflectStatus) == false;
    }
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        SetBattleFieldStatusChangeEvent NewEvent = new SetBattleFieldStatusChangeEvent(InManager, EBattleFieldStatus.ReflectStatus, 5, true, !SourcePokemon.GetIsEnemy());
        NewEvent.Process(InManager);
    }
}
