using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class HistoryEntry : MonoBehaviour
{
    public TextMeshProUGUI ModeText;
    public Image EnemyTrainer;
    public Image PlayerTrainer;

    public Image Pkm1;
    public Image Pkm2;
    public Image Pkm3;
    public Image Pkm4;
    public Image Pkm5;
    public Image Pkm6;
    public Image Item1;
    public Image Item2;
    public Image Item3;
    public Image Item4;
    public Image Item5;
    public Image Item6;
    

    public void UpdatePkm(Image Pokemon, Image Item, int PkmIndex, string ItemName)
    {
        Pokemon.sprite = PokemonSpritesManager.PKMSprites[PkmIndex];
        Item.sprite = PokemonSpritesManager.ItemSprites[ItemName];
    }

    public void UpdateUI(BattleHistory InHistory)
    {
        ModeText.text = "普通模式";
        if(InHistory.Cheated)
        {
            ModeText.text = "休闲模式";
        }

        string EnemySpritePath = "UI/TrainerAvator/" + InHistory.EnemyTrainerName;
        Sprite EnemySprite = Resources.Load<Sprite>(EnemySpritePath);
        EnemyTrainer.sprite = EnemySprite;

        string PlayerSpritePath = "UI/TrainerAvator/" + InHistory.PlayerTrainerName;
        Sprite PlayerSprite = Resources.Load<Sprite>(PlayerSpritePath);
        PlayerTrainer.sprite = PlayerSprite;

        UpdatePkm(Pkm1, Item1, InHistory.Pkm1Index, InHistory.Item1Name);
        UpdatePkm(Pkm2, Item2, InHistory.Pkm2Index, InHistory.Item2Name);
        UpdatePkm(Pkm3, Item3, InHistory.Pkm3Index, InHistory.Item3Name);
        UpdatePkm(Pkm4, Item4, InHistory.Pkm4Index, InHistory.Item4Name);
        UpdatePkm(Pkm5, Item5, InHistory.Pkm5Index, InHistory.Item5Name);
        UpdatePkm(Pkm6, Item6, InHistory.Pkm6Index, InHistory.Item6Name);
    }
}
