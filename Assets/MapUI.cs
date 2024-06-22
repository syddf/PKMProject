using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUI : MonoBehaviour
{
    public SavedData PlayerData;
    public GameObject Chapter0;
    public List<GameObject> MainChapterList = new List<GameObject>();
    public GameObject Side1;
    public GameObject Side2;
    public GameObject Side3;
    public GameObject Side4;
    void EnableAnimatorsRecursively(GameObject obj)
    {
        Animator animator = obj.GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = true;
        }
 
        foreach (Transform child in obj.transform)
        {
            DisableAnimatorsRecursively(child.gameObject);
        }
    }    
    void DisableAnimatorsRecursively(GameObject obj)
    {
        Animator animator = obj.GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = false;
        }
 
        foreach (Transform child in obj.transform)
        {
            DisableAnimatorsRecursively(child.gameObject);
        }
    }
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

        int FinishedChapter = 0;
        for(int MainIndex = 1; MainIndex <= 9; MainIndex++)
        {
            EnableAnimatorsRecursively(MainChapterList[MainIndex - 1]);
            if(PlayerData.SavedPlayerData.MainChapterProgress[MainIndex] == EProgress.FinishAllBattle)
            {
                DisableAnimatorsRecursively(MainChapterList[MainIndex - 1]);
            }
            if(PlayerData.SavedPlayerData.MainChapterProgress[MainIndex] == EProgress.FinishAllBattle)
            {
                FinishedChapter = FinishedChapter + 1;
            }
        }

        Side1.SetActive(false);
        Side2.SetActive(false);
        Side3.SetActive(false);
        Side4.SetActive(false);
        if(FinishedChapter >= 3)
        {
            if(PlayerData.SavedPlayerData.MainChapterProgress[10] != EProgress.FinishAllBattle)
            {
                Side1.SetActive(true);
            }
            else if(PlayerData.SavedPlayerData.MainChapterProgress[11] != EProgress.FinishAllBattle)
            {
                Side2.SetActive(true);
            }
            else if(PlayerData.SavedPlayerData.MainChapterProgress[12] != EProgress.FinishAllBattle)
            {
                Side3.SetActive(true);
            }
            else if(PlayerData.SavedPlayerData.MainChapterProgress[13] != EProgress.FinishAllBattle)
            {
                Side4.SetActive(true);
            }
        }
    }
    public void OnEnable()
    {
        UpdateUI();
    }
}
