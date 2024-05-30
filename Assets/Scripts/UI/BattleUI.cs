using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    public PlayerInfoUI PlayerInfo1; 
    public PlayerInfoUI EnemyInfo1;
    public CommandUI BattleCommandUI;
    public BattlePokemon CurrentPokemon;
    public PokemonTrainer CurrentPlayerTrainer;
    public SetTrainerSkillInfo PlayerTrainerSkillInfo;
    public SetTrainerSkillInfo EnemyTrainerSkillInfo;
    public Image PlayerTrainerImage;
    public Image EnemyTrainerImage;
    public Image InBattlePlayerTrainerImage;
    public Image InBattleEnemyTrainerImage;
    public GameObject PlayerTrainerUIObj;
    public GameObject EnemyTrainerUIObj;
    public GameObject Desc1Obj;
    public GameObject Desc2Obj;
    public GameObject Desc3Obj;
    public void DisableAllUI()
    {
        PlayerInfo1.gameObject.SetActive(false);
        EnemyInfo1.gameObject.SetActive(false);
        BattleCommandUI.gameObject.SetActive(false);
        PlayerTrainerUIObj.SetActive(false);
        EnemyTrainerUIObj.SetActive(false);
        Desc1Obj.SetActive(false);
        Desc2Obj.SetActive(false);
        Desc3Obj.SetActive(false);
    }
    public void SetCurrentPlayerTrainer(PokemonTrainer InTrainer)
    {
        CurrentPlayerTrainer = InTrainer;
    }
    public void SetCurrentBattlePokemon(BattlePokemon InPokemon)
    {
        CurrentPokemon = InPokemon;
    }

    public void SetPlayerHP(int HP)
    {
        PlayerInfo1.UpdateHP(HP);
    }
    public void SetEnemyHP(int HP)
    {
        EnemyInfo1.UpdateHP(HP);
    }
    public void SetPlayerStateChange(EStatusChange InType)
    {
        PlayerInfo1.UpdateStateChange(InType);
    }
    public void SetEnemyStateChange(EStatusChange InType)
    {
        EnemyInfo1.UpdateStateChange(InType);
    }
    public void UpdatePlayerType(BattlePokemon InPokemon)
    {
        PlayerInfo1.UpdateType(InPokemon);
    }
    public void UpdateEnemyType(BattlePokemon InPokemon)
    {
        EnemyInfo1.UpdateType(InPokemon);
    }
    public void UpdatePlayer1UI(BattlePokemon InPokemon, PokemonTrainer InTrainer)
    {
        PlayerInfo1.UpdateUI(InPokemon, InTrainer);
        PlayerTrainerSkillInfo.SetBattleTrainer(InTrainer);
        PlayerTrainerImage.sprite = InTrainer.TrainerSprite;
        InBattlePlayerTrainerImage.sprite = InTrainer.TrainerSprite;
    }
    public void UpdateEnemy1UI(BattlePokemon InPokemon, PokemonTrainer InTrainer)
    {
        EnemyInfo1.UpdateUI(InPokemon, InTrainer);
        EnemyTrainerSkillInfo.SetBattleTrainer(InTrainer);
        EnemyTrainerImage.sprite = InTrainer.TrainerSprite;
        InBattleEnemyTrainerImage.sprite = InTrainer.TrainerSprite;
    }
    public void GenerateSkills()
    {
        BattleCommandUI.GenerateNewSkillGroup(CurrentPokemon);
    }

    public void GenerateSwitch()
    {
        BattleCommandUI.GenerateNewSwitchGroup(CurrentPlayerTrainer);
    }

    public void EnableCommandUI()
    {
        BattleCommandUI.In();
    }
    public void DisableCommandUI()
    {
        BattleCommandUI.Out();
        Desc1Obj.SetActive(false);
        Desc2Obj.SetActive(false);
        Desc3Obj.SetActive(false);
    }
}
