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
    public PokemonTrainer CurrentTrainer;
    public BagPokemon CurrentPokemon;
    public PokemonInfoUI InfoUI;
    public GameObject MegaToggleObj;
    public Toggle MegaToggle;
    public PokemonTrainer PlayerTrainer;

    public Image PlayerPokemon0Image;
    public Image PlayerPokemon1Image;
    public Image PlayerPokemon2Image;
    public Image PlayerPokemon3Image;
    public Image PlayerPokemon4Image;
    public Image PlayerPokemon5Image;
    public Image PlayerTrainerImage;
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
        TrainerSprite.sprite = InTrainer.TrainerSprite;
        TrainerSkillDesc.text = InTrainer.TrainerSkill.GetSkillDescription();
        TrainerSkillName.text = InTrainer.TrainerSkill.GetSkillName();
        ChoosePokemon(CurrentTrainer.BagPokemons[0]);
    }
    public void SetPlayerPokemonTrainer(PokemonTrainer InTrainer)
    {
        PlayerTrainer = InTrainer;
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
        SetPokemonSprite(PlayerPokemon0Image, PlayerTrainer.BagPokemons[0], true);
        SetPokemonSprite(PlayerPokemon1Image, PlayerTrainer.BagPokemons[1], true);
        SetPokemonSprite(PlayerPokemon2Image, PlayerTrainer.BagPokemons[2], true);
        SetPokemonSprite(PlayerPokemon3Image, PlayerTrainer.BagPokemons[3], true);
        SetPokemonSprite(PlayerPokemon4Image, PlayerTrainer.BagPokemons[4], true);
        SetPokemonSprite(PlayerPokemon5Image, PlayerTrainer.BagPokemons[5], true);
        PlayerTrainerImage.sprite = PlayerTrainer.TrainerSprite;
    }
}
