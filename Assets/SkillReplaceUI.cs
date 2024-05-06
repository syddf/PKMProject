using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using UnityEngine.EventSystems;
public class SkillReplaceUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TypeUI SkillTypeUI;
    public GameObject PhysicalObj;
    public GameObject SpecialObj;
    public GameObject StatusObj;
    public BaseSkill ReferenceSkill;
    public Image Background;
    public TextMeshProUGUI SkillNameText;
    public GameObject SkillDescObj;
    public Image RectImg;

    public void Highlight()
    {
        RectImg.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
    }
    public void Reset()
    {
        RectImg.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerEnter.gameObject == Background.gameObject)
        {
            if(SkillDescObj)
            {
                SkillDescObj.GetComponent<SkillDescUI>().SetSkill(ReferenceSkill);
                SkillDescObj.SetActive(true);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerEnter.gameObject == Background.gameObject)
        {
            if(SkillDescObj)
            {
                SkillDescObj.SetActive(false);
            }
        }
    }

    public void SetSkill(BaseSkill InSkill)
    {
        ReferenceSkill = InSkill;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if(ReferenceSkill)
        {
            this.gameObject.SetActive(true);
            SkillTypeUI.SetType(ReferenceSkill.GetOriginSkillType());
            PhysicalObj.SetActive(ReferenceSkill.GetSkillClass() == ESkillClass.PhysicalMove);
            SpecialObj.SetActive(ReferenceSkill.GetSkillClass() == ESkillClass.SpecialMove);
            StatusObj.SetActive(ReferenceSkill.GetSkillClass() == ESkillClass.StatusMove);
            Background.color = SkillTypeUI.TypeColors[(int)ReferenceSkill.GetOriginSkillType()];
            Background.color = new Color(Background.color.r, Background.color.g, Background.color.b, 0.5f);
            SkillNameText.text = ReferenceSkill.GetSkillName();
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}
