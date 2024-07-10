using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using Naninovel;
using Naninovel.Commands;
public class ChapterUI : MonoBehaviour
{
    public List<Animator> DisableAnimators;
    public List<Button> DisableButtons;
    public TextMeshProUGUI PlaceText;
    private BattleConfig BattleData;
    public Image Trainer1Img;
    public Image Trainer2Img;
    public SavedData PlayerData;
    private int CurrentChapterIndex;

    public GameObject PreviewBattle1Obj;
    public GameObject PreviewBattle2Obj;
    public GameObject BeginBattle1Obj;
    public GameObject BeginBattle2Obj;
    public GameObject ReturnObj;
    public GameObject BeginStoryObj;
    public GameObject FinishObj;
    public GameObject MapObj;

    public BattleMenuUI ReferencePreviewUI;
    public BattleMenuUI ReferenceBattleMenuUI;
    public FadeUI TransitionUI;
    private GameObject CacheNaniObj = null;
    public TeamEditUI ReferenceTeamEditUI;
    public Material FloorMaterial;
    IEnumerator DisableObjectAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        MapObj.SetActive(false);
    }
    public void OnFinishBattle()
    {
        if(PlayerData.SavedPlayerData.MainChapterProgress[CurrentChapterIndex] == EProgress.FinishAllBattle)
        {
            this.gameObject.SetActive(false);
        }
        UpdateUI();
    }
    public void OnClickBeginStory()
    {
        if(CacheNaniObj == null)
        {
            GameObject NaniObj = GameObject.Find("Naninovel<Runtime>");
            NaniObj.SetActive(true);
            CacheNaniObj = NaniObj;
        }
        else
        {
            CacheNaniObj.SetActive(true);
        }
        BattleData.gameObject.GetComponent<PlayScript>().Play();
        StartCoroutine(DisableObjectAfterDelay());
        TransitionUI.TransitionAnimation();
    }
    public void SetBattleConfig(int ChapterIndex)
    {
        BattleData = GameObject.Find("BattleConfig/Chapter" + ChapterIndex.ToString()).GetComponent<BattleConfig>();
        CurrentChapterIndex = ChapterIndex;
        UpdateUI();
        RenderSettings.skybox = BattleData.SkyBoxMaterial;
        DynamicGI.UpdateEnvironment();
        FloorMaterial.SetFloat("_FloorH", BattleData.FloorH);
    }
    public void UpdateUI()
    {
        PlaceText.text = BattleData.PlaceName;
        Trainer1Img.sprite = BattleData.EnemyTrainer1.TrainerSprite;
        FinishObj.SetActive(false);
        if(BattleData.EnemyTrainer2)
        {
            Trainer2Img.gameObject.SetActive(true);
            Trainer2Img.sprite = BattleData.EnemyTrainer2.TrainerSprite;
        }
        else
        {
            Trainer2Img.gameObject.SetActive(false);
        }
        if(PlayerData.SavedPlayerData.MainChapterProgress[CurrentChapterIndex] == EProgress.New)
        {
            if(BattleData.EnemyTrainer2)
            {
                PreviewBattle2Obj.SetActive(true);
            }
            else
            {
                PreviewBattle2Obj.SetActive(false);
            }
            PreviewBattle1Obj.SetActive(true);
            BeginBattle1Obj.SetActive(false);
            BeginBattle2Obj.SetActive(false);
            ReturnObj.SetActive(true);
            BeginStoryObj.SetActive(true);
        }
        else if(PlayerData.SavedPlayerData.MainChapterProgress[CurrentChapterIndex] == EProgress.FinishStory)
        {
            if(BattleData.EnemyTrainer2 == null)
            {
                BeginBattle2Obj.SetActive(false);
            }
            else
            {
                BeginBattle2Obj.SetActive(true);
            }
            PreviewBattle1Obj.SetActive(false);
            PreviewBattle2Obj.SetActive(false);
            BeginBattle1Obj.SetActive(true);
            ReturnObj.SetActive(true);
            BeginStoryObj.SetActive(false);
        }
        else if(PlayerData.SavedPlayerData.MainChapterProgress[CurrentChapterIndex] == EProgress.FinishBattle1)
        {
            PreviewBattle1Obj.SetActive(false);
            PreviewBattle2Obj.SetActive(false);
            BeginBattle1Obj.SetActive(false);
            BeginBattle2Obj.SetActive(true);
            ReturnObj.SetActive(true);
            BeginStoryObj.SetActive(false);
        }
        else if(PlayerData.SavedPlayerData.MainChapterProgress[CurrentChapterIndex] == EProgress.FinishBattle2)
        {
            PreviewBattle1Obj.SetActive(false);
            PreviewBattle2Obj.SetActive(false);
            BeginBattle1Obj.SetActive(true);
            BeginBattle2Obj.SetActive(false);
            ReturnObj.SetActive(true);
            BeginStoryObj.SetActive(false);
        }
        else if(PlayerData.SavedPlayerData.MainChapterProgress[CurrentChapterIndex] == EProgress.FinishAllBattle)
        {
            PreviewBattle1Obj.SetActive(false);
            PreviewBattle2Obj.SetActive(false);
            BeginBattle1Obj.SetActive(false);
            BeginBattle2Obj.SetActive(false);
            ReturnObj.SetActive(true);
            BeginStoryObj.SetActive(false);
            FinishObj.SetActive(true);
        }
    }
    public void OnEnable()
    {
        foreach(var DisableAnim in DisableAnimators)
        {
            DisableAnim.enabled = false;
            DisableAnim.gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }

        foreach(var DisableBtn in DisableButtons)
        {
            DisableBtn.enabled = false;
        }
    }

    public void OnDisable()
    {
        foreach(var DisableAnim in DisableAnimators)
        {
            DisableAnim.enabled = true;
        }

        foreach(var DisableBtn in DisableButtons)
        {
            DisableBtn.enabled = true;
        }
    }

    public void OnClickPreview1()
    {
        this.gameObject.SetActive(false);
        ReferencePreviewUI.gameObject.SetActive(true);
        ReferencePreviewUI.SetEnemyPokemonTrainer(BattleData.EnemyTrainer1);
        ReferencePreviewUI.SetSpecialRule(BattleData.SpecialRule1);
    }
    public void OnClickPreview2()
    {
        this.gameObject.SetActive(false);
        ReferencePreviewUI.gameObject.SetActive(true);
        ReferencePreviewUI.SetEnemyPokemonTrainer(BattleData.EnemyTrainer2);        
        ReferencePreviewUI.SetSpecialRule(BattleData.SpecialRule2);
    }

    public void OnClickBeginBattle1()
    {
        this.gameObject.SetActive(false);
        ReferenceBattleMenuUI.gameObject.SetActive(true);
        ReferenceBattleMenuUI.ChapterIndex = CurrentChapterIndex;
        ReferenceTeamEditUI.ChapterIndex = CurrentChapterIndex;
        ReferenceBattleMenuUI.IsFirstBattle = true;
        ReferenceBattleMenuUI.SetEnemyPokemonTrainer(BattleData.EnemyTrainer1);
        ReferenceBattleMenuUI.SetPlayerPokemonTrainer(BattleData.PlayerTrainer);
        ReferenceBattleMenuUI.SetSpecialRule(BattleData.SpecialRule1);
    }
    public void OnClickBeginBattle2()
    {
        this.gameObject.SetActive(false);
        ReferenceBattleMenuUI.ChapterIndex = CurrentChapterIndex;
        ReferenceTeamEditUI.ChapterIndex = CurrentChapterIndex;
        ReferenceBattleMenuUI.IsFirstBattle = false;
        ReferenceBattleMenuUI.gameObject.SetActive(true);
        ReferenceBattleMenuUI.SetEnemyPokemonTrainer(BattleData.EnemyTrainer2); 
        ReferenceBattleMenuUI.SetPlayerPokemonTrainer(BattleData.PlayerTrainer);     
        ReferenceBattleMenuUI.SetSpecialRule(BattleData.SpecialRule2); 
    }
}
