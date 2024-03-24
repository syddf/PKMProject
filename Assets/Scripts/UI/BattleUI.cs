using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUI : MonoBehaviour
{
    public PlayerInfoUI PlayerInfo1; 
    public PlayerInfoUI EnemyInfo1;
    public CommandUI BattleCommandUI;
    public BattlePokemon CurrentPokemon;
    public void SetCurrentBattlePokemon(BattlePokemon InPokemon)
    {
        CurrentPokemon = InPokemon;
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

    public void EnableCommandUI()
    {
        BattleCommandUI.In();
    }
    public void DisableCommandUI()
    {
        BattleCommandUI.Out();
    }
}
