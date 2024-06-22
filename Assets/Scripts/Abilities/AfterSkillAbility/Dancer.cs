using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dancer : BaseAbility
{
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(TimePoint != ETimePoint.AfterSkillEffect)
        {
            return false;
        }

        if(SourceEvent.GetEventType() == EventType.UseSkill)
        {
            SkillEvent CastedEvent = (SkillEvent)SourceEvent;
            if(CastedEvent.GetSourcePokemon() == this.ReferencePokemon)
            {
                return true;
            }
        }

        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        SkillEvent CastedEvent = (SkillEvent)SourceEvent;
        List<Event> NewEvents = new List<Event>();
        List<string> ChangeStatList = new List<string>(){ "Speed", "Evasionrate" };
        List<int> ChangeLevel = new List<int>(){ 1, 1 }; 
        StatChangeEvent stat1ChangeEvent = new StatChangeEvent(CastedEvent.GetSourcePokemon(), CastedEvent.GetSourcePokemon(), ChangeStatList, ChangeLevel);
        NewEvents.Add(stat1ChangeEvent);
        return NewEvents;
    }
}
