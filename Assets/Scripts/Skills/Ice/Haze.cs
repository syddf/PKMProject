using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Haze : StatusSkill
{
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        InManager.GetBattlePokemons()[0].ResetStatChange();
        InManager.GetBattlePokemons()[1].ResetStatChange();
        List<string> Message = new List<string>();
        Message.Add("双方的能力复原了！");
        MessageAnimationFakeEvent FakeEvent = new MessageAnimationFakeEvent(Message);
        FakeEvent.Process(InManager);
    }
}
