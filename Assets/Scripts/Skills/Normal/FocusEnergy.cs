using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusEnergy : StatusSkill
{
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        StatChangeEvent NewEvent = new StatChangeEvent(SourcePokemon, SourcePokemon, "CT", 2);
        NewEvent.Process(InManager);
    }
}
