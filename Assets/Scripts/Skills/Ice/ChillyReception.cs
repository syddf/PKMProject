using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChillyReception : StatusSkill
{    
    public override bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, out string Reason)
    {
        Reason = "";
        bool CanSwitch = false;
        if(SourcePokemon.GetIsEnemy())
        {
            if(InManager.GetEnemyTrainer().GetRemainPokemonNum() > 1)
            {
                CanSwitch = true;
            }
        }
        else
        {
            if(InManager.GetPlayerTrainer().GetRemainPokemonNum() > 1)
            {
                CanSwitch = true;
            }
        }

        return InManager.GetWeatherType() != EWeather.Snow || CanSwitch;
    }
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        if(InManager.GetWeatherType() != EWeather.Snow)
        {
            WeatherChangeEvent weatherEvent = new WeatherChangeEvent(SourcePokemon, InManager, EWeather.Snow);
            weatherEvent.Process(InManager);
        }

        if(SourcePokemon.GetIsEnemy())
        {
            if(InManager.GetEnemyTrainer().GetRemainPokemonNum() > 1)
            {
                SwitchAfterSkillUseEvent switchEvent = new SwitchAfterSkillUseEvent(InManager, SourcePokemon);
                switchEvent.Process(InManager);
            }
        }
        else
        {
            if(InManager.GetPlayerTrainer().GetRemainPokemonNum() > 1)
            {
                SwitchAfterSkillUseEvent switchEvent = new SwitchAfterSkillUseEvent(InManager, SourcePokemon);
                switchEvent.Process(InManager);
            }
        }     
    }
}