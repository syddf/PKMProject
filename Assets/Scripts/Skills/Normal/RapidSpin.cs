using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidSpin : DamageSkill
{
    private void ClearAllFieldStatus(BattlePokemon ReferencePokemon, BattleManager InManager)
    {
        List<EBattleFieldStatus> StatusToClear = new List<EBattleFieldStatus>()
        { EBattleFieldStatus.LightScreenStatus, EBattleFieldStatus.ReflectStatus, EBattleFieldStatus.AuroraVeilStatus, EBattleFieldStatus.StickyWeb, EBattleFieldStatus.StealthRock, 
         EBattleFieldStatus.Spikes1, EBattleFieldStatus.Spikes2, EBattleFieldStatus.Spikes3};
        foreach(var Status in StatusToClear)
        {
            RemoveBattleFieldStatusChangeEvent ClearEvent = new RemoveBattleFieldStatusChangeEvent(InManager, Status, "", !ReferencePokemon.GetIsEnemy());
            ClearEvent.Process(InManager);
        }
    }
    public override void AfterSkillEffectEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        StatChangeEvent NewEvent = new StatChangeEvent(SourcePokemon, SourcePokemon, "Speed", 1);
        NewEvent.Process(InManager);
        ClearAllFieldStatus(SourcePokemon, InManager);
        if(SourcePokemon.HasStatusChange(EStatusChange.LeechSeed))
        {
            RemovePokemonStatusChangeEvent NewEvent2 = new RemovePokemonStatusChangeEvent(SourcePokemon, InManager, EStatusChange.LeechSeed, "高速旋转");
            NewEvent2.Process(InManager);
        }
    }
}
