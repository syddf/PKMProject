using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amnesia : StatusSkill
{
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        StatChangeEvent NewEvent = new StatChangeEvent(SourcePokemon, SourcePokemon, "SDef", 2);
        NewEvent.Process(InManager);
    }
}
