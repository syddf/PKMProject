using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : DamageSkill
{
    public override int GetAccuracy(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        if(InManager.GetWeatherType() == EWeather.SunLight)
        {
            return 50;
        }
        return Accuracy;
    }

    public override void AfterSkillEffectEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        SetPokemonStatusChangeEvent setStatChangeEvent = new SetPokemonStatusChangeEvent(TargetPokemon, SourcePokemon, InManager, EStatusChange.Paralysis, 1, false);
        setStatChangeEvent.Process(InManager);
    }

    
    public override int AfterSkillEffectEventProbablity(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        return 30;
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
