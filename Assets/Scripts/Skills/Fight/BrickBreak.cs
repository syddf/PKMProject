using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBreak : DamageSkill
{
    public override void AfterSkillEffectEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        if(InManager.HasBattleFieldStatus(!TargetPokemon.GetIsEnemy(), EBattleFieldStatus.LightScreenStatus))
        {
            RemoveBattleFieldStatusChangeEvent NewEvent = new RemoveBattleFieldStatusChangeEvent(InManager, EBattleFieldStatus.LightScreenStatus, "", !TargetPokemon.GetIsEnemy());
            NewEvent.Process(InManager);
        }
        if(InManager.HasBattleFieldStatus(!TargetPokemon.GetIsEnemy(), EBattleFieldStatus.ReflectStatus))
        {
            RemoveBattleFieldStatusChangeEvent NewEvent = new RemoveBattleFieldStatusChangeEvent(InManager, EBattleFieldStatus.ReflectStatus, "", !TargetPokemon.GetIsEnemy());
            NewEvent.Process(InManager);
        }
        if(InManager.HasBattleFieldStatus(!TargetPokemon.GetIsEnemy(), EBattleFieldStatus.AuroraVeilStatus))
        {
            RemoveBattleFieldStatusChangeEvent NewEvent = new RemoveBattleFieldStatusChangeEvent(InManager, EBattleFieldStatus.AuroraVeilStatus, "", !TargetPokemon.GetIsEnemy());
            NewEvent.Process(InManager);
        }
    }
}