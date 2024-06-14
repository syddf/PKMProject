using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinyBond : StatusSkill
{
    public override bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, out string Reason)
    {
        Reason = "";
        return SetPokemonStatusChangeEvent.IsStatusChangeEffective(InManager, SourcePokemon, SourcePokemon, EStatusChange.DestinyBond);
    }
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        SetPokemonStatusChangeEvent setStatChangeEvent = new SetPokemonStatusChangeEvent(SourcePokemon, SourcePokemon, InManager, EStatusChange.DestinyBond, 1, true);
        setStatChangeEvent.Process(InManager);
    }
}
