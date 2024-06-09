using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastRespects : DamageSkill
{
    protected override int GetSkillPower(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        int DeadCount = InManager.GetTeammatesDeadCount(SourcePokemon);
        return Power + 50 * DeadCount;
    }
}