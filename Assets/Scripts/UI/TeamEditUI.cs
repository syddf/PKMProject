using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamEditUI : MonoBehaviour
{
    public GameObject ContentsRoot;
    public GameObject TrainerEntryPrefab;
    public SavedData SaveData;
    public PokemonEditMainMenu MainMenu;
    public GameObject PokemonEditObj;
    public GameObject InfoObj;

    public void UpdateTrainerTag()
    {
        foreach (Transform child in ContentsRoot.transform)
        {
            child.gameObject.GetComponent<TrainerListEntry>().UpdateTag(SaveData.SavedPlayerData.BattleTrainerName);
        }
    }
    public void OnTrainerClick(PokemonTrainer InTrainer)
    {
        foreach (Transform child in ContentsRoot.transform)
        {
            child.gameObject.GetComponent<TrainerListEntry>().Reset();
        }
        MainMenu.SetCurrentPokemonTrainer(InTrainer);
        PokemonEditObj.SetActive(true);
        InfoObj.SetActive(false);
    }

    public void OnEnable() 
    {
        InfoObj.SetActive(true);
        foreach (Transform child in ContentsRoot.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach(string TrainerName in SaveData.SavedPlayerData.UseableTrainerList)
        {
            GameObject newObject = Instantiate(TrainerEntryPrefab, ContentsRoot.transform.position, Quaternion.identity);
            newObject.transform.SetParent(ContentsRoot.transform);
            newObject.GetComponent<TrainerListEntry>().TeamEditWindow = this.GetComponent<TeamEditUI>();

            string spritePath = "UI/TrainerAvator/" + TrainerName;
            Sprite sprite = Resources.Load<Sprite>(spritePath);
            newObject.GetComponent<TrainerListEntry>().Trainer.sprite = sprite;
            newObject.GetComponent<TrainerListEntry>().InitEntry(TrainerName);
            newObject.GetComponent<TrainerListEntry>().UpdateTag(SaveData.SavedPlayerData.BattleTrainerName);
        }
    }
}
