using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blizzard : DamageSkill
{
    public override void AfterSkillEffectEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        SetPokemonStatusChangeEvent setStatChangeEvent = new SetPokemonStatusChangeEvent(TargetPokemon, SourcePokemon, InManager, EStatusChange.Frostbite, 1, false);
        setStatChangeEvent.Process(InManager);
    }

    public override int AfterSkillEffectEventProbablity(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        return 10;
    }

    public override bool IsAfterSkillEffectToTargetPokemon()
    {
        return true;
    }
    public override bool GetAlwaysHit(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        if(InManager.GetWeatherType() == EWeather.Snow)
        {
            return true;
        }
        return false;
    }
}
