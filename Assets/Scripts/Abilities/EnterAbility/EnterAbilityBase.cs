using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterAbilityBase : BaseAbility
{
    public virtual bool ExtraCondition(ETimePoint TimePoint, Event SourceEvent)
    {
        return true;
    }
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(!ExtraCondition(TimePoint, SourceEvent))
        {
            return false;
        }
        
        if(TimePoint != ETimePoint.PokemonIn && TimePoint != ETimePoint.BattleStart && TimePoint != ETimePoint.AfterMegaEvolution)
        {
            return false;
        }

        if(SourceEvent.GetEventType() == EventType.Switch)
        {
            SwitchEvent CastedEvent = (SwitchEvent)SourceEvent;
            return CastedEvent.GetInPokemon() == this.ReferencePokemon;
        }

        if(SourceEvent.GetEventType() == EventType.SwitchAfterDefeated)
        {
            SwitchWhenDefeatedEvent CastedEvent = (SwitchWhenDefeatedEvent)SourceEvent;
            return CastedEvent.GetPlayerNewPokemon() == this.ReferencePokemon || CastedEvent.GetEnemyNewPokemon() == this.ReferencePokemon;
        }

        if(SourceEvent.GetEventType() == EventType.BattleStart)
        {
            SingleBattleGameStartEvent CastedEvent = (SingleBattleGameStartEvent)SourceEvent;
            return CastedEvent.GetPlayerPokemon() == this.ReferencePokemon || CastedEvent.GetEnemyPokemon() == this.ReferencePokemon;
        }

        if(SourceEvent.GetEventType() == EventType.MegaEvolution)
        {
            MegaEvent CastedEvent = (MegaEvent)SourceEvent;
            return CastedEvent.GetReferencePokemon() == this.ReferencePokemon;
        }

        return false;
    }
}
