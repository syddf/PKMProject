using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderWave : StatusSkill
{
    public override bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, out string Reason)
    {
        Reason = "";
        if(GetSkillType(SourcePokemon) == EType.None)
        {
            return true;
        }
        if(TargetPokemon == null)
        {
            return false;
        }
        bool J2 = TargetPokemon.GetType2() == EType.None;
        bool typeEffective = 
        DamageSkill.typeEffectiveness[(int)GetSkillType(SourcePokemon), (int)TargetPokemon.GetType1()] != 0 && 
        (J2 || DamageSkill.typeEffectiveness[(int)GetSkillType(SourcePokemon), (int)TargetPokemon.GetType2()] != 0);
        return SetPokemonStatusChangeEvent.IsStatusChangeEffective(InManager, TargetPokemon, SourcePokemon, EStatusChange.Paralysis) && typeEffective;
    }
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        SetPokemonStatusChangeEvent setStatChangeEvent = new SetPokemonStatusChangeEvent(TargetPokemon, SourcePokemon, InManager, EStatusChange.Paralysis   , 1, false);
        setStatChangeEvent.Process(InManager);
    }
}
