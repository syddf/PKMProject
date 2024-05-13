using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysticalFire : DamageSkill
{
    public override void AfterSkillEffectEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        StatChangeEvent statChangeEvent = new StatChangeEvent(TargetPokemon, SourcePokemon, "SAtk", -1);
        statChangeEvent.Process(InManager);
    }
}
