using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlePokemonsInfoUI : MonoBehaviour
{
    public Image Pkm1;
    public Image Pkm2;
    public Image Pkm3;
    public Image Pkm4;
    public Image Pkm5;
    public Image Pkm6;

    public void UpdateSprite(BattlePokemon InPokemon, Image ImgUI)
    {
        int Index = InPokemon.GetIndexInPKDex();
        if(InPokemon.IsMega())
        {
            Index = Index + 2000;
        }
        ImgUI.sprite = PokemonSpritesManager.PKMSprites[Index];

        if(InPokemon.IsDead())
        {
            ImgUI.color = new Color(ImgUI.color.r, ImgUI.color.g, ImgUI.color.b, 0.3f);
        }
        else
        {
            ImgUI.color = new Color(ImgUI.color.r, ImgUI.color.g, ImgUI.color.b, 1.0f);
        }
    }

    public void UpdateUI()
    {
        PokemonTrainer EnemyTrainer = BattleManager.StaticManager.GetEnemyTrainer();
        UpdateSprite(EnemyTrainer.BattlePokemons[0], Pkm1);
        UpdateSprite(EnemyTrainer.BattlePokemons[1], Pkm2);
        UpdateSprite(EnemyTrainer.BattlePokemons[2], Pkm3);
        UpdateSprite(EnemyTrainer.BattlePokemons[3], Pkm4);
        UpdateSprite(EnemyTrainer.BattlePokemons[4], Pkm5);
        UpdateSprite(EnemyTrainer.BattlePokemons[5], Pkm6);
    }
}
