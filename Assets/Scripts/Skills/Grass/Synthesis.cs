using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Synthesis : StatusSkill
{
    public override bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, out string Reason)
    {
        Reason = "";
        return SourcePokemon.GetHP() < SourcePokemon.GetMaxHP();
    }
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        int HealHp =  SourcePokemon.GetMaxHP() / 2;
        if(InManager.GetWeatherType() == EWeather.SunLight)
        {
            HealHp = SourcePokemon.GetMaxHP() * 2 / 3;
        }
        else if(InManager.GetWeatherType() == EWeather.None)
        {
            HealHp = SourcePokemon.GetMaxHP() / 2;
        }
        else
        {
            HealHp = SourcePokemon.GetMaxHP() / 4;
        }

        HealEvent healEvent = new HealEvent(SourcePokemon, HealHp, "光合作用");
        healEvent.Process(InManager);
    }

    public override bool HasHealEffect(BattleManager InManager)
    {
        return true;
    }
}
