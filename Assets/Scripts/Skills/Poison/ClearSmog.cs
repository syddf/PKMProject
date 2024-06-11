using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearSmog : DamageSkill
{
    public override void AfterSkillEffectEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        TargetPokemon.ResetStatChange();
        List<string> Message = new List<string>();
        Message.Add("对方的能力复原了！");
        MessageAnimationFakeEvent FakeEvent = new MessageAnimationFakeEvent(Message);
        FakeEvent.Process(InManager);
    }
    public override bool IsAfterSkillEffectToTargetPokemon()
    {
        return true;
    }
}
