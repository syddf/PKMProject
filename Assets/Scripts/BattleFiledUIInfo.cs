using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class BattleFiledUIInfo : MonoBehaviour
{
    public TextMeshProUGUI TerrainText;
    public TextMeshProUGUI TerrainTurnText;
    public TextMeshProUGUI WeatherText;
    public TextMeshProUGUI WeatherTurnText;
    public TextMeshProUGUI TrickRoomText;
    public TextMeshProUGUI TrickRoomTurnText;
    public TextMeshProUGUI TurnText;
    public TextMeshProUGUI PlayerFieldDescText;
    public TextMeshProUGUI EnemyFieldDescText;
    public TextMeshProUGUI SpecialRuleDescText;
    public void UpdateUI()
    {
        BattleManager StaticManager = BattleManager.StaticManager;
        TurnText.text = "第" + StaticManager.GetCurrentTurnIndex().ToString() + "回合";
 
        EBattleFieldTerrain terrainType = StaticManager.GetTerrainType();
        if(terrainType == EBattleFieldTerrain.None)
        {
            TerrainText.text = "无";
            TerrainTurnText.text = "/";
        }
        else
        {
            if(terrainType == EBattleFieldTerrain.Grass) TerrainText.text = "青草场地";
            if(terrainType == EBattleFieldTerrain.Misty) TerrainText.text = "薄雾场地";
            if(terrainType == EBattleFieldTerrain.Electric) TerrainText.text = "电气场地";
            if(terrainType == EBattleFieldTerrain.Psychic) TerrainText.text = "精神场地";
            TerrainTurnText.text = StaticManager.GetTerrainRemainTurn().ToString() + "回合";
        }

        EWeather weatherType = StaticManager.GetWeatherType();
        if(weatherType == EWeather.None)
        {
            WeatherText.text = "无";
            WeatherTurnText.text = "/";
        }
        else
        {
            if(weatherType == EWeather.SunLight) WeatherText.text = "晴天";
            if(weatherType == EWeather.Rain) WeatherText.text = "雨天";
            if(weatherType == EWeather.Sand) WeatherText.text = "沙暴";
            if(weatherType == EWeather.Snow) WeatherText.text = "雪天";
            WeatherTurnText.text = StaticManager.GetWeatherRemainTurn().ToString() + "回合";
        }

        if(StaticManager.GetIsTrickRoomActive())
        {
            TrickRoomText.text = "开启";
            TrickRoomTurnText.text = StaticManager.GetTrickRoomRemainTurn().ToString() + "回合";
        }
        else
        {
            TrickRoomText.text = "关闭";
            TrickRoomTurnText.text = "/";
        }
        string PlayerText = "";
        string EnemyText = "";
        List<BattleFieldStatus> PlayerStatusList = StaticManager.GetBattleFieldStatusList(true);
        foreach(var PlayerStatus in PlayerStatusList)
        {
            PlayerText += " ";
            PlayerText += BattleFieldStatus.GetFieldName(PlayerStatus.StatusType);
            if(PlayerStatus.HasLimitedTime)
            {
                PlayerText += "(";
                PlayerText += PlayerStatus.RemainTurn.ToString();
                PlayerText += "回合)";
            }
        }
        PlayerFieldDescText.text = PlayerText;

        List<BattleFieldStatus> EnemyStatusList = StaticManager.GetBattleFieldStatusList(false);
        foreach(var EnemyStatus in EnemyStatusList)
        {
            EnemyText += " ";
            EnemyText += BattleFieldStatus.GetFieldName(EnemyStatus.StatusType);
            if(EnemyStatus.HasLimitedTime)
            {
                EnemyText += "(";
                EnemyText += EnemyStatus.RemainTurn.ToString();
                EnemyText += "回合)";
            }
        }
        EnemyFieldDescText.text = EnemyText;

        BaseSpecialRule SpecialRule = BattleManager.StaticManager.GetSpecialRule();
        if(SpecialRule != null)
        {
            string DescText = SpecialRule.Description;
            if(SpecialRule.Name == "特殊规则(福爷)")
            {
                DescText += "(对手已回复：";
                DescText += BattleManager.StaticManager.GetHealedValue(false).ToString();
                DescText += "、玩家已回复：";
                DescText += BattleManager.StaticManager.GetHealedValue(true).ToString();
                DescText += ")";
            }
            if(SpecialRule.Name == "特殊规则(奇巴纳)")
            {
                int Target = 7;
                DescText += "(已达成次数：";
                DescText += BattleManager.StaticManager.GetWeatherChangCounter().ToString();
                DescText += "/";
                DescText += Target.ToString();
                DescText += ")";
            }
            if(SpecialRule.Name == "特殊规则(桄榔)")
            {
                int NotFinishedBattleCount = BattleManager.StaticManager.GetPlayerNotFinishedBattle();
                DescText += "(";
                DescText += NotFinishedBattleCount.ToString();
                DescText += ")";
            }
            SpecialRuleDescText.text = DescText;
        }
        else
        {
            SpecialRuleDescText.text = "";
        }
    }
}
