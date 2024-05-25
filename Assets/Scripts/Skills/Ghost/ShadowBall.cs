using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBall : DamageSkill
{
    public override void AfterSkillEffectEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        StatChangeEvent statChangeEvent = new StatChangeEvent(TargetPokemon, SourcePokemon, "SDef", -1);
        statChangeEvent.Process(InManager);
    }

    
    public override int AfterSkillEffectEventProbablity(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        return 20;
    }

    public override bool IsAfterSkillEffectToTargetPokemon()
    {
        return true;
    }
}
