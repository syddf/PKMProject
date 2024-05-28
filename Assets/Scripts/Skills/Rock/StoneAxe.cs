using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneAxe : DamageSkill
{
    public override void AfterSkillEffectEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        SetBattleFieldStatusChangeEvent NewEvent = new SetBattleFieldStatusChangeEvent(SourcePokemon, InManager, EBattleFieldStatus.StealthRock, 1, false, !TargetPokemon.GetIsEnemy());
        NewEvent.Process(InManager);
    }
}
