using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
public class SetTrainerSkillInfo : MonoBehaviour
{
    public TextMeshProUGUI TrainerSkillName;
    public TextMeshProUGUI TrainerSkillDescription;
    public void SetBattleTrainer(PokemonTrainer InTrainer)
    {
        TrainerSkillName.text = InTrainer.TrainerSkill.GetSkillName();
        TrainerSkillDescription.text = InTrainer.TrainerSkill.GetSkillDescription();
    }
}
