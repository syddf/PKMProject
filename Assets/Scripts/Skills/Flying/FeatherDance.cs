using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatherDance : StatusSkill
{
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        StatChangeEvent NewEvent = new StatChangeEvent(TargetPokemon, "Atk", -2);
        NewEvent.Process(InManager);
    }
}
