using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonTrainer : MonoBehaviour
{
    public string TrainerName;
    public BattlePokemon[] BattlePokemons = new BattlePokemon[6];
    public BagPokemon[] BagPokemons = new BagPokemon[6];
    public bool IsPlayer;
    public BaseTrainerSkill TrainerSkill;
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
