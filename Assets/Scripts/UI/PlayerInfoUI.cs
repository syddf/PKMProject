using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class PlayerInfoUI : MonoBehaviour
{
    public TextMeshProUGUI PokemonName;
    public TextMeshProUGUI LVText;
    public DamageUIAnimation HPUI;
    public TextMeshProUGUI HPText;
    public GameObject Type1Obj;
    public GameObject Type2Obj;
    public GameObject Male;
    public GameObject Female;

    public GameObject Poison;
    public GameObject Paralysis;
    public GameObject Burn;
    public GameObject Frostbite;
    public GameObject Drowsy;
    public Image[] PkBallImages = new Image[6];
    public Color DefeatedPokemonColor;
    public Color NormalPokemonColor;
    public Color NoPokemonColor;
    private BattlePokemon ReferencePokemon;
    public void UpdateUI(BattlePokemon InPokemon, PokemonTrainer InTrainer)
    {
        LVText.text = "Lv." + InPokemon.GetLevel();
        PokemonName.text = InPokemon.GetName();
        HPText.text = InPokemon.GetHP() + "/" + InPokemon.GetMaxHP();
        EType Type1 = InPokemon.GetType1(null, null, null);
        EType Type2 = InPokemon.GetType2(null, null, null);
        if(Type1 != EType.None)
        {
            Type1Obj.SetActive(true);
            Type1Obj.GetComponent<TypeUI>().SetType(Type1);
        }
        Type2Obj.SetActive(false);
        if(Type2 != EType.None)
        {
            Type2Obj.SetActive(true);
            Type2Obj.GetComponent<TypeUI>().SetType(Type2);
        }

        Male.SetActive(false);
        Female.SetActive(false);
        PokemonGender gender = InPokemon.GetGender();
        if(gender == PokemonGender.Male)
        {
            Male.SetActive(true);
        }
        if(gender == PokemonGender.Female)
        {
            Female.SetActive(true);
        }
        ReferencePokemon = InPokemon;
        HPUI.SetPokemon(ReferencePokemon);
        HPUI.SetColor(InPokemon.GetHP());

        for(int Index = 0; Index < 6; Index++)
        {
            if(InTrainer.BattlePokemons[Index] == null)
            {
                PkBallImages[Index].color = NoPokemonColor;
            }
            else if(InTrainer.BattlePokemons[Index].IsDead())
            {
                PkBallImages[Index].color = DefeatedPokemonColor;
            }
            else
            {
                PkBallImages[Index].color = NormalPokemonColor;
            }
        }

        Poison.SetActive(ReferencePokemon.HasStatusChange(EStatusChange.Poison));
        Paralysis.SetActive(ReferencePokemon.HasStatusChange(EStatusChange.Paralysis));
        Burn.SetActive(ReferencePokemon.HasStatusChange(EStatusChange.Burn));
        Frostbite.SetActive(ReferencePokemon.HasStatusChange(EStatusChange.Frostbite));
        Drowsy.SetActive(ReferencePokemon.HasStatusChange(EStatusChange.Drowsy));
    }

    public void UpdateStateChange(EStatusChange InType)
    {
        Poison.SetActive(InType == EStatusChange.Poison);
        Paralysis.SetActive(InType == EStatusChange.Paralysis);
        Burn.SetActive(InType == EStatusChange.Burn);
        Frostbite.SetActive(InType == EStatusChange.Frostbite);
        Drowsy.SetActive(InType == EStatusChange.Drowsy);
    }

    public void UpdateHP(int CurHP)
    {
        HPUI.SetHP(CurHP);
    }

    public void DamageUI(int Damage)
    {
        HPUI.NewDamage(Damage, ReferencePokemon);
    }
}
