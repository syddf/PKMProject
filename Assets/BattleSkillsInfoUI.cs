using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class BattleSkillsInfoUI : MonoBehaviour
{
    public TextMeshProUGUI Skill1Text;
    public TextMeshProUGUI Skill2Text;
    public TextMeshProUGUI Skill3Text;
    public TextMeshProUGUI Skill4Text;
    public TypeUI Skill1Icon;
    public TypeUI Skill2Icon;
    public TypeUI Skill3Icon;
    public TypeUI Skill4Icon;

    public void UpdateUI(BattlePokemon InPokemon)
    {
        BaseSkill Skill1 = InPokemon.GetReferenceSkill()[0];
        BaseSkill Skill2 = InPokemon.GetReferenceSkill()[1];
        BaseSkill Skill3 = InPokemon.GetReferenceSkill()[2];
        BaseSkill Skill4 = InPokemon.GetReferenceSkill()[3];
        
        Skill1Icon.SetType(Skill1.GetSkillType(InPokemon));
        Skill1Text.text = Skill1.GetSkillName();
                
        Skill2Icon.SetType(Skill2.GetSkillType(InPokemon));
        Skill2Text.text = Skill2.GetSkillName();
        
        Skill3Icon.SetType(Skill3.GetSkillType(InPokemon));
        Skill3Text.text = Skill3.GetSkillName();
        
        Skill4Icon.SetType(Skill4.GetSkillType(InPokemon));
        Skill4Text.text = Skill4.GetSkillName();
    }
}
