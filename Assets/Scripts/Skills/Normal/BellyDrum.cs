using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellyDrum : StatusSkill
{
    public override bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, out string Reason)
    {
        Reason = "";
        return SourcePokemon.GetHP() > (SourcePokemon.GetMaxHP() / 2);
    }
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        int Damage = SourcePokemon.GetMaxHP() / 2;
        DamageEvent damageEvent = new DamageEvent(SourcePokemon, Damage, "腹鼓");
        damageEvent.Process(InManager);

        StatChangeEvent NewEvent = new StatChangeEvent(SourcePokemon, SourcePokemon, "Atk", 12);
        NewEvent.Process(InManager);
    }
}