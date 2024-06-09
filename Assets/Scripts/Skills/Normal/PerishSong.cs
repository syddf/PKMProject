using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerishSong : StatusSkill
{    
    public override bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, out string Reason)
    {
        Reason = "";
        BattlePokemon P0 = InManager.GetBattlePokemons()[0];
        BattlePokemon P1 = InManager.GetBattlePokemons()[1];
        return SetPokemonStatusChangeEvent.IsStatusChangeEffective(InManager, P0, SourcePokemon, EStatusChange.PerishSong) 
        || SetPokemonStatusChangeEvent.IsStatusChangeEffective(InManager, P1, SourcePokemon, EStatusChange.PerishSong);
    }
    public override void ProcessStatusSkillEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        BattlePokemon P0 = InManager.GetBattlePokemons()[0];
        BattlePokemon P1 = InManager.GetBattlePokemons()[1];
        SetPokemonStatusChangeEvent setStatChangeEvent1 = new SetPokemonStatusChangeEvent(P0, SourcePokemon, InManager, EStatusChange.PerishSong, 4, true);
        setStatChangeEvent1.Process(InManager);
        SetPokemonStatusChangeEvent setStatChangeEvent2 = new SetPokemonStatusChangeEvent(P1, SourcePokemon, InManager, EStatusChange.PerishSong, 4, true);
        setStatChangeEvent2.Process(InManager);
    }
}
