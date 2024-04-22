using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItem : MonoBehaviour
{
    [SerializeField]
    public string ItemName;
    [SerializeField]
    public string Description;
    [SerializeField]
    public bool CanKnockOff = true;
    public virtual bool IsConsumable()
    {
        return false;
    }

    public virtual bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent, BattlePokemon ReferencePokemon)
    {
        return false;
    }

    public virtual List<Event> Trigger(BattleManager InManager, Event SourceEvent, BattlePokemon ReferencePokemon)
    {
        return null;
    }

    public virtual bool IsGem()
    {
        return false;
    }

    public virtual bool IsMeganite()
    {
        return false;
    }
}
