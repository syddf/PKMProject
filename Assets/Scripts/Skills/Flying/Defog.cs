using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defog : StatusSkill
{
    private void ClearAllFieldStatus(BattlePokemon ReferencePokemon, BattleManager InManager)
    {
        List<EBattleFieldStatus> StatusToClear = new List<EBattleFieldStatus>()
        { EBattleFieldStatus.LightScreenStatus, EBattleFieldStatus.ReflectStatus, EBattleFieldStatus.StickyWeb, EBattleFieldStatus.StealthRock, 
         EBattleFieldStatus.Spikes1, EBattleFieldStatus.Spikes2, EBattleFieldStatus.Spikes3};
        foreach(var Status in StatusToClear)
        {
            RemoveBattleFieldStatusChangeEvent ClearEvent = new RemoveBattleFieldStatusChangeEvent(InManager, Status, "", !ReferencePokemon.GetIsEnemy());
            ClearEvent.Process(InManager);
        }
    }
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        StatChangeEvent NewEvent = new StatChangeEvent(TargetPokemon, SourcePokemon, "Evasionrate", -1);
        NewEvent.Process(InManager);
        ClearAllFieldStatus(SourcePokemon, InManager);
        ClearAllFieldStatus(TargetPokemon, InManager);
    }
}
