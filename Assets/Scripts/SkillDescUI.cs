using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
public class SkillDescUI : MonoBehaviour
{
    public TextMeshProUGUI PPText;
    public TextMeshProUGUI PowerText;
    public TextMeshProUGUI AccuracyText;
    public TextMeshProUGUI DescText;
    public TextMeshProUGUI NameText;
    public void SetSkill(BaseSkill InSkill)
    {
        if(!InSkill)
        {
            return;
        }
        PPText.text = InSkill.GetPP().ToString();
        AccuracyText.text = InSkill.GetAccuracy().ToString();
        PowerText.text = InSkill.GetPower().ToString();
        if(InSkill.GetPower() == 0)
        {
            PowerText.text = "/";
        }
        DescText.text = InSkill.GetSkillDesc(); 
        NameText.text = InSkill.GetSkillName(); 
    }
}
