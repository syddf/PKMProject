using Naninovel;
using Naninovel.Commands;
using UnityEngine;

[CommandAlias("beginbattle")]
public class NaniNovelCustomFunc : Command
{
    public StringParameter Name;

    public override UniTask ExecuteAsync (AsyncToken asyncToken = default)
    {
        GameObject SingleBattle = GameObject.Find("SingleBattle");
        SingleBattle.GetComponent<BattleManager>().ShowBeforeBattleMenu(Name);
        return UniTask.CompletedTask;
    }
}