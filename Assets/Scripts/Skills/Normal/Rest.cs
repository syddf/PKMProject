using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rest : StatusSkill
{
    public override bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, out string Reason)
    {
        Reason = "";
        return SourcePokemon.GetHP() < SourcePokemon.GetMaxHP() &&
        SetPokemonStatusChangeEvent.IsStatusChangeEffective(InManager, SourcePokemon, SourcePokemon, EStatusChange.Drowsy);
    }
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        SetPokemonStatusChangeEvent setStatChangeEvent = new SetPokemonStatusChangeEvent(SourcePokemon, SourcePokemon, InManager, EStatusChange.Drowsy, 1, false);
        setStatChangeEvent.Process(InManager);

        HealEvent healEvent = new HealEvent(SourcePokemon, SourcePokemon.GetMaxHP(), "睡觉");
        healEvent.Process(InManager);
    }

    public override bool HasHealEffect(BattleManager InManager)
    {
        return true;
    }
}
