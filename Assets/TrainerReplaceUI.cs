using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerReplaceUI : MonoBehaviour
{
    public PokemonTrainer CurrentTrainer;
    public SavedData PlayerData;
    public GameObject EntryPrefab;
    public GameObject EntryRootObj;
    public GameObject PokemonReplaceUI;
    public ReplacePokemonInfoUI ReferencePokemonInfoUI;
    public List<PokemonTrainer> GetAllReplaceableTrainer()
    {
        List<PokemonTrainer> AllTrainers = new List<PokemonTrainer>();
        foreach(string TrainerName in PlayerData.SavedPlayerData.UseableTrainerList)
        {
            AllTrainers.Add(GameObject.Find("SingleBattle/AllTrainers/" + TrainerName).GetComponent<PokemonTrainer>());
        }
        return AllTrainers;
    }
    public void InitReplaceTrainerEntry()
    {
        foreach (Transform child in EntryRootObj.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        List<PokemonTrainer> AllTrainers = GetAllReplaceableTrainer();
        int EntryCount = (AllTrainers.Count + 2) / 3;
        for(int Index = 0; Index < EntryCount; Index++)
        {
            GameObject newObject = Instantiate(EntryPrefab, EntryRootObj.transform.position, Quaternion.identity);
            newObject.transform.SetParent(EntryRootObj.transform);
            newObject.GetComponent<ReplaceTrainerEntry>().ReferenceTrainerReplaceUI = this;
            newObject.GetComponent<ReplaceTrainerEntry>().Trainer1 = AllTrainers[Index * 3];
            newObject.GetComponent<ReplaceTrainerEntry>().Trainer2 = ((Index * 3 + 1) >= AllTrainers.Count) ? null : AllTrainers[Index * 3 + 1];
            newObject.GetComponent<ReplaceTrainerEntry>().Trainer3 = ((Index * 3 + 2) >= AllTrainers.Count) ? null : AllTrainers[Index * 3 + 2];
            newObject.GetComponent<ReplaceTrainerEntry>().UpdateUI();
        }
    }

    public void OnClickTrainer(PokemonTrainer InTrainer)
    {
        this.gameObject.SetActive(false);
        PokemonReplaceUI.SetActive(true);
        ReferencePokemonInfoUI.SetCurrentTrainer(InTrainer);
    }
}
