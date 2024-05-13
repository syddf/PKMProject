using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoulPlay : DamageSkill
{
    public virtual int GetSourceAtk(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, bool CT)
    {
        ECaclStatsMode Mode = GetSourceAtkCaclMode(InManager, SourcePokemon, TargetPokemon, CT);
        return TargetPokemon.GetAtk(Mode, InManager);
    }

}
