using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
public class PokemonEditMainMenu : MonoBehaviour
{
    public PokemonTrainer CurrentTrainer;
    public GameObject ReplacePokemonUI;
    public TrainerReplaceUI ReferenceTrainerReplaceUI;
    public BagPokemon CurrentPokemon;
    public PokemonInfoUI PkmInfoUI;
    public SkillReplaceUI Skill1UI;
    public SkillReplaceUI Skill2UI;
    public SkillReplaceUI Skill3UI;
    public SkillReplaceUI Skill4UI;

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
    public ReplacePokemonInfoUI ReplaceUI;
    public ReplaceItemUI ReplaceItemUI;
    public ReplaceSkillUI ReplaceSkillUI;

    public GameObject TrainerUI;

    public GameObject MegaToggleObj;
    public Toggle MegaToggle;
    public UIFadeOut WarningUI;
    public TextMeshProUGUI WarningText;
    public bool IsReplacePokemonLegal(BagPokemon InPokemon)
    {
        int ReplacedNum = 0;
        bool CurrentPokemonReplaced = false;
        for(int Index = 0; Index < 6; Index ++)
        {
            BagPokemon ReferencePokemon = CurrentTrainer.BagPokemons[Index];

            if(PlayerData.SavedPlayerData.OverrideData.ContainsKey(CurrentTrainer.TrainerName))
            {
                if(PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName].ContainsKey(ReferencePokemon.GetPokemonName()))
                {
                    BagPokemonOverrideData OverrideData = PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName][ReferencePokemon.GetPokemonName()];
                    if(OverrideData.Overrided)
                    {
                        string TrainerName = OverrideData.ReplaceTrainerName;
                        string PokemonName = OverrideData.ReplacePokemonName;
                        if(PokemonName != ReferencePokemon.GetPokemonName())
                        {
                            ReplacedNum++;
                        }
                    }
                }
            }
        }
        bool SameTrainer = ReplaceUI.CurrentTrainer.TrainerName == CurrentTrainer.TrainerName;
        if(PlayerData.SavedPlayerData.OverrideData.ContainsKey(CurrentTrainer.TrainerName))
        {
            if(PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName].ContainsKey(CurrentPokemon.GetPokemonName()))
            {
                BagPokemonOverrideData OverrideData = PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName][CurrentPokemon.GetPokemonName()];
                if(OverrideData.Overrided)
                {
                    string TrainerName = OverrideData.ReplaceTrainerName;
                    string PokemonName = OverrideData.ReplacePokemonName;
                    if(PokemonName != CurrentPokemon.GetPokemonName() && TrainerName != CurrentTrainer.TrainerName)
                    {
                        CurrentPokemonReplaced = true;
                    }
                }
            }
        }
        if(ReplacedNum >= 2 && CurrentPokemonReplaced == false && SameTrainer == false)
        {
            WarningUI.FadeOutUI(1.0f);
            WarningText.text = "同一队伍最多替换两只宝可梦！";
            return false;
        }
        if(InPokemon.GetCanMega() == true || CurrentPokemon.GetCanMega() == true)
        {
            WarningUI.FadeOutUI(1.0f);
            WarningText.text = "无法替换可Mega的宝可梦！";
            return false;
        }
        for(int Index = 0; Index < 6; Index ++)
        {
            BagPokemon ReferencePokemon = CurrentTrainer.BagPokemons[Index];

            if(PlayerData.SavedPlayerData.OverrideData.ContainsKey(CurrentTrainer.TrainerName))
            {
                if(PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName].ContainsKey(ReferencePokemon.GetPokemonName()))
                {
                    BagPokemonOverrideData OverrideData = PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName][ReferencePokemon.GetPokemonName()];
                    if(OverrideData.Overrided)
                    {
                        string TrainerName = OverrideData.ReplaceTrainerName;
                        string PokemonName = OverrideData.ReplacePokemonName;
                        GameObject TrainerObj = GameObject.Find("SingleBattle/AllTrainers/" + TrainerName);
                        PokemonTrainer Trainer = TrainerObj.GetComponent<PokemonTrainer>();
                        foreach(var BagPkm in Trainer.BagPokemons)
                        {
                            if(BagPkm.GetPokemonName() == PokemonName)
                            {
                                ReferencePokemon = BagPkm;
                                break;
                            }
                        }
                    }
                }
            }

            if(ReferencePokemon == InPokemon)
            {
                WarningUI.FadeOutUI(1.0f);
                WarningText.text = "队伍内已经有相同的宝可梦了！";
                return false;
            }
        }
        return true;
    }

    public void SwitchMega()
    {
        UpdatePokemonInfoUI();
    }
    public void ChoosePokemon(BagPokemon InPokemon)
    {
        MegaToggleObj.SetActive(InPokemon.GetCanMega());
        MegaToggle.isOn = false;
        SetPokemonSprite(Pokemon0Image, Item0Image, CurrentTrainer.BagPokemons[0], InPokemon == CurrentTrainer.BagPokemons[0]);
        SetPokemonSprite(Pokemon1Image, Item1Image, CurrentTrainer.BagPokemons[1], InPokemon == CurrentTrainer.BagPokemons[1]);
        SetPokemonSprite(Pokemon2Image, Item2Image, CurrentTrainer.BagPokemons[2], InPokemon == CurrentTrainer.BagPokemons[2]);
        SetPokemonSprite(Pokemon3Image, Item3Image, CurrentTrainer.BagPokemons[3], InPokemon == CurrentTrainer.BagPokemons[3]);
        SetPokemonSprite(Pokemon4Image, Item4Image, CurrentTrainer.BagPokemons[4], InPokemon == CurrentTrainer.BagPokemons[4]);
        SetPokemonSprite(Pokemon5Image, Item5Image, CurrentTrainer.BagPokemons[5], InPokemon == CurrentTrainer.BagPokemons[5]);
    }

    public void SetPokemonSprite(Image TargetImage, Image ItemImage, BagPokemon InPokemon, bool Chosen)
    {
        int Index = InPokemon.GetIndexInPKDex();
        TargetImage.sprite = PokemonSpritesManager.PKMSprites[Index];
        ItemImage.sprite = PokemonSpritesManager.ItemSprites[InPokemon.GetItem().ItemName];
        if(PlayerData.SavedPlayerData.OverrideData.ContainsKey(CurrentTrainer.TrainerName))
        {
            if(PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName].ContainsKey(InPokemon.GetPokemonName()))
            {
                BagPokemonOverrideData OverrideData = PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName][InPokemon.GetPokemonName()];
                if(OverrideData.Overrided)
                {
                    string TrainerName = OverrideData.ReplaceTrainerName;
                    string PokemonName = OverrideData.ReplacePokemonName;
                    int OverrideItem = OverrideData.ItemIndex;
                    ItemImage.sprite = PokemonSpritesManager.ItemSprites[CurrentTrainer.BagPokemons[OverrideItem].GetItem().ItemName];

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
        ItemImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        if(Chosen == false)
        {
            TargetImage.color = new Color(1.0f, 1.0f, 1.0f, 0.2f);
            ItemImage.color = new Color(1.0f, 1.0f, 1.0f, 0.2f);
        }
    }
    
    public void SetCurrentPokemonTrainer(PokemonTrainer InTrainer)
    {
        CurrentTrainer = InTrainer;
        SetBagPokemon(CurrentTrainer.BagPokemons[0]);
    }
    public void OnClickReplacePokemon()
    {
        if(CurrentTrainer.TrainerName == "小智" || CurrentTrainer.TrainerName == "莎莉娜")
        {                
            WarningUI.FadeOutUI(1.0f);
            WarningText.text = "本场不可替换宝可梦！";
            return;
        }
        ReplacePokemonUI.SetActive(true);
        ReferenceTrainerReplaceUI.CurrentTrainer = CurrentTrainer;
        ReferenceTrainerReplaceUI.InitReplaceTrainerEntry();
        this.gameObject.SetActive(false);
        TrainerUI.SetActive(true);
    }

    public void OnClickReplace()
    {
        if(IsReplacePokemonLegal(ReplaceUI.CurrentBagPokemon) == false)
        {
            return;
        }
        BagPokemonOverrideData OverrideData = PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName][CurrentPokemon.GetPokemonName()];
        int OverrideItem = -1;
        if(PlayerData.SavedPlayerData.OverrideData.ContainsKey(CurrentTrainer.TrainerName))
        {
            if(PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName].ContainsKey(CurrentPokemon.GetPokemonName()))
            {
                OverrideItem = PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName][CurrentPokemon.GetPokemonName()].ItemIndex;
            }
        }
        OverrideData.Overrided = true;
        OverrideData.ReplaceTrainerName = ReplaceUI.CurrentTrainer.TrainerName;
        OverrideData.ReplacePokemonName = ReplaceUI.CurrentBagPokemon.GetPokemonName();
        OverrideData.SkillIndex0 = 0;
        OverrideData.SkillIndex1 = 1;
        OverrideData.SkillIndex2 = 2;
        OverrideData.SkillIndex3 = 3;
        OverrideData.SkillIndex4 = 4;
        OverrideData.SkillIndex5 = 5;
        OverrideData.SkillIndex6 = 6;
        OverrideData.SkillIndex7 = 7;
        OverrideData.Nature = ReplaceUI.CurrentBagPokemon.GetNature();
        OverrideData.ItemIndex = GetItemIndex(CurrentTrainer, CurrentPokemon.GetItem());
        if(OverrideItem != -1)
        {
            OverrideData.ItemIndex = OverrideItem;
        }
        PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName][CurrentPokemon.GetPokemonName()] = OverrideData;
        UpdatePokemonInfoUI();
        ChoosePokemon(CurrentPokemon);
        ReplaceUI.ResetUI();
    }

    public void UpdateItem()
    {
        ChoosePokemon(CurrentPokemon);
    }

    public void OnClickNature(int Val)
    {
        BagPokemonOverrideData OverrideData = PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName][CurrentPokemon.GetPokemonName()];
        OverrideData.Nature = (PokemonNature)Val;
        PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName][CurrentPokemon.GetPokemonName()] = OverrideData;
        UpdatePokemonInfoUI(false);
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

    public BagPokemonOverrideData GetOverrideData()
    {
        BagPokemonOverrideData OverrideData = new BagPokemonOverrideData();
        if(PlayerData.SavedPlayerData.OverrideData.ContainsKey(CurrentTrainer.TrainerName))
        {
            if(PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName].ContainsKey(CurrentPokemon.GetPokemonName()))
            {
                return PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName][CurrentPokemon.GetPokemonName()];
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
        OverrideData.ItemIndex = GetItemIndex(CurrentTrainer, CurrentPokemon.GetItem());
        PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName][CurrentPokemon.GetPokemonName()] = OverrideData;
        return PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName][CurrentPokemon.GetPokemonName()];
    }

    public void OnClickTrainerPokemon(int Index)
    {
        SetBagPokemon(CurrentTrainer.BagPokemons[Index]);
    }

    public void OnClickReplaceSkill()
    {
        ReplaceSkillUI.gameObject.SetActive(true);
        ReplaceSkillUI.SetCurrentTrainer(CurrentTrainer, CurrentPokemon);
        this.gameObject.SetActive(false);
    }

    public void OnClickReplaceItem()
    {
        ReplaceItemUI.gameObject.SetActive(true);
        ReplaceItemUI.SetCurrentTrainer(CurrentTrainer, CurrentPokemon);
        this.gameObject.SetActive(false);
    }

    public void SetBagPokemon(BagPokemon InPokemon)
    {
        CurrentPokemon = InPokemon;
        ReplaceItemUI.SetCurrentTrainer(CurrentTrainer, CurrentPokemon);
        ReplaceSkillUI.SetCurrentTrainer(CurrentTrainer, CurrentPokemon);
        UpdatePokemonInfoUI();
        ChoosePokemon(InPokemon);
    }

    public void OnClickSetAsBattleTrainer()
    {
        if(CurrentTrainer)
        {
            PlayerData.SavedPlayerData.BattleTrainerName = CurrentTrainer.TrainerName;
        }
    }

    public void OnClickResetTrainerTeam()
    {
        if(PlayerData.SavedPlayerData.OverrideData.ContainsKey(CurrentTrainer.TrainerName))
        {
            PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName].Clear();
            SetBagPokemon(CurrentPokemon);
        }
    }
    public void UpdatePokemonInfoUI(bool UpdateNatureDropDown = true)
    {
        BagPokemonOverrideData OverrideData = GetOverrideData();
        PkmInfoUI.CurrentTrainer = CurrentTrainer;
        PkmInfoUI.SetBagPokemon(CurrentPokemon, MegaToggle.isOn && CurrentPokemon.GetCanMega(), OverrideData, UpdateNatureDropDown);

        int Skill0Index = 0;
        int Skill1Index = 1;
        int Skill2Index = 2;
        int Skill3Index = 3;

        BagPokemon ReferencePokemon = CurrentPokemon;
        if(PlayerData.SavedPlayerData.OverrideData.ContainsKey(CurrentTrainer.TrainerName))
        {
            if(PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName].ContainsKey(CurrentPokemon.GetPokemonName()))
            {
                if(PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName][CurrentPokemon.GetPokemonName()].Overrided)
                {
                    Skill0Index = PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName][CurrentPokemon.GetPokemonName()].SkillIndex0;
                    Skill1Index = PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName][CurrentPokemon.GetPokemonName()].SkillIndex1;
                    Skill2Index = PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName][CurrentPokemon.GetPokemonName()].SkillIndex2;
                    Skill3Index = PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName][CurrentPokemon.GetPokemonName()].SkillIndex3;

                    string TrainerName = PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName][CurrentPokemon.GetPokemonName()].ReplaceTrainerName;
                    string PokemonName = PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName][CurrentPokemon.GetPokemonName()].ReplacePokemonName;

                    GameObject TrainerObj = GameObject.Find("SingleBattle/AllTrainers/" + TrainerName);
                    PokemonTrainer Trainer = TrainerObj.GetComponent<PokemonTrainer>();
                    foreach(var BagPkm in Trainer.BagPokemons)
                    {
                        if(BagPkm.GetPokemonName() == PokemonName)
                        {
                            ReferencePokemon = BagPkm;
                            break;
                        }
                    }
                }
            }
        }
        Skill1UI.SetSkill(ReferencePokemon.GetSkillPoolSkill(Skill0Index));
        Skill2UI.SetSkill(ReferencePokemon.GetSkillPoolSkill(Skill1Index));
        Skill3UI.SetSkill(ReferencePokemon.GetSkillPoolSkill(Skill2Index));
        Skill4UI.SetSkill(ReferencePokemon.GetSkillPoolSkill(Skill3Index));
    }
}
