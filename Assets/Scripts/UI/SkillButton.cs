using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using UnityEngine.EventSystems;
public class SkillButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public BattleManager g_BattleManager;
    public BaseSkill ReferenceSkill;
    public BattlePokemon ReferencePokemon;
    private Button UIButton;
    public TextMeshProUGUI SkillNameText;
    public TextMeshProUGUI PPText;
    public GameObject PhysicalObj;
    public GameObject SpecialObj;
    public GameObject StatusObj;
    public TypeUI TypeUI;
    public AudioSource Audio;
    private bool Forbidden;
    public Color NormalColor;
    public Color ForbiddenColor;
    public Toggle MegaToggle;
    public SkillDescUI ReferenceDescUI;
    public bool IsStruggle;
    public void OnPointerEnter(PointerEventData eventData)
    {
        ReferenceDescUI.SetSkill(ReferenceSkill);
        if (eventData.pointerEnter.gameObject == this.gameObject)
        {
            ReferenceDescUI.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerEnter.gameObject == this.gameObject)
        {
            ReferenceDescUI.gameObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UIButton = this.GetComponent<Button>();
        Audio = this.GetComponent<AudioSource>();
        Forbidden = false;
        if(IsStruggle)
        {
            this.GetComponent<Button>().onClick.AddListener(() => ButtonClicked());
        }
    }

    public void Init(BattleManager InManager, BaseSkill InReferenceSkill, BattlePokemon InReferencePokemon, bool InForbidden)
    {
        g_BattleManager = InManager;
        ReferenceSkill = InReferenceSkill;
        ReferencePokemon = InReferencePokemon;
        Forbidden = InForbidden;
        UpdateButtonUI();
    }

    public void UpdateButtonUI()
    {
        this.GetComponent<Image>().color = NormalColor;
        if(Forbidden)
        {
            this.GetComponent<Image>().color = ForbiddenColor;
        }
        else
        {
            this.GetComponent<Button>().onClick.AddListener(() => ButtonClicked());
        }
        SkillNameText.text = ReferenceSkill.GetSkillName();
        int CurPP = ReferencePokemon.GetSkillPP(ReferenceSkill);
        int MaxPP = ReferenceSkill.GetPP();
        PPText.text = "" + CurPP + "/" + MaxPP;
        PhysicalObj.SetActive(false);
        SpecialObj.SetActive(false);
        StatusObj.SetActive(false);
        ESkillClass SkillClass = ReferenceSkill.GetSkillClass();

        EType SkillType = ReferenceSkill.GetSkillType(ReferencePokemon);
        TypeUI.SetType(SkillType);
        
        if(SkillClass == ESkillClass.PhysicalMove)
        {
            PhysicalObj.SetActive(true);
        }
        if(SkillClass == ESkillClass.SpecialMove)
        {
            SpecialObj.SetActive(true);
        }
        if(SkillClass == ESkillClass.StatusMove)
        {
            StatusObj.SetActive(true);
        }
    }

    void ButtonClicked()
    {
        if(!this.Forbidden)
        {
            Audio.Play();
            g_BattleManager.OnUseSkill(ReferenceSkill, ReferencePokemon, MegaToggle.isOn);
        }
    }
}
