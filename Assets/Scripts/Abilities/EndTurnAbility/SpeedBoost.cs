using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : EndTurnAbility
{
    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        List<Event> NewEvents = new List<Event>();
        List<string> ChangeStatList = new List<string>(){ "Speed" };
        List<int> ChangeLevel = new List<int>(){ 1 }; 
        StatChangeEvent stat1ChangeEvent = new StatChangeEvent(this.ReferencePokemon, this.ReferencePokemon, ChangeStatList, ChangeLevel);
        NewEvents.Add(stat1ChangeEvent);
        return NewEvents;
    }
}
