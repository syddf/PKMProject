using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparklingAria : DamageSkill
{
    public override void AfterSkillEffectEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        if(TargetPokemon.HasStatusChange(EStatusChange.Burn))
        {
            RemovePokemonStatusChangeEvent RemoveEvent = 
            new RemovePokemonStatusChangeEvent(TargetPokemon, InManager, EStatusChange.Burn, "泡影的咏叹调的效果");
            RemoveEvent.Process(InManager);
        }
    }
}

