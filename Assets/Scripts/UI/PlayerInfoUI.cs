using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class PlayerInfoUI : MonoBehaviour
{
    public TextMeshProUGUI PokemonName;
    public TextMeshProUGUI LVText;
    public TextMeshProUGUI HPText;
    public GameObject Type1Obj;
    public GameObject Type2Obj;
    public GameObject Male;
    public GameObject Female;
    
    public void UpdateUI(BattlePokemon InPokemon)
    {
        LVText.text = "Lv." + InPokemon.GetLevel();
        PokemonName.text = InPokemon.GetName();
        HPText.text = InPokemon.GetHP() + "/" + InPokemon.GetMaxHP();
        EType Type1 = InPokemon.GetType1();
        EType Type2 = InPokemon.GetType2();
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
    }
}
