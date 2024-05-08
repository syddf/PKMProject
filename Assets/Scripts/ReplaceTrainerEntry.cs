using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ReplaceTrainerEntry : MonoBehaviour
{
    public PokemonTrainer Trainer1;
    public PokemonTrainer Trainer2;
    public PokemonTrainer Trainer3;
    public GameObject Trainer1Entry;
    public GameObject Trainer2Entry;
    public GameObject Trainer3Entry;
    public Image Trainer1Sprite;
    public Image Trainer2Sprite;
    public Image Trainer3Sprite;
    public TrainerReplaceUI ReferenceTrainerReplaceUI;


    public void UpdateUI()
    {
        Trainer1Entry.SetActive(Trainer1 != null);
        if(Trainer1)
            Trainer1Sprite.sprite = Trainer1.TrainerSprite;
        Trainer2Entry.SetActive(Trainer2 != null);
        if(Trainer2)
            Trainer2Sprite.sprite = Trainer2.TrainerSprite;
        Trainer3Entry.SetActive(Trainer3 != null);
        if(Trainer3)
            Trainer3Sprite.sprite = Trainer3.TrainerSprite;
    }

    public void OnClickTrainer(int Index)
    {
        if(Index == 0)
        {
            ReferenceTrainerReplaceUI.OnClickTrainer(Trainer1);
        }
        else if(Index == 1)
        {
            ReferenceTrainerReplaceUI.OnClickTrainer(Trainer2);
        }
        else
        {
            ReferenceTrainerReplaceUI.OnClickTrainer(Trainer3);
        }
    }
}
