using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PokemonTrainer : MonoBehaviour
{
    public string TrainerName;
    public Sprite TrainerSprite;
    public BattlePokemon[] BattlePokemons = new BattlePokemon[6];
    public BagPokemon[] BagPokemons = new BagPokemon[6];
    public List<string> LineWhenPokemon0FirstIn = new List<string>();
    public List<string> LineWhenPokemon1FirstIn = new List<string>();
    public List<string> LineWhenPokemon2FirstIn = new List<string>();
    public List<string> LineWhenPokemon3FirstIn = new List<string>();
    public List<string> LineWhenPokemon4FirstIn = new List<string>();
    public List<string> LineWhenPokemon5FirstIn = new List<string>();
    public List<string> BattleStartLines = new List<string>();
    public AudioClip BGMFirst;
    public AudioClip BGMLoop;
    public float BGMDelaySeconds;
    public bool IsPlayer;
    public BaseTrainerSkill TrainerSkill;
    public int GetPokemonIndex(BattlePokemon InPokemon)
    {
        if(BattlePokemons[0] == InPokemon) return 0;
        if(BattlePokemons[1] == InPokemon) return 1;
        if(BattlePokemons[2] == InPokemon) return 2;
        if(BattlePokemons[3] == InPokemon) return 3;
        if(BattlePokemons[4] == InPokemon) return 4;
        if(BattlePokemons[5] == InPokemon) return 5;
        return -1;
    }
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
