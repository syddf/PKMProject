using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurricane : DamageSkill
{
    public override int GetAccuracy(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        if(InManager.GetWeatherType() == EWeather.SunLight)
        {
            return 50;
        }
        return Accuracy;
    }

    public override bool GetAlwaysHit(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        if(InManager.GetWeatherType() == EWeather.Rain)
        {
            return true;
        }
        return false;
    }
}
