using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUI : MonoBehaviour
{
    public SavedData PlayerData;
    public GameObject Chapter0;
    public List<GameObject> MainChapterList = new List<GameObject>();
    public void UpdateUI()
    {
        if(PlayerData.SavedPlayerData.MainChapterProgress.Count == 0)
        {
            return;
        }
        if(PlayerData.SavedPlayerData.MainChapterProgress[0] == EProgress.FinishAllBattle)
        {
            Chapter0.SetActive(false);
            foreach(var mainObj in MainChapterList)
            {
                mainObj.SetActive(true);
            }
        }
        else
        {
            Chapter0.SetActive(true);
            foreach(var mainObj in MainChapterList)
            {
                mainObj.SetActive(false);
            }
        }
    }
    public void OnEnable()
    {
        UpdateUI();
    }
}
