using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickRoom : StatusSkill
{
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        bool Active = true;
        if(InManager.GetIsTrickRoomActive())
        {
            Active = false;
        }
        TrickRoomChangeEvent trickRoomEvent = new TrickRoomChangeEvent(SourcePokemon, InManager, Active, 5);
        trickRoomEvent.Process(InManager);
    }
}
