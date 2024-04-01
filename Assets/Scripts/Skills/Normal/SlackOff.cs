using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlackOff : StatusSkill
{
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        HealEvent healEvent = new HealEvent(SourcePokemon, SourcePokemon.GetMaxHP() / 2, "休息");
        healEvent.Process(InManager);
    }

    public override bool HasHealEffect(BattleManager InManager)
    {
        return true;
    }
}
