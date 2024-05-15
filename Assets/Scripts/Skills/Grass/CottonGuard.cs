using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CottonGuard : StatusSkill
{
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        StatChangeEvent NewEvent = new StatChangeEvent(SourcePokemon, SourcePokemon, "Def", 3);
        NewEvent.Process(InManager);
    }
}
