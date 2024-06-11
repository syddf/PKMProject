using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthSap : StatusSkill
{
    public override bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, out string Reason)
    {
        Reason = "";
        return SourcePokemon.GetHP() < SourcePokemon.GetMaxHP() || TargetPokemon.GetAtkChangeLevel() > -6;
    }
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        if(SourcePokemon.GetHP() < SourcePokemon.GetMaxHP())
        {
            HealEvent healEvent = new HealEvent(SourcePokemon, TargetPokemon.GetAtk(ECaclStatsMode.Normal, InManager), "吸取力量");
            healEvent.Process(InManager);
        }

        StatChangeEvent NewEvent = new StatChangeEvent(TargetPokemon, SourcePokemon, "Atk", -1);
        NewEvent.Process(InManager);
    }

    public override bool HasHealEffect(BattleManager InManager)
    {
        return true;
    }
}
