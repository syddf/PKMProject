using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taunt : StatusSkill
{
    public override bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, out string Reason)
    {
        Reason = "";
        return SetPokemonStatusChangeEvent.IsStatusChangeEffective(InManager, TargetPokemon, SourcePokemon, EStatusChange.Taunt);
    }
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        SetPokemonStatusChangeEvent setStatChangeEvent = new SetPokemonStatusChangeEvent(TargetPokemon, SourcePokemon, InManager, EStatusChange.Taunt, TargetPokemon.GetActivated() ? 4 : 3, true);
        setStatChangeEvent.Process(InManager);
    }
}
