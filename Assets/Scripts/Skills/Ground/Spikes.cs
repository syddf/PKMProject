using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : StatusSkill
{
    public override bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, out string Reason)
    {
        Reason = "";
        return InManager.HasBattleFieldStatus(!TargetPokemon.GetIsEnemy(), EBattleFieldStatus.Spikes3) == false;
    }
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        if(InManager.HasBattleFieldStatus(!TargetPokemon.GetIsEnemy(), EBattleFieldStatus.Spikes1))
        {
            RemoveBattleFieldStatusChangeEvent ClearEvent = new RemoveBattleFieldStatusChangeEvent(InManager, EBattleFieldStatus.Spikes1, "", !TargetPokemon.GetIsEnemy(), false);
            ClearEvent.Process(InManager);
            SetBattleFieldStatusChangeEvent NewEvent = new SetBattleFieldStatusChangeEvent(SourcePokemon, InManager, EBattleFieldStatus.Spikes2, 1, false, !TargetPokemon.GetIsEnemy());
            NewEvent.Process(InManager);
        }
        else if(InManager.HasBattleFieldStatus(!TargetPokemon.GetIsEnemy(), EBattleFieldStatus.Spikes2))
        {            
            RemoveBattleFieldStatusChangeEvent ClearEvent = new RemoveBattleFieldStatusChangeEvent(InManager, EBattleFieldStatus.Spikes2, "", !TargetPokemon.GetIsEnemy(), false);
            ClearEvent.Process(InManager);
            SetBattleFieldStatusChangeEvent NewEvent = new SetBattleFieldStatusChangeEvent(SourcePokemon, InManager, EBattleFieldStatus.Spikes3, 1, false, !TargetPokemon.GetIsEnemy());
            NewEvent.Process(InManager);
        }
        else
        {
            SetBattleFieldStatusChangeEvent NewEvent = new SetBattleFieldStatusChangeEvent(SourcePokemon, InManager, EBattleFieldStatus.Spikes1, 1, false, !TargetPokemon.GetIsEnemy());
            NewEvent.Process(InManager);
        }
    }
}
