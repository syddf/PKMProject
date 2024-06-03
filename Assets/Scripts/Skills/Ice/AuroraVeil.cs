using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuroraVeil : StatusSkill
{
    public override bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, out string Reason)
    {
        Reason = "";
        return InManager.HasBattleFieldStatus(!SourcePokemon.GetIsEnemy(), EBattleFieldStatus.AuroraVeilStatus) == false &&
        InManager.GetWeatherType() == EWeather.Snow;;
    }
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        int Turn = 5;
        if(SourcePokemon.HasItem("光之黏土"))
        {
            Turn = 8;
        }
        SetBattleFieldStatusChangeEvent NewEvent = new SetBattleFieldStatusChangeEvent(SourcePokemon, InManager, EBattleFieldStatus.AuroraVeilStatus, Turn, true, !SourcePokemon.GetIsEnemy());
        NewEvent.Process(InManager);
    }
}
