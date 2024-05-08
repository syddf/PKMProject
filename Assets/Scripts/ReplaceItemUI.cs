using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
public class ReplaceItemUI : MonoBehaviour
{
    public BagPokemon CurrentBagPokemon;
    public BagPokemon ReplaceBagPokemon;
    public PokemonTrainer CurrentTrainer;

    public Image Pokemon0Image;
    public Image Pokemon1Image;
    public Image Pokemon2Image;
    public Image Pokemon3Image;
    public Image Pokemon4Image;
    public Image Pokemon5Image;
    public Image Item0Image;
    public Image Item1Image;
    public Image Item2Image;
    public Image Item3Image;
    public Image Item4Image;
    public Image Item5Image;
    public SavedData PlayerData;
    public GameObject ConfirmButton;
    public GameObject ItemDesc1Obj;
    public GameObject ItemDesc2Obj;
    public Image ItemDesc0Icon;
    public Image ItemDesc1Icon;
    public TextMeshProUGUI ItemDesc0Text;
    public TextMeshProUGUI ItemDesc1Text;

    public void OnClickPokemon(int Index)
    {
        if(CurrentTrainer.BagPokemons[Index] != CurrentBagPokemon 
            && CurrentTrainer.BagPokemons[Index].GetCanMega() == false
            && CurrentBagPokemon.GetCanMega() == false)
        {
            ReplaceBagPokemon = CurrentTrainer.BagPokemons[Index];
            UpdateUI();
            ConfirmButton.SetActive(true);
            ItemDesc1Obj.SetActive(true);
            ItemDesc2Obj.SetActive(true);

            BaseItem Item1 = GetBagPokemonItem(CurrentBagPokemon);
            BaseItem Item2 = GetBagPokemonItem(ReplaceBagPokemon);

            ItemDesc0Icon.sprite = PokemonSpritesManager.ItemSprites[Item1.ItemName];
            ItemDesc1Icon.sprite = PokemonSpritesManager.ItemSprites[Item2.ItemName];
            ItemDesc0Text.text = Item1.Description;
            ItemDesc1Text.text = Item2.Description;
        }
    }

    public void SetPokemonSprite(Image TargetImage, BagPokemon InPokemon, bool Chosen)
    {
        int Index = InPokemon.GetIndexInPKDex();
        TargetImage.sprite = PokemonSpritesManager.PKMSprites[Index];
        if(PlayerData.SavedPlayerData.OverrideData.ContainsKey(CurrentTrainer.TrainerName))
        {
            if(PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName].ContainsKey(InPokemon.GetPokemonName()))
            {
                BagPokemonOverrideData OverrideData = PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName][InPokemon.GetPokemonName()];
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
                            int NewIndex = ReferencePokemon.GetIndexInPKDex();
                            TargetImage.sprite = PokemonSpritesManager.PKMSprites[NewIndex];
                            break;
                        }
                    }
                }
            }
        }
        TargetImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        if(Chosen == false)
        {
            TargetImage.color = new Color(1.0f, 1.0f, 1.0f, 0.2f);
        }
    }

    public BaseItem GetBagPokemonItem(BagPokemon InPokemon)
    {
        if(PlayerData.SavedPlayerData.OverrideData.ContainsKey(CurrentTrainer.TrainerName))
        {
            if(PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName].ContainsKey(InPokemon.GetPokemonName()))
            {
                BagPokemonOverrideData OverrideData = PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName][InPokemon.GetPokemonName()];
                if(OverrideData.Overrided)
                {
                    int OverrideItem = OverrideData.ItemIndex;
                    return CurrentTrainer.BagPokemons[OverrideItem].GetItem();
                }
            }
        }
        return InPokemon.GetItem();
    } 
    public void SetItemSprite(Image TargetImage, BagPokemon InPokemon, bool Chosen)
    {
        TargetImage.sprite = PokemonSpritesManager.ItemSprites[InPokemon.GetItem().ItemName];
                
        if(PlayerData.SavedPlayerData.OverrideData.ContainsKey(CurrentTrainer.TrainerName))
        {
            if(PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName].ContainsKey(InPokemon.GetPokemonName()))
            {
                BagPokemonOverrideData OverrideData = PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName][InPokemon.GetPokemonName()];
                if(OverrideData.Overrided)
                {
                    int OverrideItem = OverrideData.ItemIndex;
                    TargetImage.sprite = PokemonSpritesManager.ItemSprites[CurrentTrainer.BagPokemons[OverrideItem].GetItem().ItemName];
                }
            }
        }
        TargetImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        if(Chosen == false)
        {
            TargetImage.color = new Color(1.0f, 1.0f, 1.0f, 0.2f);
        }
    }

    public void ChoosePokemon(BagPokemon InPokemon)
    {
        SetPokemonSprite(Pokemon0Image, CurrentTrainer.BagPokemons[0], InPokemon == CurrentTrainer.BagPokemons[0]);
        SetPokemonSprite(Pokemon1Image, CurrentTrainer.BagPokemons[1], InPokemon == CurrentTrainer.BagPokemons[1]);
        SetPokemonSprite(Pokemon2Image, CurrentTrainer.BagPokemons[2], InPokemon == CurrentTrainer.BagPokemons[2]);
        SetPokemonSprite(Pokemon3Image, CurrentTrainer.BagPokemons[3], InPokemon == CurrentTrainer.BagPokemons[3]);
        SetPokemonSprite(Pokemon4Image, CurrentTrainer.BagPokemons[4], InPokemon == CurrentTrainer.BagPokemons[4]);
        SetPokemonSprite(Pokemon5Image, CurrentTrainer.BagPokemons[5], InPokemon == CurrentTrainer.BagPokemons[5]);

        SetItemSprite(Item0Image, CurrentTrainer.BagPokemons[0], InPokemon == CurrentTrainer.BagPokemons[0]);
        SetItemSprite(Item1Image, CurrentTrainer.BagPokemons[1], InPokemon == CurrentTrainer.BagPokemons[1]);
        SetItemSprite(Item2Image, CurrentTrainer.BagPokemons[2], InPokemon == CurrentTrainer.BagPokemons[2]);
        SetItemSprite(Item3Image, CurrentTrainer.BagPokemons[3], InPokemon == CurrentTrainer.BagPokemons[3]);
        SetItemSprite(Item4Image, CurrentTrainer.BagPokemons[4], InPokemon == CurrentTrainer.BagPokemons[4]);
        SetItemSprite(Item5Image, CurrentTrainer.BagPokemons[5], InPokemon == CurrentTrainer.BagPokemons[5]);
    }

    public void SetCurrentTrainer(PokemonTrainer InTrainer, BagPokemon InCurrentBagPokemon)
    {
        CurrentTrainer = InTrainer;
        CurrentBagPokemon = InCurrentBagPokemon;
        ReplaceBagPokemon = null;
        ConfirmButton.SetActive(false);
        ItemDesc1Obj.SetActive(false);
        ItemDesc2Obj.SetActive(false);
        UpdateUI();
    }

    public void UpdateUI()
    {
        ChoosePokemon(ReplaceBagPokemon);
    }


    private int GetItemIndex(PokemonTrainer InTrainer, BaseItem InItem)
    {
        int Index = 0;
        for(Index = 0; Index < 6; Index++)
        {
            if(InTrainer.BagPokemons[Index].GetItem() == InItem)
            {
                return Index;
            }
        }
        return -1;
    }

    public void ModifyPokemonItem(BagPokemon CurrentPokemon, BaseItem InItem)
    {
        BagPokemonOverrideData OverrideData = new BagPokemonOverrideData();
        int ItemIndex = GetItemIndex(CurrentTrainer, InItem);
        if(PlayerData.SavedPlayerData.OverrideData.ContainsKey(CurrentTrainer.TrainerName))
        {
            if(PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName].ContainsKey(CurrentPokemon.GetPokemonName()))
            {
                BagPokemonOverrideData OldOverrideData = PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName][CurrentPokemon.GetPokemonName()];
                OldOverrideData.ItemIndex = ItemIndex;
                PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName][CurrentPokemon.GetPokemonName()] = OldOverrideData;
                return;
            }
            else
            {
                PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName].Add(CurrentPokemon.GetPokemonName(), OverrideData);
            }
        }
        else
        {
            PlayerData.SavedPlayerData.OverrideData.Add(CurrentTrainer.TrainerName, new SerializableDictionary<string, BagPokemonOverrideData>());
        }
           
        OverrideData.Overrided = true;
        OverrideData.ReplaceTrainerName = CurrentTrainer.TrainerName;
        OverrideData.ReplacePokemonName = CurrentPokemon.GetPokemonName();
        OverrideData.SkillIndex0 = 0;
        OverrideData.SkillIndex1 = 1;
        OverrideData.SkillIndex2 = 2;
        OverrideData.SkillIndex3 = 3;
        OverrideData.SkillIndex4 = 4;
        OverrideData.SkillIndex5 = 5;
        OverrideData.SkillIndex6 = 6;
        OverrideData.SkillIndex7 = 7;
        OverrideData.Nature = CurrentPokemon.GetNature();
        OverrideData.ItemIndex = GetItemIndex(CurrentTrainer, InItem);
        PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName][CurrentPokemon.GetPokemonName()] = OverrideData;
    }

    public void OnClickConfirm()
    {
        BaseItem Item1 = GetBagPokemonItem(CurrentBagPokemon);
        BaseItem Item2 = GetBagPokemonItem(ReplaceBagPokemon);
        ModifyPokemonItem(CurrentBagPokemon, Item2);
        ModifyPokemonItem(ReplaceBagPokemon, Item1);
        UpdateUI();

        ItemDesc0Icon.sprite = PokemonSpritesManager.ItemSprites[Item2.ItemName];
        ItemDesc1Icon.sprite = PokemonSpritesManager.ItemSprites[Item1.ItemName];
        ItemDesc0Text.text = Item2.Description;
        ItemDesc1Text.text = Item1.Description;
    }
}
