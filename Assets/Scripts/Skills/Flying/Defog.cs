using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defog : StatusSkill
{
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        StatChangeEvent NewEvent = new StatChangeEvent(TargetPokemon, SourcePokemon, "Evasionrate", -1);
        NewEvent.Process(InManager);

        if(InManager.HasBattleFieldStatus(!TargetPokemon.GetIsEnemy(), EBattleFieldStatus.LightScreenStatus))
        {
            RemoveBattleFieldStatusChangeEvent ClearEvent = new RemoveBattleFieldStatusChangeEvent(InManager, EBattleFieldStatus.LightScreenStatus, "", !TargetPokemon.GetIsEnemy());
            ClearEvent.Process(InManager);
        }
        if(InManager.HasBattleFieldStatus(!TargetPokemon.GetIsEnemy(), EBattleFieldStatus.ReflectStatus))
        {
            RemoveBattleFieldStatusChangeEvent ClearEvent = new RemoveBattleFieldStatusChangeEvent(InManager, EBattleFieldStatus.ReflectStatus, "", !TargetPokemon.GetIsEnemy());
            ClearEvent.Process(InManager);
        }
    }
}
