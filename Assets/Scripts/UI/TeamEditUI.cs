using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamEditUI : MonoBehaviour
{
    public GameObject ContentsRoot;
    public GameObject TrainerEntryPrefab;
    public void OnTrainerClick()
    {
        foreach (Transform child in ContentsRoot.transform)
        {
            child.gameObject.GetComponent<TrainerListEntry>().Reset();
        }
    }

    public void OnEnable() 
    {
        foreach (Transform child in ContentsRoot.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        GameObject newObject = Instantiate(TrainerEntryPrefab, ContentsRoot.transform.position, Quaternion.identity);
        newObject.transform.SetParent(ContentsRoot.transform);
        newObject.GetComponent<TrainerListEntry>().TeamEditWindow = this.GetComponent<TeamEditUI>();
        GameObject newObject2 = Instantiate(TrainerEntryPrefab, ContentsRoot.transform.position, Quaternion.identity);
        newObject2.transform.SetParent(ContentsRoot.transform);
        newObject2.GetComponent<TrainerListEntry>().TeamEditWindow = this.GetComponent<TeamEditUI>();
    }
}
