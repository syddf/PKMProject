using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
public class SkillButton : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        UIButton = this.GetComponent<Button>();
        UIButton.onClick.AddListener(() => ButtonClicked());
    }

    public void Init(BattleManager InManager, BaseSkill InReferenceSkill, BattlePokemon InReferencePokemon)
    {
        g_BattleManager = InManager;
        ReferenceSkill = InReferenceSkill;
        ReferencePokemon = InReferencePokemon;
        UpdateButtonUI();
    }

    public void UpdateButtonUI()
    {
        SkillNameText.text = ReferenceSkill.GetSkillName();
        int CurPP = ReferencePokemon.GetSkillPP(ReferenceSkill);
        int MaxPP = ReferenceSkill.GetPP();
        PPText.text = "" + CurPP + "/" + MaxPP;
        PhysicalObj.SetActive(false);
        SpecialObj.SetActive(false);
        StatusObj.SetActive(false);
        ESkillClass SkillClass = ReferenceSkill.GetSkillClass();

        EType SkillType = ReferenceSkill.GetSkillType();
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
        g_BattleManager.OnUseSkill(ReferenceSkill, ReferencePokemon);
    }
}
