using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReplacePokemonInfoUI : MonoBehaviour
{
    public PokemonInfoUI PokemonInfoUI;
    public BagPokemon CurrentBagPokemon;
    public PokemonTrainer CurrentTrainer;

    public Image Pokemon0Image;
    public Image Pokemon1Image;
    public Image Pokemon2Image;
    public Image Pokemon3Image;
    public Image Pokemon4Image;
    public Image Pokemon5Image;

    public SkillReplaceUI Skill1UI;
    public SkillReplaceUI Skill2UI;
    public SkillReplaceUI Skill3UI;
    public SkillReplaceUI Skill4UI;
    public GameObject ConfirmReplaceButtonObj;
    public GameObject MegaToggleObj;
    public Toggle MegaToggle;
    public PokemonEditMainMenu ReferenceMainMenu;
    public GameObject ConfirmButton;
    public void OnClickPokemon(int Index)
    {
        if(ReferenceMainMenu.IsReplacePokemonLegal(CurrentTrainer.BagPokemons[Index]))
        {
            CurrentBagPokemon = CurrentTrainer.BagPokemons[Index];
            MegaToggleObj.SetActive(CurrentBagPokemon.GetCanMega());
            MegaToggle.isOn = false;
            ConfirmReplaceButtonObj.SetActive(true);
            UpdateUI();
            ConfirmButton.SetActive(true);
        }
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

    public void ChoosePokemon(BagPokemon InPokemon)
    {
        SetPokemonSprite(Pokemon0Image, CurrentTrainer.BagPokemons[0], InPokemon == CurrentTrainer.BagPokemons[0]);
        SetPokemonSprite(Pokemon1Image, CurrentTrainer.BagPokemons[1], InPokemon == CurrentTrainer.BagPokemons[1]);
        SetPokemonSprite(Pokemon2Image, CurrentTrainer.BagPokemons[2], InPokemon == CurrentTrainer.BagPokemons[2]);
        SetPokemonSprite(Pokemon3Image, CurrentTrainer.BagPokemons[3], InPokemon == CurrentTrainer.BagPokemons[3]);
        SetPokemonSprite(Pokemon4Image, CurrentTrainer.BagPokemons[4], InPokemon == CurrentTrainer.BagPokemons[4]);
        SetPokemonSprite(Pokemon5Image, CurrentTrainer.BagPokemons[5], InPokemon == CurrentTrainer.BagPokemons[5]);
        BagPokemonOverrideData OverrideData = new BagPokemonOverrideData();
        PokemonInfoUI.SetBagPokemon(InPokemon, MegaToggle.isOn && CurrentBagPokemon.GetCanMega(), OverrideData);
    }

    public void ResetUI()
    {
        CurrentBagPokemon = null;
        UpdateUI();
        ConfirmButton.SetActive(false);
    }
    public void SetCurrentTrainer(PokemonTrainer InTrainer)
    {
        CurrentTrainer = InTrainer;
        ResetUI();
    }

    public void UpdateUI()
    {
        if(CurrentBagPokemon == null)
        {
            PokemonInfoUI.gameObject.SetActive(false);
            Skill1UI.gameObject.SetActive(false);
            Skill2UI.gameObject.SetActive(false);
            Skill3UI.gameObject.SetActive(false);
            Skill4UI.gameObject.SetActive(false);

            SetPokemonSprite(Pokemon0Image, CurrentTrainer.BagPokemons[0], false);
            SetPokemonSprite(Pokemon1Image, CurrentTrainer.BagPokemons[1], false);
            SetPokemonSprite(Pokemon2Image, CurrentTrainer.BagPokemons[2], false);
            SetPokemonSprite(Pokemon3Image, CurrentTrainer.BagPokemons[3], false);
            SetPokemonSprite(Pokemon4Image, CurrentTrainer.BagPokemons[4], false);
            SetPokemonSprite(Pokemon5Image, CurrentTrainer.BagPokemons[5], false);
        }
        else
        {
            Skill1UI.gameObject.SetActive(true);
            Skill2UI.gameObject.SetActive(true);
            Skill3UI.gameObject.SetActive(true);
            Skill4UI.gameObject.SetActive(true);
            PokemonInfoUI.gameObject.SetActive(true);
            ChoosePokemon(CurrentBagPokemon);
            Skill1UI.SetSkill(CurrentBagPokemon.GetBaseSkill(0));
            Skill2UI.SetSkill(CurrentBagPokemon.GetBaseSkill(1));
            Skill3UI.SetSkill(CurrentBagPokemon.GetBaseSkill(2));
            Skill4UI.SetSkill(CurrentBagPokemon.GetBaseSkill(3));
        }
    }
}
