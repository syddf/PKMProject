using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LovelyKiss : StatusSkill
{
    public override bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, out string Reason)
    {
        Reason = "";
        return SetPokemonStatusChangeEvent.IsStatusChangeEffective(InManager, TargetPokemon, SourcePokemon, EStatusChange.Drowsy);
    }
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        SetPokemonStatusChangeEvent setStatChangeEvent = new SetPokemonStatusChangeEvent(TargetPokemon, SourcePokemon, InManager, EStatusChange.Drowsy, 1, false);
        setStatChangeEvent.Process(InManager);
    }
}
