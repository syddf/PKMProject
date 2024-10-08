using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPress : DamageSkill
{
    public override int GetSourceAtk(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, bool CT)
    {
        ECaclStatsMode Mode = GetSourceAtkCaclMode(InManager, SourcePokemon, TargetPokemon, CT);
        return SourcePokemon.GetDef(Mode, InManager);
    }

}
