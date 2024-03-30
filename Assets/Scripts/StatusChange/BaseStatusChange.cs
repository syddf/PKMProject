using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStatusChange
{
    protected BattlePokemon ReferencePokemon;
    public BaseStatusChange(BattlePokemon InReferencePokemon)
    {
        ReferencePokemon = InReferencePokemon;
    }
    
    public virtual bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        return false;
    }

    public virtual List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        return null;
    }

    public virtual string GetSetMessageText()
    {
        return "";
    }
    
    public BattlePokemon GetReferencePokemon()
    {
        return ReferencePokemon;
    }
}

public class BaseStatusChangeComparer : IComparer<BaseStatusChange>
{
    public int Compare(BaseStatusChange x, BaseStatusChange y)
    {
        if(x.GetReferencePokemon().GetSpeed(ECaclStatsMode.Normal) < y.GetReferencePokemon().GetSpeed(ECaclStatsMode.Normal))
        {
            return 1;
        }
        else if(x.GetReferencePokemon().GetSpeed(ECaclStatsMode.Normal) > y.GetReferencePokemon().GetSpeed(ECaclStatsMode.Normal))
        {
            return -1;
        }
        return 0;
    }
}
