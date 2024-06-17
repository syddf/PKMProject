using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherStrategy : BaseTrainerSkill
{    
    private BattlePokemon GetTarget()
    {
        if(ReferenceTrainer.IsPlayer)
        {
            return BattleManager.StaticManager.GetBattlePokemons()[0];
        }
        return BattleManager.StaticManager.GetBattlePokemons()[1];

    }
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(TimePoint != ETimePoint.AfterChangeWeather)
        {
            return false;
        }
        BattlePokemon ReferencePokemon = GetTarget();
        return ReferencePokemon.IsDead() == false && BattleManager.StaticManager.IsPokemonInField(ReferencePokemon);
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        List<Event> NewEvents = new List<Event>();
        BattlePokemon ReferencePokemon = GetTarget();
        NewEvents.Add(new StatChangeEvent(ReferencePokemon, null, ReferencePokemon.GetMaxStat(), 1));
        return NewEvents;
    }

}
