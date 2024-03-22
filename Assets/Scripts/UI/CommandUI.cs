using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUI : MonoBehaviour
{
    public GameObject SkillGroupRootObj;
    public GameObject SkillButtonPrefab;

    public BattleUI BattleUIManager;
    public void GenerateNewSkillGroup(BattlePokemon InPokemon)
    {        
        foreach (Transform child in SkillGroupRootObj.transform)
        {
            Destroy(child.gameObject);
        }
        BaseSkill[] PokemonSkills = InPokemon.GetReferenceSkill();
        for(int Index = 0; Index < 4; Index ++)
        {
            if(PokemonSkills[Index] != null)
            {
                GameObject NewButton = Instantiate(SkillButtonPrefab, new Vector3(0, 0, 0), Quaternion.identity, SkillGroupRootObj.transform);
                NewButton.GetComponent<SkillButton>().Init(null, PokemonSkills[Index], InPokemon);

            }
        }
    }
}
