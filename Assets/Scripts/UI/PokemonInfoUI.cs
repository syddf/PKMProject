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
    public TMP_Dropdown NatureDropDown;
    public PokemonTrainer CurrentTrainer;
    public TypeUI TypeUI1;
    public TypeUI TypeUI2;
    
    public void SetBattlePokemon(BattlePokemon InPokemon)
    {
        int HP = InPokemon.GetHP();
        int MaxHP = InPokemon.GetMaxHP();
        bool Mega = InPokemon.IsMega();
        HPText.text = HP.ToString() + "/" + MaxHP.ToString();
        AtkText.text = InPokemon.GetAtk(ECaclStatsMode.Normal, BattleManager.StaticManager).ToString();
        DefText.text = InPokemon.GetDef(ECaclStatsMode.Normal, BattleManager.StaticManager).ToString();
        SAtkText.text = InPokemon.GetSAtk(ECaclStatsMode.Normal, BattleManager.StaticManager).ToString();
        SDefText.text = InPokemon.GetSDef(ECaclStatsMode.Normal, BattleManager.StaticManager).ToString();
        SpeedText.text = InPokemon.GetSpeed(ECaclStatsMode.Normal, BattleManager.StaticManager).ToString();
        PokemonNameText.text = InPokemon.GetName();
        BaseAbility Ability = InPokemon.GetAbility();
        if(Ability)
        {
            AbilityDesc.text = "特性：" + InPokemon.GetAbility().GetAbilityDesc();
        }
        else
        {
            AbilityDesc.text = "无";
        }
        PokemonSpriteImage.sprite = InPokemon.GetPkmSprite();

        NatureText.text = BagPokemon.GetChineseNameWithCorrection(InPokemon.GetNature());
        BattleItem Item = InPokemon.GetItem();
        if(Item != null)
        {
            ItemText.text = Item.GetItemName();
            ItemDesc.text = "道具：" + Item.GetItemDescription();
        }
        else
        {
            ItemText.text = "无";
            ItemDesc.text = "无";
        }

        TypeUI1.SetType(InPokemon.GetType1(BattleManager.StaticManager, null, null));
        TypeUI2.gameObject.SetActive(false);
        if(InPokemon.GetType2(BattleManager.StaticManager, null, null) != EType.None)
        {
            TypeUI2.gameObject.SetActive(true);            
            TypeUI2.SetType(InPokemon.GetType2(BattleManager.StaticManager, null, null));
        }

    }
    public void SetBagPokemon(BagPokemon InPokemon, bool Mega, BagPokemonOverrideData OverrideData, bool UpdateNatureDropDown = true)
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

        TypeUI1.SetType(CurrentBagPokemon.GetType0(Mega));
        TypeUI2.gameObject.SetActive(false);
        if(CurrentBagPokemon.GetType1(Mega) != EType.None)
        {
            TypeUI2.gameObject.SetActive(true);            
            TypeUI2.SetType(CurrentBagPokemon.GetType1(Mega));
        }

        if(UpdateNatureDropDown && NatureDropDown)
        {
            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
            int Start = (int)PokemonNature.Hardy;
            int End = (int)PokemonNature.Serious;
            for(int Val = Start; Val <= End; Val++)
            {
                options.Add(new TMP_Dropdown.OptionData(BagPokemon.GetChineseNameWithCorrection((PokemonNature)Val)));
            }
            NatureDropDown.ClearOptions();
            NatureDropDown.AddOptions(options);
            NatureDropDown.value = (int)InPokemon.GetNature();
        }


        if(OverrideData.Overrided)
        {
            string TrainerName = OverrideData.ReplaceTrainerName;
            string PokemonName = OverrideData.ReplacePokemonName;

            GameObject TrainerObj = GameObject.Find("SingleBattle/AllTrainers/" + TrainerName);
            PokemonTrainer Trainer = TrainerObj.GetComponent<PokemonTrainer>();
            BagPokemon ReferencePokemon = null;
            foreach(var BagPkm in Trainer.BagPokemons)
            {
                if(BagPkm.GetPokemonName() == PokemonName)
                {
                    ReferencePokemon = BagPkm;
                    break;
                }
            }

            if(ReferencePokemon != null)
            {
                if(UpdateNatureDropDown && NatureDropDown)
                {
                    NatureDropDown.value = (int)OverrideData.Nature;
                }
                ReferencePokemon.SetOverrideNature(OverrideData.Nature);
                int NewHP = ReferencePokemon.GetMaxHP();
                HPText.text = NewHP.ToString() + "/" + NewHP.ToString();
                AtkText.text = ReferencePokemon.GetAtk(Mega).ToString();
                DefText.text = ReferencePokemon.GetDef(Mega).ToString();
                SAtkText.text = ReferencePokemon.GetSAtk(Mega).ToString();
                SDefText.text = ReferencePokemon.GetSDef(Mega).ToString();
                SpeedText.text = ReferencePokemon.GetSpeed(Mega).ToString();
                PokemonNameText.text = ReferencePokemon.GetPokemonName();
                BaseAbility NewAbility = ReferencePokemon.GetAbility(Mega);
                if(NewAbility)
                {
                    AbilityText.text = ReferencePokemon.GetAbility(Mega).GetAbilityName();
                    AbilityDesc.text = ReferencePokemon.GetAbility(Mega).GetAbilityDesc();
                }
                else
                {
                    AbilityText.text = "无";
                    AbilityDesc.text = "无";
                }
                int NewIndex = ReferencePokemon.GetIndexInPKDex();
                if(Mega)
                {
                    NewIndex = NewIndex + 2000;
                }
                PokemonSpriteImage.sprite = PokemonSpritesManager.PKMSprites[NewIndex];
                NatureText.text = BagPokemon.GetChineseNameWithCorrection(ReferencePokemon.GetNature());

                int NewItemIndex = OverrideData.ItemIndex;
                if(CurrentTrainer)
                {
                    BaseItem NewItem = CurrentTrainer.BagPokemons[NewItemIndex].GetItem();
                    if(NewItem)
                    {
                        ItemText.text = NewItem.ItemName;
                        ItemDesc.text = NewItem.Description;
                        ItemImage.sprite = PokemonSpritesManager.ItemSprites[NewItem.ItemName];
                    }
                    else
                    {
                        ItemText.text = "无";
                        ItemDesc.text = "无";
                        ItemImage.sprite = null;
                    }
                }
                else
                {
                    BaseItem NewItem = ReferencePokemon.GetItem();
                    if(NewItem)
                    {
                        ItemText.text = NewItem.ItemName;
                        ItemDesc.text = NewItem.Description;
                        ItemImage.sprite = PokemonSpritesManager.ItemSprites[NewItem.ItemName];
                    }
                    else
                    {
                        ItemText.text = "无";
                        ItemDesc.text = "无";
                        ItemImage.sprite = null;
                    }
                }

                ReferencePokemon.DisableOverrideNature();

                TypeUI1.SetType(ReferencePokemon.GetType0(Mega));
                TypeUI2.gameObject.SetActive(false);
                if(ReferencePokemon.GetType1(Mega) != EType.None)
                {
                    TypeUI2.gameObject.SetActive(true);            
                    TypeUI2.SetType(ReferencePokemon.GetType1(Mega));
                }
            }
        }
    }
}
