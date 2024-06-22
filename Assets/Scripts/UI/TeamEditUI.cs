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
    public int ChapterIndex;
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
        if(ChapterIndex >= 10)
        {
            if(ChapterIndex == 10 || ChapterIndex == 11 || ChapterIndex == 13)
            {
                GameObject newObject = Instantiate(TrainerEntryPrefab, ContentsRoot.transform.position, Quaternion.identity);
                newObject.transform.SetParent(ContentsRoot.transform);
                newObject.GetComponent<TrainerListEntry>().TeamEditWindow = this.GetComponent<TeamEditUI>();

                string spritePath = "UI/TrainerAvator/小智";
                Sprite sprite = Resources.Load<Sprite>(spritePath);
                newObject.GetComponent<TrainerListEntry>().Trainer.sprite = sprite;
                newObject.GetComponent<TrainerListEntry>().InitEntry("小智");
                newObject.GetComponent<TrainerListEntry>().UpdateTag("小智");
            }
        }
        else
        {
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
}
