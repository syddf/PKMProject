using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainDance : StatusSkill
{
    public override bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, out string Reason)
    {
        Reason = "";
        return InManager.GetWeatherType() != EWeather.Rain;
    }
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        WeatherChangeEvent weatherEvent = new WeatherChangeEvent(SourcePokemon, InManager, EWeather.Rain);
        weatherEvent.Process(InManager);
    }
}
