using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAbility : MonoBehaviour
{
    [SerializeField]
    protected string Name;
    [SerializeField]
    protected string Description;
    [SerializeField]
    protected BattlePokemon ReferencePokemon;

    public virtual bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        return false;
    }

    public string GetAbilityName()
    {
        return this.Name;
    }

    public virtual List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        return null;
    }

    public BattlePokemon GetReferencePokemon() => ReferencePokemon;
}

public class AbilityComparer : IComparer<BaseAbility>
{
    public int Compare(BaseAbility x, BaseAbility y)
    {
        if(x.GetReferencePokemon().GetSpeed() < y.GetReferencePokemon().GetSpeed())
        {
            return 1;
        }
        else if(x.GetReferencePokemon().GetSpeed() > y.GetReferencePokemon().GetSpeed())
        {
            return -1;
        }
        return 0;
    }
}
