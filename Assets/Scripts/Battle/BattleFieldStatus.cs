using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EBattleFieldStatus
{
    None,
    LightScreenStatus,
    ReflectStatus,
    PowerChordBlue,
    PowerChordGreen,
    PowerChordPurple
}

public struct BattleFieldStatus
{
    public EBattleFieldStatus StatusType;
    public bool HasLimitedTime;
    public int RemainTurn;
    public BaseBattleFieldStatusChange BaseStatusChange;
    public bool IsPlayer;

    public BattleFieldStatus(EBattleFieldStatus InStatusType, bool InHasLimitedTime, int InRemainTurn, bool InIsPlayer)
    {
        StatusType = InStatusType;
        HasLimitedTime = InHasLimitedTime;
        RemainTurn = InRemainTurn;
        IsPlayer = InIsPlayer;
        BaseStatusChange = GetBaseStatusChange(StatusType, IsPlayer);
    }

    public static BaseBattleFieldStatusChange GetBaseStatusChange(EBattleFieldStatus StatusType, bool InIsPlayer)
    {
        if(StatusType == EBattleFieldStatus.PowerChordBlue)
        {
            return new PowerChordStatusChange(0, InIsPlayer);
        }
        if(StatusType == EBattleFieldStatus.PowerChordGreen)
        {
            return new PowerChordStatusChange(1, InIsPlayer);
        }
        if(StatusType == EBattleFieldStatus.PowerChordPurple)
        {
            return new PowerChordStatusChange(2, InIsPlayer);
        }
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
            return "受到的物理伤害降低了！";
        }

        if(InStatusType == EBattleFieldStatus.LightScreenStatus)
        {
            return "受到的特殊伤害降低了！";
        }

        if(InStatusType == EBattleFieldStatus.PowerChordBlue)
        {
            return "场上响起了英勇赞美诗！";
        }
        
        if(InStatusType == EBattleFieldStatus.PowerChordGreen)
        {
            return "场上响起了坚毅咏叹调！";
        }

        if(InStatusType == EBattleFieldStatus.PowerChordPurple)
        {
            return "场上响起了迅捷鸣奏曲！";
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

        if(InStatusType == EBattleFieldStatus.PowerChordBlue ||
           InStatusType == EBattleFieldStatus.PowerChordGreen ||
           InStatusType == EBattleFieldStatus.PowerChordPurple )
        {
            return "能量和弦";
        }

        return "";        
    }
}

public class BaseBattleFieldStatusChange
{
    protected bool IsPlayer;
    public BaseBattleFieldStatusChange(bool InIsPlayer)
    {
        IsPlayer = InIsPlayer;
    }
    public virtual bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        return false;
    }

    public virtual List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        return null;
    }

    public virtual void OnSetAnimation()
    {
    }

    public virtual void OnRemoveAnimation()
    {
    }
}

public class PowerChordStatusChange : BaseBattleFieldStatusChange
{
    private int ChordType;
    private BattlePokemon TargetPokemon;
    public PowerChordStatusChange(int InChordType, bool InIsPlayer) : base(InIsPlayer)
    {
        ChordType = InChordType;
    }

    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        TargetPokemon = null;
        if(SourceEvent.GetEventType() == EventType.Switch)
        {
            SwitchEvent CastedEvent = (SwitchEvent)SourceEvent;
            if(CastedEvent.GetInPokemon().GetIsEnemy() != IsPlayer)
            {
                TargetPokemon = CastedEvent.GetInPokemon();
                return true;
            }
        }
        else if(SourceEvent.GetEventType() == EventType.SwitchAfterDefeated)
        {
            SwitchWhenDefeatedEvent CastedEvent = (SwitchWhenDefeatedEvent)SourceEvent;
            if(CastedEvent.GetPlayerNewPokemon() && CastedEvent.GetPlayerNewPokemon().GetIsEnemy() != IsPlayer)
            {
                TargetPokemon = CastedEvent.GetPlayerNewPokemon();
                return true;
            }
            if(CastedEvent.GetEnemyNewPokemon() && CastedEvent.GetEnemyNewPokemon().GetIsEnemy() != IsPlayer)
            {
                TargetPokemon = CastedEvent.GetEnemyNewPokemon();
                return true;
            }
        }
        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        List<Event> NewEvents = new List<Event>();
        if(ChordType == 0)
        {
            NewEvents.Add(new StatChangeEvent(TargetPokemon, null, "Atk", 1, "能量和弦效果"));
            NewEvents.Add(new StatChangeEvent(TargetPokemon, null, "SAtk", 1, "能量和弦效果"));
        }
        else if(ChordType == 1)
        {
            NewEvents.Add(new StatChangeEvent(TargetPokemon, null, "Def", 1, "能量和弦效果"));
            NewEvents.Add(new StatChangeEvent(TargetPokemon, null, "SDef", 1, "能量和弦效果"));
        }
        else if(ChordType == 2)
        {
            NewEvents.Add(new StatChangeEvent(TargetPokemon, null, "Speed", 1, "能量和弦效果"));
        }
        return NewEvents;
    }

    public override void OnSetAnimation()
    {
        GameObject AnimObj = GameObject.Find("G_Anims/PowerChordParticles");
        SubObjects Subs = AnimObj.GetComponent<SubObjects>();
        GameObject[] Chords = new GameObject[]{Subs.SubObject1, Subs.SubObject2, Subs.SubObject3};
        AnimObj.transform.position = Subs.SubObject4.transform.position;
        if(!IsPlayer)
        {
            AnimObj.transform.position = Subs.SubObject5.transform.position;
        }
        Chords[ChordType].SetActive(true);
    }

    public override void OnRemoveAnimation()
    {
        GameObject AnimObj = GameObject.Find("G_Anims/PowerChordParticles");
        SubObjects Subs = AnimObj.GetComponent<SubObjects>();
        GameObject[] Chords = new GameObject[]{Subs.SubObject1, Subs.SubObject2, Subs.SubObject3};
        Chords[ChordType].SetActive(false);
    }
}