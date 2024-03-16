using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatChangeEvent : Event
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

    public void PlayAnimation()
    {

    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        return true;
    }

    public void Process(BattleManager InManager)
    {
        InManager.TranslateTimePoint(ETimePoint.BeforeStatChange, this);
        if(ShouldChange)
        {
            EditorLog.DebugLog(TargetPokemon.GetName() + "Stat Changed..");
            int Factor = 1;
            if(ReverseChangeLevel)
            {
                Factor = -1;
            }
            TargetPokemon.ChangeStat(ChangedStatName, Factor * ChangedStatLevel);
        }
        InManager.TranslateTimePoint(ETimePoint.AfterStatChange, this);        
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
