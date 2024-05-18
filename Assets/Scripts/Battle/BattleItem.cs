using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BattleItem
{    
    private BaseItem ReferenceBaseItem;
    private BattlePokemon ReferencePokemon;
    private bool Consumed = false;
    private bool KnockOffed = false;

    public BattleItem(BaseItem InReferenceBaseItem, BattlePokemon InReferencePokemon)
    {
        ReferenceBaseItem = InReferenceBaseItem;
        ReferencePokemon = InReferencePokemon;
    }

    public BaseItem GetBaseItem()
    {
        return ReferenceBaseItem;
    }
    public string GetItemName()
    {
        return ReferenceBaseItem.ItemName;
    }

    public string GetItemDescription()
    {
        return ReferenceBaseItem.Description;
    }

    public BattlePokemon GetReferencePokemon()
    {
        return ReferencePokemon;
    }

    public bool IsConsumable()
    {
        if(ReferenceBaseItem == null) return false;
        return ReferenceBaseItem.IsConsumable();
    }

    public bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(IsConsumable() && Consumed) return false;
        if(KnockOffed) return false;
        if(ReferenceBaseItem == null) return false;
        return ReferenceBaseItem.ShouldTrigger(TimePoint, SourceEvent, ReferencePokemon);
    }

    public virtual List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        Consume();
        return ReferenceBaseItem.Trigger(InManager, SourceEvent, ReferencePokemon);
    }

    public bool HasItem()
    {
        return ReferenceBaseItem != null && KnockOffed == false && Consumed == false;
    }

    public void Consume()
    {
        if(IsConsumable())
            Consumed = true;
        ReferencePokemon.SetLostItem();       
        ReferencePokemon.SetConsumeThisTurn(); 
    }

    public void KnockOffItem()
    {
        KnockOffed = true;
        ReferencePokemon.SetLostItem();        
    }

    public bool CanKnockOff()
    {
        return ReferenceBaseItem.CanKnockOff;
    }

    public bool IsConsumedState()
    {
        return Consumed && ReferencePokemon.GetLostItem();  
    }
}

public class BattleItemComparer : IComparer<BattleItem>
{
    public int Compare(BattleItem x, BattleItem y)
    {
        if(x.GetReferencePokemon().GetSpeed(ECaclStatsMode.Normal, BattleManager.StaticManager) < y.GetReferencePokemon().GetSpeed(ECaclStatsMode.Normal, BattleManager.StaticManager))
        {
            return 1;
        }
        else if(x.GetReferencePokemon().GetSpeed(ECaclStatsMode.Normal, BattleManager.StaticManager) > y.GetReferencePokemon().GetSpeed(ECaclStatsMode.Normal, BattleManager.StaticManager))
        {
            return -1;
        }
        return 0;
    }
}
