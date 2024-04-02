using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class KnockOff : DamageSkill
{
    protected override int GetSkillPower(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        BattleItem Item = TargetPokemon.GetBattleItem();
        double Result = Power;
        if(Item.HasItem() && Item.CanKnockOff())
        {
            Result = Result * 1.5;
        }
        return (int)Math.Floor(Result);
    }
    public override void AfterSkillEffectEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        BattleItem Item = TargetPokemon.GetBattleItem();
        if(Item.HasItem() && Item.CanKnockOff())
        {
            KnockOffItemEvent newEvent = new KnockOffItemEvent(TargetPokemon);
            newEvent.Process(InManager);
        }
    }
}
