using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordsDance : StatusSkill
{
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        StatChangeEvent NewEvent = new StatChangeEvent(SourcePokemon, "Atk", 2);
        NewEvent.Process(InManager);
    }
}
