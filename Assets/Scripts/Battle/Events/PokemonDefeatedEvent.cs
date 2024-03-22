using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonDefeatedEvent : EventAnimationPlayer, Event
{
    private BattlePokemon TargetPokemon;
    private BattlePokemon SourcePokemon;
    private BattleSkill SourceSkill;

    public PokemonDefeatedEvent(BattlePokemon InTargetPokemon, BattlePokemon InSourcePokemon, BattleSkill InSkill = null)
    {
        TargetPokemon = InTargetPokemon;
        SourcePokemon = InSourcePokemon;
        SourceSkill = InSkill;
    }
    public bool ShouldProcess(BattleManager InBattleManager)
    {
        return true;
    }

    public void Process(BattleManager InManager)
    {
        InManager.TranslateTimePoint(ETimePoint.BeforePokemonDefeated, this);
        InManager.AddAnimationEvent(this);
        EditorLog.DebugLog(TargetPokemon.name + " Defeated.");
        InManager.AddDefeatedPokemon(TargetPokemon);
        InManager.TranslateTimePoint(ETimePoint.AfterPokemonDefeated, this);      
    }

    public EventType GetEventType()
    {
        return EventType.PokemonDefeated;
    }

    public BattlePokemon GetTargetPokemon() => TargetPokemon;
    public BattlePokemon GetSourcePokemon() => SourcePokemon;
}