using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class MapUI : MonoBehaviour
{
    public SavedData PlayerData;
    public GameObject Chapter0;
    public List<GameObject> MainChapterList = new List<GameObject>();
    public GameObject Side1;
    public GameObject Side2;
    public GameObject Side3;
    public GameObject Side4;
    public GameObject Final;
    public TextMeshProUGUI MainText;
    public TextMeshProUGUI SideText;
    public GameObject ResetButton;
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
        int FinishedBattle = 0;
        if(PlayerData.SavedPlayerData.MainChapterProgress[0] == EProgress.FinishAllBattle)
        {
            FinishedBattle = FinishedBattle + 1;
        }
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
                FinishedBattle = FinishedBattle + 2;
            }
            else if(PlayerData.SavedPlayerData.MainChapterProgress[MainIndex] == EProgress.FinishBattle1 || 
                    PlayerData.SavedPlayerData.MainChapterProgress[MainIndex] == EProgress.FinishBattle2)
            {
                FinishedBattle = FinishedBattle + 1;
            }
        }

        ResetButton.gameObject.SetActive(false);
        if(PlayerData.SavedPlayerData.FinishAllChapter == true && FinishedBattle == 19)
        {   
            ResetButton.gameObject.SetActive(true);
        }

        Side1.SetActive(false);
        Side2.SetActive(false);
        Side3.SetActive(false);
        Side4.SetActive(false);
        Final.SetActive(false);
        SideText.text = "";
        if(FinishedBattle >= 4 && PlayerData.SavedPlayerData.MainChapterProgress[10] != EProgress.FinishAllBattle)
        {
            Side1.SetActive(true);
        }
        else if(FinishedBattle >= 7 && PlayerData.SavedPlayerData.MainChapterProgress[10] == EProgress.FinishAllBattle && PlayerData.SavedPlayerData.MainChapterProgress[11] != EProgress.FinishAllBattle)
        {
            Side2.SetActive(true);
        }
        else if(FinishedBattle >= 10 && PlayerData.SavedPlayerData.MainChapterProgress[11] == EProgress.FinishAllBattle && PlayerData.SavedPlayerData.MainChapterProgress[12] != EProgress.FinishAllBattle)
        {
            Side3.SetActive(true);
        }
        else if(FinishedBattle >= 13 && PlayerData.SavedPlayerData.MainChapterProgress[12] == EProgress.FinishAllBattle && PlayerData.SavedPlayerData.MainChapterProgress[13] != EProgress.FinishAllBattle)
        {
            Side4.SetActive(true);
        }

        if(FinishedBattle >= 1 && FinishedBattle < 4 && PlayerData.SavedPlayerData.FinishAllChapter == false)
        {
            int RemainBattle = 4 - FinishedBattle;
            SideText.text = "再通关" + RemainBattle.ToString() + "场战斗解锁隐藏关卡1";
        }
        if(FinishedBattle >= 4 && FinishedBattle < 7 && PlayerData.SavedPlayerData.FinishAllChapter == false) 
        {
            int RemainBattle = 7 - FinishedBattle;
            SideText.text = "再通关" + RemainBattle.ToString() + "场战斗解锁隐藏关卡2";
        }
        if(FinishedBattle >= 7 && FinishedBattle < 10 && PlayerData.SavedPlayerData.FinishAllChapter == false)
        {
            int RemainBattle = 10 - FinishedBattle;
            SideText.text = "再通关" + RemainBattle.ToString() + "场战斗解锁隐藏关卡3";
        }
        if(FinishedBattle >= 10 && FinishedBattle < 13 && PlayerData.SavedPlayerData.FinishAllChapter == false)
        {
            int RemainBattle = 13 - FinishedBattle;
            SideText.text = "再通关" + RemainBattle.ToString() + "场战斗解锁隐藏关卡4";
        }
        MainText.text = "通关战斗：" + FinishedBattle.ToString() + "/19";

        if(FinishedBattle == 19 && PlayerData.SavedPlayerData.MainChapterProgress[13] == EProgress.FinishAllBattle && PlayerData.SavedPlayerData.FinishAllChapter == false)
        {
            EnableAnimatorsRecursively(Final);
            foreach(var ChapterObj in MainChapterList)
            {
                ChapterObj.SetActive(false);
            }
            Side1.SetActive(false);
            Side2.SetActive(false);
            Side3.SetActive(false);
            Side4.SetActive(false);
            Final.SetActive(true);

            if(PlayerData.SavedPlayerData.FinishAllChapter == true)
            {
                DisableAnimatorsRecursively(Final);
            }
        }
    }
    public void OnEnable()
    {
        UpdateUI();
    }
}
