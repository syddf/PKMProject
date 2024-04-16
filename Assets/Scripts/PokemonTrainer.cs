using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonTrainer : MonoBehaviour
{
    public string TrainerName;
    public BattlePokemon[] BattlePokemons = new BattlePokemon[6];
    public BagPokemon[] BagPokemons = new BagPokemon[6];
    public List<string> LineWhenPokemon0FirstIn = new List<string>();
    public List<string> LineWhenPokemon1FirstIn = new List<string>();
    public List<string> LineWhenPokemon2FirstIn = new List<string>();
    public List<string> LineWhenPokemon3FirstIn = new List<string>();
    public List<string> LineWhenPokemon4FirstIn = new List<string>();
    public List<string> LineWhenPokemon5FirstIn = new List<string>();
    public List<string> BattleStartLines = new List<string>();
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
    public List<string> GetLineWhenBattleStart()
    {
        return BattleStartLines;
    }
    public List<string> GetLineWhenPokemonFirstIn(BattlePokemon InPokemon)
    {
        List<string>[] Lists = new List<string>[]{
            LineWhenPokemon0FirstIn, 
            LineWhenPokemon1FirstIn,
            LineWhenPokemon2FirstIn,
            LineWhenPokemon3FirstIn,
            LineWhenPokemon4FirstIn,
            LineWhenPokemon5FirstIn,
        };
        for(int Index = 0; Index < 6; Index++)
        {
            if(InPokemon == BattlePokemons[Index])
            {
                return Lists[Index];
            }
        }

        return null;
    }
}
