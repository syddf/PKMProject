using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatChangeEvent : EventAnimationPlayer, Event
{
    private BattlePokemon TargetPokemon;
    private string ChangedStatName;
    private int ChangedStatLevel;
    private bool ShouldChange;
    private bool ReverseChangeLevel;

    public StatChangeEvent(BattlePokemon InTargetPokemon, string InChangedStatName, int InChangedStatLevel)
    {
        TargetPokemon = InTargetPokemon;
        ChangedStatName = InChangedStatName;
        ChangedStatLevel = InChangedStatLevel;
        ShouldChange = true;
        ReverseChangeLevel = false;
    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        return true;
    }

    public override void InitAnimation()
    {

    }

    public void Process(BattleManager InManager)
    {
        InManager.TranslateTimePoint(ETimePoint.BeforeStatChange, this);
        if(ShouldChange)
        {
            InManager.AddAnimationEvent(this);
            int Factor = 1;
            if(ReverseChangeLevel)
            {
                Factor = -1;
            }
            if(TargetPokemon.ChangeStat(ChangedStatName, Factor * ChangedStatLevel))
            {
                InManager.TranslateTimePoint(ETimePoint.AfterStatChange, this);        
            }
        }
    }

    public EventType GetEventType()
    {
        return EventType.StatChange;
    }

    public BattlePokemon GetTargetPokemon() => TargetPokemon;
    public int GetChangeLevel()
    {
        int Factor = 1;
        if(ReverseChangeLevel)
        {
            Factor = -1;
        }
        return Factor * ChangedStatLevel;
    }
    public void SetReverseLevel() { ReverseChangeLevel = true;}
}
