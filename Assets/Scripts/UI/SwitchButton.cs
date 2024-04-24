using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchButton : MonoBehaviour
{
    public BattleManager g_BattleManager;
    public int PKMIndexInTrainerTeam;
    public Image PokemonSprite;
    public Image ItemSprite;
    public BattlePokemon ReferencePokemon;
    public void UpdateSprite(BattlePokemon InPokemon, BattleManager InManager)
    {
        g_BattleManager = InManager;
        ReferencePokemon = InPokemon;
        if(InPokemon == null)
            return;        
        int Index = InPokemon.GetIndexInPKDex();
        if(InPokemon.IsMega())
        {
            Index = Index + 2000;
        }
        PokemonSprite.sprite = PokemonSpritesManager.PKMSprites[Index];
        ItemSprite.sprite = null;
        if(InPokemon.HasItem())
        {
            ItemSprite.sprite = PokemonSpritesManager.ItemSprites[InPokemon.GetBattleItem().GetItemName()];
        }
        if(InPokemon.IsDead())
        {
            PokemonSprite.color = new Color(PokemonSprite.color.r, PokemonSprite.color.g, PokemonSprite.color.b, 0.3f);
            ItemSprite.color = new Color(ItemSprite.color.r, ItemSprite.color.g, ItemSprite.color.b, 0.3f);
        }
        else
        {
            PokemonSprite.color = new Color(PokemonSprite.color.r, PokemonSprite.color.g, PokemonSprite.color.b, 1.0f);
            ItemSprite.color = new Color(ItemSprite.color.r, ItemSprite.color.g, ItemSprite.color.b, 1.0f);
        }
        if(ItemSprite.sprite == null)
        {
            ItemSprite.color = new Color(ItemSprite.color.r, ItemSprite.color.g, ItemSprite.color.b, 0.0f);
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
