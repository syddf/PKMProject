using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpicyExtract : StatusSkill
{
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        StatChangeEvent NewEvent = new StatChangeEvent(TargetPokemon, SourcePokemon, "Atk", 2);
        NewEvent.Process(InManager);

        StatChangeEvent NewEvent2 = new StatChangeEvent(TargetPokemon, SourcePokemon, "Def", -2);
        NewEvent2.Process(InManager);
    }
}
