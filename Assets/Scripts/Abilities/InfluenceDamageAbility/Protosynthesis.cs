using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protosynthesis : BaseAbility
{
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(HasTriggerd)
        {
            return false;
        }

        if(BattleManager.StaticManager.GetWeatherType() != EWeather.SunLight)
        {
            return false;
        }
        
        if(ReferencePokemon.ItemIs("驱劲能量") && ReferencePokemon.GetBattleItem().IsConsumedState())
        {
            return false;
        }

        if(TimePoint != ETimePoint.PokemonIn && TimePoint != ETimePoint.BattleStart && TimePoint != ETimePoint.AfterMegaEvolution && TimePoint != ETimePoint.AfterChangeWeather)
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

        if(SourceEvent.GetEventType() == EventType.WeatherChange)
        {
            WeatherChangeEvent CastedEvent = (WeatherChangeEvent)SourceEvent;
            return CastedEvent.GetWeatherType() == EWeather.SunLight;
        }
        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        List<Event> NewEvents = new List<Event>();
        if(InManager.GetWeatherType() == EWeather.SunLight)
        {
            string MaxStat = ReferencePokemon.GetMaxStat();
            if(MaxStat == "Atk") MaxStat = "攻击";
            if(MaxStat == "Def") MaxStat = "防御";
            if(MaxStat == "SAtk") MaxStat = "特攻";
            if(MaxStat == "SDef") MaxStat = "特防";
            if(MaxStat == "Speed") MaxStat = "速度";
            List<string> Message = new List<string>();
            Message.Add(ReferencePokemon.GetName() + "的" + MaxStat + "提高了!");
            NewEvents.Add(new MessageAnimationFakeEvent(Message));
            HasTriggerd = true;
        }
        return NewEvents;
    }
}
