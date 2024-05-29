using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainerListEntry : MonoBehaviour
{
    public Image Border;
    public Image Trainer;
    public TeamEditUI TeamEditWindow;
    public GameObject ReferenceTrainerObject;
    private string TrainerName;
    public GameObject TagObj;
    public SetTrainerSkillInfo ReferenceUI;
    public void UpdateTag(string BattleTrainerName)
    {
        TagObj.SetActive(TrainerName == BattleTrainerName);
    }
    
    public void InitEntry(string InTrainerName)
    {
        TrainerName = InTrainerName;
        ReferenceTrainerObject = GameObject.Find("SingleBattle/AllTrainers/" + TrainerName);
        ReferenceUI.SetBattleTrainer(ReferenceTrainerObject.GetComponent<PokemonTrainer>());
    }
    public void Reset()
    {
        Border.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }
    public void OnClick()
    {
        TeamEditWindow.OnTrainerClick(ReferenceTrainerObject.GetComponent<PokemonTrainer>());
        Border.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
    }
}
