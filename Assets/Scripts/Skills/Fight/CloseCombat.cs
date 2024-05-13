using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseCombat : DamageSkill
{
    public override void AfterSkillEffectEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        StatChangeEvent stat1ChangeEvent = new StatChangeEvent(SourcePokemon, SourcePokemon, "Def", -1);
        StatChangeEvent stat2ChangeEvent = new StatChangeEvent(SourcePokemon, SourcePokemon, "SDef", -1);
        stat1ChangeEvent.Process(InManager);
        stat2ChangeEvent.Process(InManager);
    }
}
