using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulkUp : StatusSkill
{
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        StatChangeEvent NewEvent1 = new StatChangeEvent(SourcePokemon, "Atk", 1);
        StatChangeEvent NewEvent2 = new StatChangeEvent(SourcePokemon, "Def", 1);
        NewEvent1.Process(InManager);
        NewEvent2.Process(InManager);
    }
}
