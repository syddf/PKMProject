using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFang : DamageSkill
{
    public override void AfterSkillEffectEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {        
        System.Random rnd = new System.Random();
        int Random = rnd.Next(0, 2);
        if(Random == 0)
        {
            SetPokemonStatusChangeEvent setStatChangeEvent = new SetPokemonStatusChangeEvent(TargetPokemon, SourcePokemon, InManager, EStatusChange.Flinch, 1, true);
            setStatChangeEvent.Process(InManager);
        }
        else
        {
            SetPokemonStatusChangeEvent setStatChangeEvent = new SetPokemonStatusChangeEvent(TargetPokemon, SourcePokemon, InManager, EStatusChange.Burn, 1, false);
            setStatChangeEvent.Process(InManager);
        }
    }

    public override int AfterSkillEffectEventProbablity(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        return 10;
    }

    public override bool IsAfterSkillEffectToTargetPokemon()
    {
        return true;
    }
}