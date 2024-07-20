using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using Naninovel;
using Naninovel.Commands;
using System.Linq;
using UnityEngine.SceneManagement;

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
    public GameObject FloorObj;
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

    public void Start()
    {
        CacheNaniObj = GameObject.Find("Naninovel<Runtime>");
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
    public void UpdateSkyBox()
    {
        BattleData = GameObject.Find("BattleConfig/Chapter" + CurrentChapterIndex.ToString()).GetComponent<BattleConfig>();        
        RenderSettings.skybox = BattleData.SkyBoxMaterial;
        DynamicGI.UpdateEnvironment();
        MeshRenderer meshRenderer = FloorObj.GetComponent<MeshRenderer>();
        Material SelfMaterial = meshRenderer.material;
        SelfMaterial.SetFloat("_FloorH", BattleData.FloorH);
    }
    public void SetBattleConfig(int ChapterIndex)
    {
        BattleData = GameObject.Find("BattleConfig/Chapter" + ChapterIndex.ToString()).GetComponent<BattleConfig>();
        CurrentChapterIndex = ChapterIndex;
        UpdateUI();
        RenderSettings.skybox = null;
        DynamicGI.UpdateEnvironment();
    }

    public void ResetGame()
    {
        PlayerData playerData = new PlayerData();
        List<string> UseableTrainerList = new List<string>();
        UseableTrainerList.Add("希特隆");
        UseableTrainerList.Add("可尔妮");
        UseableTrainerList.Add("志米");
        UseableTrainerList.Add("雁铠");
        UseableTrainerList.Add("梅丽莎");
        UseableTrainerList.Add("库库伊");
        playerData.BattleTrainerName = "希特隆";
        playerData.UseableTrainerList = UseableTrainerList;
        playerData.OverrideData = new SerializableDictionary<string, SerializableDictionary<string, BagPokemonOverrideData>>();
        playerData.MainChapterProgress = new List<EProgress>();
        playerData.FinishAllChapter = true;
        playerData.HistoryList = new List<BattleHistory>();
        for(int Index = 0; Index <= 13; Index++)
        {
            playerData.MainChapterProgress.Add(EProgress.FinishStory);
        }
        playerData.MainChapterProgress[0] = EProgress.FinishAllBattle;
        playerData.MainChapterProgress[10] = EProgress.FinishAllBattle;
        playerData.MainChapterProgress[11] = EProgress.FinishAllBattle;
        playerData.MainChapterProgress[12] = EProgress.FinishAllBattle;
        playerData.MainChapterProgress[13] = EProgress.FinishAllBattle;
        PlayerData.SavedPlayerData = playerData;
        PlayerData.SaveData();
    }

    public void UpdateUI()
    {
        if(CurrentChapterIndex == 14)
        {
            Trainer1Img.gameObject.SetActive(false);
            Trainer2Img.gameObject.SetActive(false);
            PreviewBattle1Obj.SetActive(false);
            PreviewBattle2Obj.SetActive(false);
            BeginBattle1Obj.SetActive(false);
            BeginBattle2Obj.SetActive(false);
            ReturnObj.SetActive(true);
            BeginStoryObj.SetActive(true);
            FinishObj.SetActive(false);
            if(PlayerData.SavedPlayerData.FinishAllChapter == true)
            {
                BeginStoryObj.SetActive(false);
                FinishObj.SetActive(true);
            }
            return;
        }
        
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
        UpdateSkyBox();
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

        UpdateSkyBox();
    }
}
