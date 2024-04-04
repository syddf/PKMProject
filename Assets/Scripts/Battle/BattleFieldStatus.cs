using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EBattleFieldStatus
{
    LightScreenStatus,
    ReflectStatus
}

public struct BattleFieldStatus
{
    public EBattleFieldStatus StatusType;
    public bool HasLimitedTime;
    public int RemainTurn;
    public BaseBattleFieldStatusChange BaseStatusChange;

    public BattleFieldStatus(EBattleFieldStatus InStatusType, bool InHasLimitedTime, int InRemainTurn)
    {
        StatusType = InStatusType;
        HasLimitedTime = InHasLimitedTime;
        RemainTurn = InRemainTurn;
        BaseStatusChange = GetBaseStatusChange(StatusType);
    }

    public static BaseBattleFieldStatusChange GetBaseStatusChange(EBattleFieldStatus StatusType)
    {
        return null;
    }

    public static bool IsStatusOnlyOne(EBattleFieldStatus StatusType)
    {
        return true;
    }

    public static string GetFieldDescription(EBattleFieldStatus InStatusType)
    {
        if(InStatusType == EBattleFieldStatus.ReflectStatus)
        {
            return "受到的物理伤害降低了!";
        }

        if(InStatusType == EBattleFieldStatus.LightScreenStatus)
        {
            return "受到的特殊伤害降低了!";
        }

        return "";        
    }

    public static string GetFieldName(EBattleFieldStatus InStatusType)
    {
        if(InStatusType == EBattleFieldStatus.ReflectStatus)
        {
            return "反射壁";
        }

        if(InStatusType == EBattleFieldStatus.LightScreenStatus)
        {
            return "光墙";
        }

        return "";        
    }
}

public class BaseBattleFieldStatusChange
{
    public virtual bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        return false;
    }

    public virtual List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        return null;
    }
}