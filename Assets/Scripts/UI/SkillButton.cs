using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public BattleManager g_BattleManager;
    public BaseSkill ReferenceSkill;
    public BattlePokemon ReferencePokemon;
    private Button UIButton;

    // Start is called before the first frame update
    void Start()
    {
        UIButton = this.GetComponent<Button>();
        UIButton.onClick.AddListener(() => ButtonClicked());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ButtonClicked()
    {
        
    }
}
