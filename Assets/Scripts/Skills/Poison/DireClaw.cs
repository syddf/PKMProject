using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DireClaw : DamageSkill
{
    public override void AfterSkillEffectEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        System.Random rnd = new System.Random();
        int Random = rnd.Next(0, 3);
        EStatusChange NewStatus = EStatusChange.Poison;
        if(Random == 1)
            NewStatus = EStatusChange.Paralysis;
        if(Random == 2)
            NewStatus = EStatusChange.Drowsy;
        SetPokemonStatusChangeEvent setStatChangeEvent = new SetPokemonStatusChangeEvent(TargetPokemon, SourcePokemon, InManager, NewStatus, 1, false);
        setStatChangeEvent.Process(InManager);
    }

    public override int AfterSkillEffectEventProbablity(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        return 50;
    }
}
