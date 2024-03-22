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
    public void UpdatePlayer1UI(BattlePokemon InPokemon)
    {
        PlayerInfo1.UpdateUI(InPokemon);
    }
    public void UpdateEnemy1UI(BattlePokemon InPokemon)
    {
        EnemyInfo1.UpdateUI(InPokemon);
    }
    public void GenerateSkills()
    {
        BattleCommandUI.GenerateNewSkillGroup(CurrentPokemon);
    }
}
