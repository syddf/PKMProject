using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WillOWisp : StatusSkill
{
    public override bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        return TargetPokemon.HasType(EType.Fire) == false;
    }
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        SetPokemonStatusChangeEvent setStatChangeEvent = new SetPokemonStatusChangeEvent(TargetPokemon, InManager, EStatusChange.Burn, 1, false);
        setStatChangeEvent.Process(InManager);
    }
}
