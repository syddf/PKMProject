using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class HistoryUI : MonoBehaviour
{
    public HistoryEntry Entry1;
    public HistoryEntry Entry2;
    public HistoryEntry Entry3;
    public HistoryEntry Entry4;
    public HistoryEntry Entry5;
    public HistoryEntry Entry6;
    public SavedData PlayerData;
    private int CurPage;
    private int TotalPage;
    public TextMeshProUGUI PageText;

    public void OnEnable()
    {
        CurPage = 1;
        TotalPage = (PlayerData.SavedPlayerData.HistoryList.Count + 5) / 6;
        if(TotalPage == 0)
        {
            CurPage = 0;
        }
        UpdateUI();
    }
    public void UpdateEntry(int Index, HistoryEntry TargetEntry)
    {
        if(Index >= PlayerData.SavedPlayerData.HistoryList.Count)
        {
            TargetEntry.gameObject.SetActive(false);
        }
        else
        {
            TargetEntry.gameObject.SetActive(true);
            TargetEntry.UpdateUI(PlayerData.SavedPlayerData.HistoryList[Index]);
        }
    }
    public void UpdateUI()
    {
        PageText.text = CurPage.ToString() + "/" + TotalPage.ToString(); 
        if(CurPage == 0)
        {
            Entry1.gameObject.SetActive(false);
            Entry2.gameObject.SetActive(false);
            Entry3.gameObject.SetActive(false);
            Entry4.gameObject.SetActive(false);
            Entry5.gameObject.SetActive(false);
            Entry6.gameObject.SetActive(false);
            return;
        }
        int Start = (CurPage - 1) * 6;
        UpdateEntry(Start + 0, Entry1);
        UpdateEntry(Start + 1, Entry2);
        UpdateEntry(Start + 2, Entry3);
        UpdateEntry(Start + 3, Entry4);
        UpdateEntry(Start + 4, Entry5);
        UpdateEntry(Start + 5, Entry6);

    }

    public void NextPage()
    {
        if(CurPage < TotalPage)
        {
            CurPage = CurPage + 1;
            UpdateUI();
        }
    }

    public void PrevPage()
    {
        if(CurPage > 1)
        {
            CurPage = CurPage - 1;
            UpdateUI();
        }
    }
}
