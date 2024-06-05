using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recover : StatusSkill
{
    public override bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, out string Reason)
    {
        Reason = "";
        return SourcePokemon.GetHP() < SourcePokemon.GetMaxHP();
    }
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        HealEvent healEvent = new HealEvent(SourcePokemon, SourcePokemon.GetMaxHP() / 2, "自我再生");
        healEvent.Process(InManager);
    }

    public override bool HasHealEffect(BattleManager InManager)
    {
        return true;
    }
}
