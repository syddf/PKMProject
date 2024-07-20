using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EDSignalReceiver : ParameterizedSignalReceiver
{
    public GameObject InnerObj;
    public GameObject EndObj;
    public GameObject AnimObj;
    public EDUI EDCanvasUI;
    public EDUI EDPictureUI;
    public EDMessage EDMessage;
    public Image EDAvator;
    public Image EDPicture;
    public static Dictionary<string, Sprite> EDSprites = new Dictionary<string, Sprite>();
    public static Dictionary<string, Sprite> AvatorSprites = new Dictionary<string, Sprite>();
    public void Start()
    {
        Sprite[] EDsprites = Resources.LoadAll<Sprite>("UI/ED");

        foreach (Sprite sprite in EDsprites)
        {
            EDSprites.Add(sprite.name, sprite);
        }

        Sprite[] Avatorsprites = Resources.LoadAll<Sprite>("UI/TrainerAvator");

        foreach (Sprite sprite in Avatorsprites)
        {
            AvatorSprites.Add(sprite.name, sprite);
        }
    }
    public override void Process(SignalWithParams InSignal)
    {
        string Command = InSignal.GetParamValue("Command");
        string Param1 = InSignal.GetParamValue("Param1");
        string Param2 = InSignal.GetParamValue("Param2");
        if(Command == "Start")
        {
            EDCanvasUI.FadeIn();
            EDPictureUI.FadeIn();
            EDPicture.sprite = EDSprites[Param1];
            EDAvator.sprite = AvatorSprites[Param2];
        }
        else if(Command == "FadeOut")
        {
            EDCanvasUI.FadeOut();
            EDPictureUI.FadeOut();
        }
        else if(Command == "FadeIn")
        {
            EDCanvasUI.FadeIn();
            EDPictureUI.FadeIn();
            EDPicture.sprite = EDSprites[Param1];
            EDAvator.sprite = AvatorSprites[Param2];
        }
        else if(Command == "PictureFadeOut")
        {
            EDPictureUI.FadeOut();
        }
        else if(Command == "PictureFadeIn")
        {
            EDPictureUI.FadeIn();
            EDPicture.sprite = EDSprites[Param1];
        }
        else if(Command == "ShowMessage")
        {
            EDMessage.ShowMessage(Param1);
        }
        else if(Command == "End")
        {
            AnimObj.SetActive(false);
            InnerObj.SetActive(false);
            //EndObj.SetActive(false);
        }
        else if(Command == "FadeAllOut")
        {
            GameObject.Find("NaniBridge").GetComponent<SubObjects>().SubObject3.GetComponent<DelayActive>().Delay();
            GameObject.Find("NaniBridge").GetComponent<SubObjects>().SubObject4.GetComponent<ChapterUI>().UpdateUI();
            GameObject.Find("NaniBridge").GetComponent<SubObjects>().SubObject2.GetComponent<FadeUI>().TransitionAnimation();
        }
    }

    public void OnEnable()
    {
        InnerObj.SetActive(true);
        AnimObj.SetActive(true);
    }
}
