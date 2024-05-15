using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonCheer : StatusSkill
{
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        if(SourcePokemon.GetType1() == EType.Dragon || SourcePokemon.GetType2() == EType.Dragon)
        {
            StatChangeEvent NewEvent = new StatChangeEvent(SourcePokemon, SourcePokemon, "CT", 2);
            NewEvent.Process(InManager);
        }
        else
        {
            StatChangeEvent NewEvent = new StatChangeEvent(SourcePokemon, SourcePokemon, "CT", 1);
            NewEvent.Process(InManager);
        }
    }
}
