using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPokemonDataCollection", menuName = "Pokemon/PokemonDataCollection")]
public class PokemonDataCollection : ScriptableObject
{
    public List<PokemonData> pokemonList;

    public PokemonData GetPokemonData(string PokemonName)
    {
        foreach(var PokemonData in pokemonList)
        {
            if(PokemonData.Name == PokemonName)
            {
                return PokemonData;
            }
        }
        return null;
    }
}