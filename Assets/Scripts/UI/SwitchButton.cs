using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchButton : MonoBehaviour
{
    public BattleManager g_BattleManager;
    public int PKMIndexInTrainerTeam;
    public Image PokemonSprite;
    public BattlePokemon ReferencePokemon;
    public void UpdateSprite(BattlePokemon InPokemon, BattleManager InManager)
    {
        g_BattleManager = InManager;
        ReferencePokemon = InPokemon;
        if(InPokemon == null)
            return;        
        int Index = InPokemon.GetIndexInPKDex();
        PokemonSprite.sprite = PokemonSpritesManager.Sprites[Index];
        if(InPokemon.IsDead())
        {
            PokemonSprite.color = new Color(PokemonSprite.color.r, PokemonSprite.color.g, PokemonSprite.color.b, 0.3f);
        }
        else
        {
            PokemonSprite.color = new Color(PokemonSprite.color.r, PokemonSprite.color.g, PokemonSprite.color.b, 1.0f);
        }
    }

    public void OnClick()
    {
        if(ReferencePokemon && ReferencePokemon.IsDead() == false)
        {
            g_BattleManager.OnPlayerSwitchNewPokemon(ReferencePokemon);
        }
    }
}
