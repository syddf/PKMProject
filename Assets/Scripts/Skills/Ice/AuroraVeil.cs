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
        SetBattleFieldStatusChangeEvent NewEvent = new SetBattleFieldStatusChangeEvent(SourcePokemon, InManager, EBattleFieldStatus.AuroraVeilStatus, 5, true, !SourcePokemon.GetIsEnemy());
        NewEvent.Process(InManager);
    }
}
