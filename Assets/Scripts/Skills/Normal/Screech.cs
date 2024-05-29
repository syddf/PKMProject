using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screech : StatusSkill
{
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        StatChangeEvent NewEvent = new StatChangeEvent(TargetPokemon, SourcePokemon, "Def", -2);
        NewEvent.Process(InManager);
    }
}
