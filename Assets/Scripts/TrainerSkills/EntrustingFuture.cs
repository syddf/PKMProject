using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntrustingFuture : BaseTrainerSkill
{
    private BattlePokemon GetTarget()
    {
        if(ReferenceTrainer.IsPlayer)
        {
            return BattleManager.StaticManager.GetBattlePokemons()[1];
        }
        return BattleManager.StaticManager.GetBattlePokemons()[0];

    }
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(TimePoint != ETimePoint.TurnEnd)
        {
            return false;
        }

        if(SourceEvent.GetEventType() == EventType.TurnEnd && GetTarget().IsDead() == false)
        {
            return true;
        }

        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        List<Event> NewEvents = new List<Event>();
        List<string> ChangeStatList = new List<string>();
        System.Random rnd = new System.Random();
        int Random = rnd.Next(0, 5);
        EStatusChange NewStatus = EStatusChange.Poison;
        if(Random == 0)
        {
            ChangeStatList.Add("Atk");
        }
        if(Random == 1)
        {
            ChangeStatList.Add("SAtk");
        }
        if(Random == 2)
        {
            ChangeStatList.Add("Def");
        }
        if(Random == 3)
        {
            ChangeStatList.Add("SDef");
        }
        if(Random == 4)
        {
            ChangeStatList.Add("Speed");
        }
        List<int> ChangeLevel = new List<int>(){ 1 }; 
        StatChangeEvent stat1ChangeEvent = new StatChangeEvent(GetTarget(), null, ChangeStatList, ChangeLevel);
        NewEvents.Add(stat1ChangeEvent);
        return NewEvents;
    }

}
