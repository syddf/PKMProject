using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUI : MonoBehaviour
{
    public PlayerInfoUI PlayerInfo1; 
    public PlayerInfoUI EnemyInfo1;
    public CommandUI BattleCommandUI;
    public BattlePokemon CurrentPokemon;
    public PokemonTrainer CurrentPlayerTrainer;
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
    public void UpdatePlayer1UI(BattlePokemon InPokemon, PokemonTrainer InTrainer)
    {
        PlayerInfo1.UpdateUI(InPokemon, InTrainer);
    }
    public void UpdateEnemy1UI(BattlePokemon InPokemon, PokemonTrainer InTrainer)
    {
        EnemyInfo1.UpdateUI(InPokemon, InTrainer);
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
    }
}
