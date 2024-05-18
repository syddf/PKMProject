using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roost : StatusSkill
{
    public override bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, out string Reason)
    {
        Reason = "";
        return SourcePokemon.GetHP() < SourcePokemon.GetMaxHP();
    }
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        HealEvent healEvent = new HealEvent(SourcePokemon, SourcePokemon.GetMaxHP() / 2, "羽栖");
        healEvent.Process(InManager);

        if(SourcePokemon.HasType(EType.Flying, null, null, null))
        {
            SetPokemonStatusChangeEvent setStatChangeEvent = new SetPokemonStatusChangeEvent(SourcePokemon, SourcePokemon, InManager, EStatusChange.Roost, 1, true);
            setStatChangeEvent.Process(InManager);
        }
    }

    public override bool HasHealEffect(BattleManager InManager)
    {
        return true;
    }
}
