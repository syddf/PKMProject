using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonTrainer : MonoBehaviour
{
    public BattlePokemon[] BattlePokemons = new BattlePokemon[6];

    public int GetRemainPokemonNum()
    {
        int Result = 0;
        for(int Index = 0; Index < 6; Index++)
        {
            if(BattlePokemons[Index] && !BattlePokemons[Index].IsDead())
            {
                Result++;
            }
        }
        return Result;
    }
}
