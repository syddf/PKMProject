using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceSwitchAI : TrainerSwitchAI
{
    public override BattlePokemon GetNextPokemon(BattlePokemon OutPokemon, BattleManager ReferenceManager, PokemonTrainer ReferenceTrainer)
    {
        BattlePokemon[] BattlePokemons = ReferenceTrainer.BattlePokemons;
        int Index = 0;
        for(; Index < 6; Index++)
        {
            if(BattlePokemons[Index] == OutPokemon)
            {
                break;
            }
        }
        Index = (Index + 1) % 5;
        int FailedNum = 0;
        while(true)
        {
            if(BattlePokemons[Index] != null && BattlePokemons[Index] != OutPokemon && BattlePokemons[Index].IsDead() == false)
            {
                return BattlePokemons[Index];
            }
            Index = (Index + 1) % 5;
            FailedNum++;
            if(FailedNum >= 5)
            {
                break;
            }
        }
        return BattlePokemons[5];
    }
}
