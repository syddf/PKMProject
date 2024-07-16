using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class BattlePokemonInfoUI : MonoBehaviour
{
    public TextMeshProUGUI StatusChangeText;
    public TextMeshProUGUI AtkStatChangeText;
    public TextMeshProUGUI DefStatChangeText;
    public TextMeshProUGUI SAtkStatChangeText;
    public TextMeshProUGUI SDefStatChangeText;
    public TextMeshProUGUI SpeedStatChangeText;
    public TextMeshProUGUI CTStatChangeText;
    public TextMeshProUGUI AccuStatChangeText;
    public TextMeshProUGUI EvaStatChangeText;
    public PokemonInfoUI PkmInfo;

    public void SetBattlePokemon(BattlePokemon InPokemon)
    {
        PkmInfo.SetBattlePokemon(InPokemon);
        AtkStatChangeText.text = InPokemon.GetAtkChangeLevel().ToString();
        DefStatChangeText.text = InPokemon.GetDefChangeLevel().ToString();
        SAtkStatChangeText.text = InPokemon.GetSAtkChangeLevel().ToString();
        SDefStatChangeText.text = InPokemon.GetSDefChangeLevel().ToString();
        SpeedStatChangeText.text = InPokemon.GetSpeedChangeLevel().ToString();
        CTStatChangeText.text = InPokemon.GetCTLevel().ToString();
        AccuStatChangeText.text = InPokemon.GetAccuracyrateLevel(ECaclStatsMode.Normal).ToString();
        EvaStatChangeText.text = InPokemon.GetEvasionrateLevel(ECaclStatsMode.Normal).ToString();

        List<StatusChange> StatusChangeList = InPokemon.GetAllStatusChangeInfo();

        string statusChangeText = "";
        if(StatusChangeList != null)
        {
            foreach(var StatusChange in StatusChangeList)
            {
                string text = " " + SetPokemonStatusChangeEvent.GetSetMessageText(StatusChange.StatusChangeType);
                if(StatusChange.HasLimitedTime)
                {
                    text += "(";
                    text += StatusChange.RemainTime.ToString();
                    text += "回合";
                    text += ")";
                }
                statusChangeText += text;
            }
        }
        StatusChangeText.text = statusChangeText;
    }
}
