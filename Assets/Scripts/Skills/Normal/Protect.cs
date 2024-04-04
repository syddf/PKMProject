using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Protect : StatusSkill
{
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        SetPokemonStatusChangeEvent setStatChangeEvent = new SetPokemonStatusChangeEvent(SourcePokemon, InManager, EStatusChange.Protect, 1, true);
        setStatChangeEvent.Process(InManager);
    }

    public override bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, out string Reason)
    {
        Reason = "";
        int FailedNumber = 1;
        int CurrentTurn = InManager.GetCurrentTurnIndex();

        while(CurrentTurn > 0)
        {
            List<BattleSkill> UsedSkill = InManager.GetPokemonSkillInTurnEffective(SourcePokemon, CurrentTurn - 1);
            bool Found = false;
            foreach(var Skill in UsedSkill)
            {
                if(Skill.GetSkillName() == "守住")
                {
                    FailedNumber *= 3;
                    break;
                }
            }
            if(Found == false)
            {
                break;
            }
            CurrentTurn = CurrentTurn - 1;
        }

        System.Random rnd = new System.Random();
        int Random = rnd.Next(0, FailedNumber);
        return Random == 0;
    }
}
