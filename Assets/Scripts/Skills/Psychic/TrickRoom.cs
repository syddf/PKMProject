using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickRoom : StatusSkill
{    
    public override bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, out string Reason)
    {
        Reason = "";
        if(InManager.HasSpecialRule("特殊规则(玛绣)") && InManager.GetIsTrickRoomActive())
        {
            return false;
        }
        return true;
    }
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
