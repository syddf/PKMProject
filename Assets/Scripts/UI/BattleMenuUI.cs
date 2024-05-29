using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
public class BattleMenuUI : MonoBehaviour
{
    public Image Pokemon0Image;
    public Image Pokemon1Image;
    public Image Pokemon2Image;
    public Image Pokemon3Image;
    public Image Pokemon4Image;
    public Image Pokemon5Image;
    public Image TrainerSprite;
    public TextMeshProUGUI TrainerSkillName;

    public TextMeshProUGUI TrainerSkillDesc;
    public TextMeshProUGUI SpecialRuleDesc;
    public TextMeshProUGUI TrainerName;
    public PokemonTrainer CurrentTrainer;
    public BagPokemon CurrentPokemon;
    public PokemonInfoUI InfoUI;
    public GameObject MegaToggleObj;
    public Toggle MegaToggle;
    public PokemonTrainer PlayerTrainer;
    public BaseSpecialRule CurrentSpecialRule;

    public Image PlayerPokemon0Image;
    public Image PlayerPokemon1Image;
    public Image PlayerPokemon2Image;
    public Image PlayerPokemon3Image;
    public Image PlayerPokemon4Image;
    public Image PlayerPokemon5Image;
    public Image PlayerTrainerImage;
    public GameObject Pokemon0FirstObj;
    public GameObject Pokemon1FirstObj;
    public GameObject Pokemon2FirstObj;
    public GameObject Pokemon3FirstObj;
    public GameObject Pokemon4FirstObj;
    public GameObject Pokemon5FirstObj;

    public BagPokemon PlayerOverridePokemon1;
    public BagPokemon PlayerOverridePokemon2;
    public BagPokemon PlayerOverridePokemon3;
    public BagPokemon PlayerOverridePokemon4;
    public BagPokemon PlayerOverridePokemon5;
    public BagPokemon PlayerOverridePokemon6;
    public SavedData SavedPlayerData;
    public BattleManager ReferenceBattleManager;
    public int FirstPokemonIndex = 0;
    public AudioController BattleBGM;
    public int ChapterIndex;
    public bool IsFirstBattle;
    public SetTrainerSkillInfo ReferenceTrainerSkillUI;
    public void OnChangeFirstPokemon(int Index)
    {
        FirstPokemonIndex = Index;
        Pokemon0FirstObj.SetActive(false);
        Pokemon1FirstObj.SetActive(false);
        Pokemon2FirstObj.SetActive(false);
        Pokemon3FirstObj.SetActive(false);
        Pokemon4FirstObj.SetActive(false);
        Pokemon5FirstObj.SetActive(false);
        if(Index == 0)
            Pokemon0FirstObj.SetActive(true);
        if(Index == 1)
            Pokemon1FirstObj.SetActive(true);
        if(Index == 2)
            Pokemon2FirstObj.SetActive(true);
        if(Index == 3)
            Pokemon3FirstObj.SetActive(true);
        if(Index == 4)
            Pokemon4FirstObj.SetActive(true);
        if(Index == 5)
            Pokemon5FirstObj.SetActive(true);
    }

    public void OnClickBeginBattle()
    {
        ReferenceBattleManager.SetSpecialRule(CurrentSpecialRule);
        ReferenceBattleManager.SetPlayerTrainer(PlayerTrainer);
        ReferenceBattleManager.SetEnemyTrainer(CurrentTrainer);
        ReferenceBattleManager.BeginBattle(FirstPokemonIndex, ChapterIndex, IsFirstBattle);
        BattleBGM.firstClip = CurrentTrainer.BGMFirst;
        BattleBGM.secondClip = CurrentTrainer.BGMLoop;
    }
    
    public void UpdatePlayerTeam()
    {
        PokemonTrainer UsingTrainer = GameObject.Find("SingleBattle/AllTrainers/" + SavedPlayerData.SavedPlayerData.BattleTrainerName).GetComponent<PokemonTrainer>();
        PlayerTrainer.TrainerName = UsingTrainer.TrainerName;
        PlayerTrainer.TrainerSprite = UsingTrainer.TrainerSprite;
        PlayerTrainer.TrainerSkill = UsingTrainer.TrainerSkill;
        ReferenceTrainerSkillUI.SetBattleTrainer(UsingTrainer);
        PlayerTrainer.BagPokemons[0].UpdateByOverrideData(SavedPlayerData, UsingTrainer.TrainerName, UsingTrainer.BagPokemons[0].GetPokemonName());
        PlayerTrainer.BagPokemons[1].UpdateByOverrideData(SavedPlayerData, UsingTrainer.TrainerName, UsingTrainer.BagPokemons[1].GetPokemonName());
        PlayerTrainer.BagPokemons[2].UpdateByOverrideData(SavedPlayerData, UsingTrainer.TrainerName, UsingTrainer.BagPokemons[2].GetPokemonName());
        PlayerTrainer.BagPokemons[3].UpdateByOverrideData(SavedPlayerData, UsingTrainer.TrainerName, UsingTrainer.BagPokemons[3].GetPokemonName());
        PlayerTrainer.BagPokemons[4].UpdateByOverrideData(SavedPlayerData, UsingTrainer.TrainerName, UsingTrainer.BagPokemons[4].GetPokemonName());
        PlayerTrainer.BagPokemons[5].UpdateByOverrideData(SavedPlayerData, UsingTrainer.TrainerName, UsingTrainer.BagPokemons[5].GetPokemonName());
        PlayerOverridePokemon1 = PlayerTrainer.BagPokemons[0];
        PlayerOverridePokemon2 = PlayerTrainer.BagPokemons[1];
        PlayerOverridePokemon3 = PlayerTrainer.BagPokemons[2];
        PlayerOverridePokemon4 = PlayerTrainer.BagPokemons[3];
        PlayerOverridePokemon5 = PlayerTrainer.BagPokemons[4];
        PlayerOverridePokemon6 = PlayerTrainer.BagPokemons[5];
        UpdatePlayerInfo();
        UpdateSpecialRule();
    }

    public void SetPokemonSprite(Image TargetImage, BagPokemon InPokemon, bool Chosen)
    {
        int Index = InPokemon.GetIndexInPKDex();
        TargetImage.sprite = PokemonSpritesManager.PKMSprites[Index];

        TargetImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        if(Chosen == false)
        {
            TargetImage.color = new Color(1.0f, 1.0f, 1.0f, 0.2f);
        }
    }

    public void SetEnemyPokemonTrainer(PokemonTrainer InTrainer)
    {
        CurrentTrainer = InTrainer;
        TrainerName.text = CurrentTrainer.TrainerName;
        TrainerSprite.sprite = InTrainer.TrainerSprite;
        TrainerSkillDesc.text = InTrainer.TrainerSkill.GetSkillDescription();
        TrainerSkillName.text = InTrainer.TrainerSkill.GetSkillName();
        ChoosePokemon(CurrentTrainer.BagPokemons[0]);
    }

    public void UpdateSpecialRule()
    {
        if(CurrentSpecialRule)
        {
            SpecialRuleDesc.text = CurrentSpecialRule.Description;
            if(CurrentSpecialRule.Name == "特殊规则(紫罗兰)")
            {
                string ExtraString = "\n受影响的宝可梦：";
                if(PlayerOverridePokemon1.GetSpeciesTotalValue(false) >= 500)
                {
                    ExtraString += PlayerOverridePokemon1.GetPokemonName();
                }
                if(PlayerOverridePokemon2.GetSpeciesTotalValue(false) >= 500)
                {
                    ExtraString += " ";
                    ExtraString += PlayerOverridePokemon2.GetPokemonName();
                }
                if(PlayerOverridePokemon3.GetSpeciesTotalValue(false) >= 500)
                {
                    ExtraString += " ";
                    ExtraString += PlayerOverridePokemon3.GetPokemonName();
                }
                if(PlayerOverridePokemon4.GetSpeciesTotalValue(false) >= 500)
                {
                    ExtraString += " ";
                    ExtraString += PlayerOverridePokemon4.GetPokemonName();
                }
                if(PlayerOverridePokemon5.GetSpeciesTotalValue(false) >= 500)
                {
                    ExtraString += " ";
                    ExtraString += PlayerOverridePokemon5.GetPokemonName();
                }
                if(PlayerOverridePokemon6.GetSpeciesTotalValue(false) >= 500)
                {
                    ExtraString += " ";
                    ExtraString += PlayerOverridePokemon6.GetPokemonName();
                }
                SpecialRuleDesc.text = CurrentSpecialRule.Description + ExtraString;
            }
        }
        else
        {
            SpecialRuleDesc.text = "无";
        }        
    }
    public void SetSpecialRule(BaseSpecialRule InRule)
    {
        CurrentSpecialRule = InRule;
        UpdateSpecialRule();
    }
    public void SetPlayerPokemonTrainer(PokemonTrainer InTrainer)
    {
        PlayerTrainer = InTrainer;
        UpdatePlayerTeam();
        UpdatePlayerInfo();
    }

    public void SwitchMega(bool isOn)
    {
        BagPokemonOverrideData OverrideData = new BagPokemonOverrideData();
        InfoUI.SetBagPokemon(CurrentPokemon, isOn, OverrideData);
    }

    public void ChoosePokemon(BagPokemon InPokemon)
    {
        MegaToggleObj.SetActive(InPokemon.GetCanMega());
        MegaToggle.isOn = false;
        SetPokemonSprite(Pokemon0Image, CurrentTrainer.BagPokemons[0], InPokemon == CurrentTrainer.BagPokemons[0]);
        SetPokemonSprite(Pokemon1Image, CurrentTrainer.BagPokemons[1], InPokemon == CurrentTrainer.BagPokemons[1]);
        SetPokemonSprite(Pokemon2Image, CurrentTrainer.BagPokemons[2], InPokemon == CurrentTrainer.BagPokemons[2]);
        SetPokemonSprite(Pokemon3Image, CurrentTrainer.BagPokemons[3], InPokemon == CurrentTrainer.BagPokemons[3]);
        SetPokemonSprite(Pokemon4Image, CurrentTrainer.BagPokemons[4], InPokemon == CurrentTrainer.BagPokemons[4]);
        SetPokemonSprite(Pokemon5Image, CurrentTrainer.BagPokemons[5], InPokemon == CurrentTrainer.BagPokemons[5]);
        CurrentPokemon = InPokemon;
        BagPokemonOverrideData OverrideData = new BagPokemonOverrideData();
        InfoUI.SetBagPokemon(InPokemon, false, OverrideData);
    }

    public void OnPokemonClick(int Index)
    {
        ChoosePokemon(CurrentTrainer.BagPokemons[Index]);
    }

    public void UpdatePlayerInfo()
    {
        OnChangeFirstPokemon(FirstPokemonIndex);
        SetPokemonSprite(PlayerPokemon0Image, PlayerTrainer.BagPokemons[0], true);
        SetPokemonSprite(PlayerPokemon1Image, PlayerTrainer.BagPokemons[1], true);
        SetPokemonSprite(PlayerPokemon2Image, PlayerTrainer.BagPokemons[2], true);
        SetPokemonSprite(PlayerPokemon3Image, PlayerTrainer.BagPokemons[3], true);
        SetPokemonSprite(PlayerPokemon4Image, PlayerTrainer.BagPokemons[4], true);
        SetPokemonSprite(PlayerPokemon5Image, PlayerTrainer.BagPokemons[5], true);
        PlayerTrainerImage.sprite = PlayerTrainer.TrainerSprite;
    }
}
