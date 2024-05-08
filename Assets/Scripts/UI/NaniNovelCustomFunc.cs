using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using Naninovel;
using Naninovel.Commands;

[CommandAlias("finishStory")]
public class NaniNovelCustomFunc : Command
{
    public StringParameter ChapterIndex;
    public override UniTask ExecuteAsync (AsyncToken asyncToken = default)
    {
        GameObject SavedDataObj = GameObject.Find("SavedData");
        SavedDataObj.GetComponent<SavedData>().SavedPlayerData.MainChapterProgress[int.Parse(ChapterIndex)] = EProgress.FinishStory;
        GameObject.Find("NaniBridge").GetComponent<SubObjects>().SubObject3.GetComponent<DelayActive>().Delay();
        GameObject.Find("NaniBridge").GetComponent<SubObjects>().SubObject4.GetComponent<ChapterUI>().UpdateUI();
        GameObject.Find("NaniBridge").GetComponent<SubObjects>().SubObject2.GetComponent<FadeUI>().TransitionAnimation();
        return UniTask.CompletedTask;
    }
}