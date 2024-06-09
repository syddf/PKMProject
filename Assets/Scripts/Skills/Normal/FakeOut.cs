using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FakeOut : DamageSkill
{    
    public override bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, out string Reason)
    {
        if(base.JudgeIsEffective(InManager, SourcePokemon, TargetPokemon, out Reason) == false)
        {
            return false;
        }
        Reason = "使用击掌奇袭失败了！击掌奇袭只能在登场回合使用！";
        return InManager.GetCurrentTurnIndex() == 1 || InManager.IsPokemonInLastTurn(SourcePokemon);
    }
    public override void AfterSkillEffectEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        SetPokemonStatusChangeEvent setStatChangeEvent = new SetPokemonStatusChangeEvent(TargetPokemon, SourcePokemon, InManager, EStatusChange.Flinch, 1, true);
        setStatChangeEvent.Process(InManager);
    }

    public override bool IsAfterSkillEffectToTargetPokemon()
    {
        return true;
    }
}
