using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class PokemonInfoUI : MonoBehaviour
{
    public TextMeshProUGUI HPText;
    public TextMeshProUGUI AtkText;
    public TextMeshProUGUI DefText;
    public TextMeshProUGUI SAtkText;
    public TextMeshProUGUI SDefText;
    public TextMeshProUGUI SpeedText;
    public TextMeshProUGUI NatureText;
    public TextMeshProUGUI AbilityText;
    public TextMeshProUGUI ItemText;
    public TextMeshProUGUI PokemonNameText;
    public Image PokemonSpriteImage;
    public BagPokemon CurrentBagPokemon;
    public TextMeshProUGUI AbilityDesc;
    public Image ItemImage;
    public TextMeshProUGUI ItemDesc;
    public void SetBagPokemon(BagPokemon InPokemon, bool Mega)
    {
        CurrentBagPokemon = InPokemon;
        int HP = InPokemon.GetMaxHP();
        HPText.text = HP.ToString() + "/" + HP.ToString();
        AtkText.text = InPokemon.GetAtk(Mega).ToString();
        DefText.text = InPokemon.GetDef(Mega).ToString();
        SAtkText.text = InPokemon.GetSAtk(Mega).ToString();
        SDefText.text = InPokemon.GetSDef(Mega).ToString();
        SpeedText.text = InPokemon.GetSpeed(Mega).ToString();
        PokemonNameText.text = InPokemon.GetPokemonName();
        BaseAbility Ability = InPokemon.GetAbility(Mega);
        if(Ability)
        {
            AbilityText.text = InPokemon.GetAbility(Mega).GetAbilityName();
            AbilityDesc.text = InPokemon.GetAbility(Mega).GetAbilityDesc();
        }
        else
        {
            AbilityText.text = "无";
            AbilityDesc.text = "无";
        }
        int Index = InPokemon.GetIndexInPKDex();
        if(Mega)
        {
            Index = Index + 2000;
        }
        PokemonSpriteImage.sprite = PokemonSpritesManager.PKMSprites[Index];
        NatureText.text = BagPokemon.GetChineseNameWithCorrection(InPokemon.GetNature());
        BaseItem Item = InPokemon.GetItem();
        if(Item)
        {
            ItemText.text = Item.ItemName;
            ItemDesc.text = Item.Description;
            ItemImage.sprite = PokemonSpritesManager.ItemSprites[Item.ItemName];
        }
        else
        {
            ItemText.text = "无";
            ItemDesc.text = "无";
            ItemImage.sprite = null;
        }
    }
}
