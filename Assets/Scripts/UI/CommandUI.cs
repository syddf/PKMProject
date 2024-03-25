using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUI : MonoBehaviour
{
    public GameObject SkillGroupRootObj;
    public GameObject SkillButtonPrefab;
    public GameObject SwitchGroupRootObj;
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
        SwitchGroupRootObj.SetActive(false);
        SkillGroupRootObj.SetActive(true);
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

    public void GenerateNewSwitchGroup(PokemonTrainer InTrainer)
    {
        SwitchGroupRootObj.SetActive(true);
        SkillGroupRootObj.SetActive(false);
        BattlePokemon[] Pokemons = InTrainer.BattlePokemons;
        SubObjects SubScript = SwitchGroupRootObj.GetComponent<SubObjects>();
        SubScript.SubObject1.SetActive(false);
        SubScript.SubObject2.SetActive(false);
        SubScript.SubObject3.SetActive(false);
        SubScript.SubObject4.SetActive(false);
        SubScript.SubObject5.SetActive(false);
        SubScript.SubObject6.SetActive(false);
        if(Pokemons[0] != null) SubScript.SubObject1.SetActive(true);
        if(Pokemons[1] != null) SubScript.SubObject2.SetActive(true);
        if(Pokemons[2] != null) SubScript.SubObject3.SetActive(true);
        if(Pokemons[3] != null) SubScript.SubObject4.SetActive(true);
        if(Pokemons[4] != null) SubScript.SubObject5.SetActive(true);
        if(Pokemons[5] != null) SubScript.SubObject6.SetActive(true);
        SubScript.SubObject1.GetComponent<SwitchButton>().UpdateSprite(Pokemons[0], BattleManager);
        SubScript.SubObject2.GetComponent<SwitchButton>().UpdateSprite(Pokemons[1], BattleManager);
        SubScript.SubObject3.GetComponent<SwitchButton>().UpdateSprite(Pokemons[2], BattleManager);
        SubScript.SubObject4.GetComponent<SwitchButton>().UpdateSprite(Pokemons[3], BattleManager);
        SubScript.SubObject5.GetComponent<SwitchButton>().UpdateSprite(Pokemons[4], BattleManager);
        SubScript.SubObject6.GetComponent<SwitchButton>().UpdateSprite(Pokemons[5], BattleManager);
    }
}
