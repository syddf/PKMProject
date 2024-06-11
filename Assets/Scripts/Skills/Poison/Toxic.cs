using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toxic : StatusSkill
{
    public override bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, out string Reason)
    {
        Reason = "";
        return SetPokemonStatusChangeEvent.IsStatusChangeEffective(InManager, TargetPokemon, SourcePokemon, EStatusChange.Poison);
    }
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        SetPokemonStatusChangeEvent setStatChangeEvent = new SetPokemonStatusChangeEvent(TargetPokemon, SourcePokemon, InManager, EStatusChange.Poison, 1, false);
        setStatChangeEvent.Process(InManager);
    }
}
