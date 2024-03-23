using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUI : MonoBehaviour
{
    public GameObject SkillGroupRootObj;
    public GameObject SkillButtonPrefab;

    public Transform InPosition;
    public Transform OutPosition;
    public float Duration = 1.0f;
    private float Timer;
    private Transform TargetTransform;
    private Transform SourceTransform;
    public BattleManager BattleManager;
    private bool Play = false;
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
                NewButton.GetComponent<SkillButton>().Init(BattleManager, PokemonSkills[Index], InPokemon);

            }
        }
    }
    public void Out()
    {
        Play = true;
        Timer = 1.1f;
        TargetTransform = OutPosition;
        SourceTransform = InPosition;
    }

    public void In()
    {
        Play = true;
        Timer = 0;
        TargetTransform = InPosition;
        SourceTransform = OutPosition;
    }

    public void Update()
    {
        if(!Play)
        {
            return;
        }
        float Ratio = Timer / Duration;
        if(Ratio <= 1.0f)
        {
            Timer += Time.deltaTime;
            this.gameObject.transform.position = Vector3.Lerp(SourceTransform.position, TargetTransform.position, Ratio);
        }
        else
        {
            Play = true;
            this.gameObject.transform.position = TargetTransform.position;
        }    
    }
}
