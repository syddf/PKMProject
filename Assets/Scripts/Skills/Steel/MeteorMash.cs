using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorMash : DamageSkill
{
    public override void AfterSkillEffectEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        StatChangeEvent statChangeEvent = new StatChangeEvent(SourcePokemon, SourcePokemon, "Atk", 1);
        statChangeEvent.Process(InManager);
    }

    
    public override int AfterSkillEffectEventProbablity(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        return 20;
    }
}
