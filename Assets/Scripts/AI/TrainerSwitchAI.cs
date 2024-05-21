using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerSwitchAI : MonoBehaviour
{
    public virtual BattlePokemon GetNextPokemon(BattlePokemon OutPokemon, BattleManager ReferenceManager, PokemonTrainer ReferenceTrainer)
    {
        BattlePokemon[] BattlePokemons = ReferenceTrainer.BattlePokemons;
        System.Random rnd = new System.Random();
        int RandNum = rnd.Next(0, 5);
        int FailedNum = 0;
        while(true)
        {
            if(BattlePokemons[RandNum] != null && BattlePokemons[RandNum] != OutPokemon && BattlePokemons[RandNum].IsDead() == false)
            {
                return BattlePokemons[RandNum];
            }
            RandNum = (RandNum + 1) % 5;
            FailedNum++;
            if(FailedNum >= 5)
            {
                break;
            }
        }
        return BattlePokemons[5];
    }
}
