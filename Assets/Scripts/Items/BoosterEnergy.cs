using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterEnergy : BaseItem
{
    public override bool IsConsumable()
    {
        return true;
    }

    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent, BattlePokemon ReferencePokemon)
    {
        if(TimePoint != ETimePoint.PokemonIn && TimePoint != ETimePoint.BattleStart)
        {
            return false;
        }
        if(ReferencePokemon.HasAbility("古代活性", BattleManager.StaticManager, null, ReferencePokemon) 
        && BattleManager.StaticManager.GetWeatherType() != EWeather.SunLight
        && ReferencePokemon.GetAbility().GetTriggered() == false)
        {
            return true;
        }

        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent, BattlePokemon ReferencePokemon)
    {
        List<Event> NewEvents = new List<Event>();
        string MaxStat = ReferencePokemon.GetMaxStat();
        if(MaxStat == "Atk") MaxStat = "攻击";
        if(MaxStat == "Def") MaxStat = "防御";
        if(MaxStat == "SAtk") MaxStat = "特攻";
        if(MaxStat == "SDef") MaxStat = "特防";
        if(MaxStat == "Speed") MaxStat = "速度";
        List<string> Message = new List<string>();
        Message.Add(ReferencePokemon.GetName() + "的" + MaxStat + "提高了!");
        NewEvents.Add(new MessageAnimationFakeEvent(Message));
        return NewEvents;
    }
}
