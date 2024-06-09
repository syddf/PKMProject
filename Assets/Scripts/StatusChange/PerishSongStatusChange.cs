using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PerishSongStatusChange : BaseStatusChange
{
    public PerishSongStatusChange(BattlePokemon InReferencePokemon) : base(InReferencePokemon)
    {
        
    }
    
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(TimePoint != ETimePoint.TurnEnd)
        {
            return false;
        }
        TurnEndEvent CastedEvent = (TurnEndEvent)SourceEvent;
        if(CastedEvent.GetReferenceManager().IsPokemonInField(ReferencePokemon) && ReferencePokemon.IsDead() == false)
        {
            return true;
        }
        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        List<Event> NewEvents = new List<Event>();
        int RemainTurn = ReferencePokemon.GetStatusChangeRemainTime(EStatusChange.PerishSong);
        if(RemainTurn == 1)
        {
            int selfDamage = ReferencePokemon.GetHP();
            DamageEvent damageEvent = new DamageEvent(ReferencePokemon, selfDamage, "终焉之歌");
            NewEvents.Add(damageEvent);
        }
        else
        {
            List<string> Message = new List<string>();
            Message.Add(ReferencePokemon.GetName() + "的终焉之歌计数为：" + (RemainTurn - 1).ToString());
            NewEvents.Add(new MessageAnimationFakeEvent(Message));
        }
        return NewEvents;
    }

}
