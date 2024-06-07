using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum EBattleFieldStatus
{
    None,
    LightScreenStatus,
    ReflectStatus,
    AuroraVeilStatus,
    PowerChordBlue,
    PowerChordGreen,
    PowerChordPurple,
    StickyWeb,
    StealthRock,
    Spikes1,
    Spikes2,
    Spikes3,
    Safeguard,
    Tailwind,
    ToxicSpikes,
    FutureAttack,
    FutureAttackEnhanced,
    Wish
}

public struct BattleFieldStatus
{
    public EBattleFieldStatus StatusType;
    public bool HasLimitedTime;
    public int RemainTurn;
    public BaseBattleFieldStatusChange BaseStatusChange;
    public bool IsPlayer;
    public BattlePokemon SourcePokemon;
    public BattleFieldStatus(BattlePokemon InSourcePokemon, EBattleFieldStatus InStatusType, bool InHasLimitedTime, int InRemainTurn, bool InIsPlayer)
    {
        StatusType = InStatusType;
        HasLimitedTime = InHasLimitedTime;
        RemainTurn = InRemainTurn;
        IsPlayer = InIsPlayer;
        SourcePokemon= InSourcePokemon;
        BaseStatusChange = GetBaseStatusChange(SourcePokemon, StatusType, IsPlayer);
    }

    public static BaseBattleFieldStatusChange GetBaseStatusChange(BattlePokemon SourcePokemon, EBattleFieldStatus StatusType, bool InIsPlayer)
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
        if(StatusType == EBattleFieldStatus.StickyWeb)
        {
            return new StickyWebStatusChange(SourcePokemon, InIsPlayer);
        }
        if(StatusType == EBattleFieldStatus.StealthRock)
        {
            return new StealthRockStatusChange(SourcePokemon, InIsPlayer);
        }
        if(StatusType == EBattleFieldStatus.Spikes1)
        {
            return new SpikesStatusChange(0, SourcePokemon, InIsPlayer);
        }
        if(StatusType == EBattleFieldStatus.Spikes2)
        {
            return new SpikesStatusChange(1, SourcePokemon, InIsPlayer);
        }
        if(StatusType == EBattleFieldStatus.Spikes3)
        {
            return new SpikesStatusChange(2, SourcePokemon, InIsPlayer);
        }
        if(StatusType == EBattleFieldStatus.ToxicSpikes)
        {
            return new ToxicSpikesStatusChange(SourcePokemon, InIsPlayer);
        }
        if(StatusType == EBattleFieldStatus.FutureAttack)
        {
            return new FutureAttackStatusChange(SourcePokemon, InIsPlayer);
        }
        if(StatusType == EBattleFieldStatus.FutureAttackEnhanced)
        {
            return new FutureAttackEnhancedStatusChange(SourcePokemon, InIsPlayer);
        }
        if(StatusType == EBattleFieldStatus.Wish)
        {
            return new WishStatusChange(SourcePokemon, InIsPlayer);
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

        if(InStatusType == EBattleFieldStatus.AuroraVeilStatus)
        {
            return "受到的物理伤害和特殊伤害降低了！";
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

        if(InStatusType == EBattleFieldStatus.StickyWeb)
        {
            return "场上布满了黏黏网！";
        }

        if(InStatusType == EBattleFieldStatus.StealthRock)
        {
            return "场上布满了尖锐的岩石！";
        }

        if(InStatusType == EBattleFieldStatus.Spikes1)
        {
            return "场上布满了一层尖锐的刺！";
        }

        if(InStatusType == EBattleFieldStatus.Spikes2)
        {
            return "场上布满了两层尖锐的刺！";
        }

        if(InStatusType == EBattleFieldStatus.Spikes3)
        {
            return "场上布满了三层尖锐的刺！";
        }

        if(InStatusType == EBattleFieldStatus.Safeguard)
        {
            return "受到神秘守护的保护了！";
        }

        if(InStatusType == EBattleFieldStatus.Tailwind)
        {
            return "场上吹起了顺风！";
        }

        if(InStatusType == EBattleFieldStatus.ToxicSpikes)
        {
            return "场上布满了毒菱！";
        }

        if(InStatusType == EBattleFieldStatus.FutureAttack || InStatusType == EBattleFieldStatus.FutureAttackEnhanced)
        {
            return "场上感知到了来自未来的攻击！";
        }

        if(InStatusType == EBattleFieldStatus.Wish)
        {
            return "场上被赐予了祝福！";
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

        if(InStatusType == EBattleFieldStatus.AuroraVeilStatus)
        {
            return "极光幕";
        }

        if(InStatusType == EBattleFieldStatus.PowerChordBlue ||
           InStatusType == EBattleFieldStatus.PowerChordGreen ||
           InStatusType == EBattleFieldStatus.PowerChordPurple )
        {
            return "能量和弦";
        }

        if(InStatusType == EBattleFieldStatus.StickyWeb)
        {
            return "黏黏网";
        }

        
        if(InStatusType == EBattleFieldStatus.StealthRock)
        {
            return "隐形岩";
        }

        if(InStatusType == EBattleFieldStatus.Spikes1)
        {
            return "撒菱(1)";
        }

        if(InStatusType == EBattleFieldStatus.Spikes2)
        {
            return "撒菱(2)";
        }

        if(InStatusType == EBattleFieldStatus.Spikes3)
        {
            return "撒菱(3)";
        }

        if(InStatusType == EBattleFieldStatus.Safeguard)
        {
            return "神秘守护";
        }

        if(InStatusType == EBattleFieldStatus.Tailwind)
        {
            return "顺风";
        }

        if(InStatusType == EBattleFieldStatus.ToxicSpikes)
        {
            return "毒菱";
        }

        if(InStatusType == EBattleFieldStatus.FutureAttack || InStatusType == EBattleFieldStatus.FutureAttackEnhanced)
        {
            return "未来攻击";
        }

        if(InStatusType == EBattleFieldStatus.Wish)
        {
            return "祈愿";
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

public class ToxicSpikesStatusChange : BaseBattleFieldStatusChange
{
    private BattlePokemon SourcePokemon;    
    private BattlePokemon TargetPokemon;
    public ToxicSpikesStatusChange(BattlePokemon InSourcePokemon, bool InIsPlayer) : base(InIsPlayer)
    {
        SourcePokemon = InSourcePokemon;
    }

    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        TargetPokemon = null;
        if(SourceEvent.GetEventType() == EventType.Switch)
        {
            SwitchEvent CastedEvent = (SwitchEvent)SourceEvent;
            if(CastedEvent.GetInPokemon().IsGroundPokemon(BattleManager.StaticManager) && 
            CastedEvent.GetInPokemon().GetIsEnemy() != IsPlayer &&
            CastedEvent.GetInPokemon().HasItem("厚底靴") == false)
            {
                TargetPokemon = CastedEvent.GetInPokemon();
                return true;
            }
        }
        else if(SourceEvent.GetEventType() == EventType.SwitchAfterDefeated)
        {
            SwitchWhenDefeatedEvent CastedEvent = (SwitchWhenDefeatedEvent)SourceEvent;
            if(CastedEvent.GetPlayerNewPokemon() && 
            CastedEvent.GetPlayerNewPokemon().IsGroundPokemon(BattleManager.StaticManager) && 
            CastedEvent.GetPlayerNewPokemon().GetIsEnemy() != IsPlayer && 
            CastedEvent.GetPlayerNewPokemon().HasItem("厚底靴") == false)
            {
                TargetPokemon = CastedEvent.GetPlayerNewPokemon();
                return true;
            }
            if(CastedEvent.GetEnemyNewPokemon() && 
            CastedEvent.GetEnemyNewPokemon().IsGroundPokemon(BattleManager.StaticManager) && 
            CastedEvent.GetEnemyNewPokemon().GetIsEnemy() != IsPlayer &&
            CastedEvent.GetEnemyNewPokemon().HasItem("厚底靴") == false)
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
        SetPokemonStatusChangeEvent StatusEvent = new SetPokemonStatusChangeEvent(
            TargetPokemon,
            null,
            InManager,
            EStatusChange.Poison,
            0, 
            false
        );
        NewEvents.Add(StatusEvent);
        return NewEvents;
    }

}

public class StickyWebStatusChange : BaseBattleFieldStatusChange
{
    private BattlePokemon SourcePokemon;    
    private BattlePokemon TargetPokemon;
    public StickyWebStatusChange(BattlePokemon InSourcePokemon, bool InIsPlayer) : base(InIsPlayer)
    {
        SourcePokemon = InSourcePokemon;
    }

    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        TargetPokemon = null;
        if(SourceEvent.GetEventType() == EventType.Switch)
        {
            SwitchEvent CastedEvent = (SwitchEvent)SourceEvent;
            if(CastedEvent.GetInPokemon().IsGroundPokemon(BattleManager.StaticManager) && 
            CastedEvent.GetInPokemon().GetIsEnemy() != IsPlayer &&
            CastedEvent.GetInPokemon().HasItem("厚底靴") == false)
            {
                TargetPokemon = CastedEvent.GetInPokemon();
                return true;
            }
        }
        else if(SourceEvent.GetEventType() == EventType.SwitchAfterDefeated)
        {
            SwitchWhenDefeatedEvent CastedEvent = (SwitchWhenDefeatedEvent)SourceEvent;
            if(CastedEvent.GetPlayerNewPokemon() && 
            CastedEvent.GetPlayerNewPokemon().IsGroundPokemon(BattleManager.StaticManager) && 
            CastedEvent.GetPlayerNewPokemon().GetIsEnemy() != IsPlayer &&
            CastedEvent.GetPlayerNewPokemon().HasItem("厚底靴") == false)
            {
                TargetPokemon = CastedEvent.GetPlayerNewPokemon();
                return true;
            }
            if(CastedEvent.GetEnemyNewPokemon() && 
            CastedEvent.GetEnemyNewPokemon().IsGroundPokemon(BattleManager.StaticManager) && 
            CastedEvent.GetEnemyNewPokemon().GetIsEnemy() != IsPlayer && 
            CastedEvent.GetEnemyNewPokemon().HasItem("厚底靴") == false)
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
        NewEvents.Add(new StatChangeEvent(TargetPokemon, SourcePokemon, "Speed", -1, "黏黏网的效果"));
        return NewEvents;
    }

}
public class StealthRockStatusChange : BaseBattleFieldStatusChange
{
    private BattlePokemon SourcePokemon;    
    private BattlePokemon TargetPokemon;
    public StealthRockStatusChange(BattlePokemon InSourcePokemon, bool InIsPlayer) : base(InIsPlayer)
    {
        SourcePokemon = InSourcePokemon;
    }

    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        TargetPokemon = null;
        if(SourceEvent.GetEventType() == EventType.Switch)
        {
            SwitchEvent CastedEvent = (SwitchEvent)SourceEvent;
            if(CastedEvent.GetInPokemon().GetIsEnemy() != IsPlayer &&
            CastedEvent.GetInPokemon().HasItem("厚底靴") == false)
            {
                TargetPokemon = CastedEvent.GetInPokemon();
                return true;
            }
        }
        else if(SourceEvent.GetEventType() == EventType.SwitchAfterDefeated)
        {
            SwitchWhenDefeatedEvent CastedEvent = (SwitchWhenDefeatedEvent)SourceEvent;
            if(CastedEvent.GetPlayerNewPokemon() && CastedEvent.GetPlayerNewPokemon().GetIsEnemy() != IsPlayer
            && CastedEvent.GetPlayerNewPokemon().HasItem("厚底靴") == false)
            {
                TargetPokemon = CastedEvent.GetPlayerNewPokemon();
                return true;
            }
            if(CastedEvent.GetEnemyNewPokemon() && CastedEvent.GetEnemyNewPokemon().GetIsEnemy() != IsPlayer
            && CastedEvent.GetEnemyNewPokemon().HasItem("厚底靴") == false)
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
        int baseDamage = TargetPokemon.GetMaxHP() / 8;
        double Factor2 = TargetPokemon.GetType2(InManager, SourcePokemon, TargetPokemon) == EType.None ? 1.0 : DamageSkill.typeEffectiveness[(int)EType.Rock, (int)TargetPokemon.GetType2(InManager, SourcePokemon, TargetPokemon)];
        double Factor = DamageSkill.typeEffectiveness[(int)EType.Rock, (int)TargetPokemon.GetType1(InManager, SourcePokemon, TargetPokemon)] * Factor2;

        int selfDamage = Math.Min(TargetPokemon.GetHP(), Math.Max(1, (int)(baseDamage * Factor)));
        DamageEvent damageEvent = new DamageEvent(TargetPokemon, selfDamage, "隐形岩");
        NewEvents.Add(damageEvent);
        return NewEvents;
    }

}
public class SpikesStatusChange : BaseBattleFieldStatusChange
{
    private BattlePokemon SourcePokemon;    
    private BattlePokemon TargetPokemon;
    private int Level;
    public SpikesStatusChange(int InLevel, BattlePokemon InSourcePokemon, bool InIsPlayer) : base(InIsPlayer)
    {
        SourcePokemon = InSourcePokemon;
        Level = InLevel;
    }

    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        TargetPokemon = null;
        if(SourceEvent.GetEventType() == EventType.Switch)
        {
            SwitchEvent CastedEvent = (SwitchEvent)SourceEvent;
            if(CastedEvent.GetInPokemon().IsGroundPokemon(BattleManager.StaticManager) && 
            CastedEvent.GetInPokemon().GetIsEnemy() != IsPlayer &&
            CastedEvent.GetInPokemon().HasItem("厚底靴") == false)
            {
                TargetPokemon = CastedEvent.GetInPokemon();
                return true;
            }
        }
        else if(SourceEvent.GetEventType() == EventType.SwitchAfterDefeated)
        {
            SwitchWhenDefeatedEvent CastedEvent = (SwitchWhenDefeatedEvent)SourceEvent;
            if(CastedEvent.GetPlayerNewPokemon() && 
            CastedEvent.GetPlayerNewPokemon().IsGroundPokemon(BattleManager.StaticManager) && 
            CastedEvent.GetPlayerNewPokemon().GetIsEnemy() != IsPlayer &&
            CastedEvent.GetPlayerNewPokemon().HasItem("厚底靴") == false)
            {
                TargetPokemon = CastedEvent.GetPlayerNewPokemon();
                return true;
            }
            if(CastedEvent.GetEnemyNewPokemon() && 
            CastedEvent.GetEnemyNewPokemon().IsGroundPokemon(BattleManager.StaticManager) && 
            CastedEvent.GetEnemyNewPokemon().GetIsEnemy() != IsPlayer &&
            CastedEvent.GetEnemyNewPokemon().HasItem("厚底靴") == false)
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
        int baseDamage = TargetPokemon.GetMaxHP() / 8;
        if(Level == 1)
        {
            baseDamage = TargetPokemon.GetMaxHP() / 6;
        }
        if(Level == 2)
        {
            baseDamage = TargetPokemon.GetMaxHP() / 4;
        }
        int selfDamage = Math.Min(TargetPokemon.GetHP(), Math.Max(1, baseDamage));
        DamageEvent damageEvent = new DamageEvent(TargetPokemon, selfDamage, "撒菱");
        NewEvents.Add(damageEvent);
        return NewEvents;
    }
}

public class FutureAttackEnhancedStatusChange : BaseBattleFieldStatusChange
{
    private BattlePokemon SourcePokemon;
    public FutureAttackEnhancedStatusChange(BattlePokemon InSourcePokemon, bool InIsPlayer) : base(InIsPlayer)
    {
        SourcePokemon = InSourcePokemon;
    }

    public BattlePokemon GetSourcePokemon()
    {
        return SourcePokemon;
    }

    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(SourceEvent.GetEventType() == EventType.TurnEnd && TimePoint == ETimePoint.TurnEnd)
        {
            return true;
        }

        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        List<Event> NewEvents = new List<Event>();
        DamageSkill CastSkill = (DamageSkill)InManager.GetFutureAttackSkill();
        BattleSkill UseBattleSkill = new BattleSkill(InManager.GetFutureAttackSkill(), EMasterSkill.None, SourcePokemon);
        if(InManager.IsPokemonInField(SourcePokemon))
        {
            List<ETarget> TargetList = new List<ETarget>();
            if(IsPlayer)
            {   
                TargetList.Add(ETarget.P0);                
            }
            else
            {
                TargetList.Add(ETarget.E0);                
            }
            SkillEvent NewSkillEvent = new SkillEvent(InManager, UseBattleSkill, SourcePokemon, TargetList);
            NewEvents.Add(NewSkillEvent);
        }
        else
        {
            int Damage = 1;
            BattlePokemon TargetPokemon = InManager.GetBattlePokemons()[0];
            if(IsPlayer == false)
            {
                TargetPokemon = InManager.GetBattlePokemons()[1];
            }
            double Factor;            
            Damage = UseBattleSkill.DamagePhase(InManager, SourcePokemon, TargetPokemon, false, out Factor);
            if(Factor != 0.0)
            {
                DamageEvent damageEvent = new DamageEvent(TargetPokemon, Damage, "未来攻击", Factor);
                NewEvents.Add(damageEvent);
            }
        }
        return NewEvents;
    }

}
public class FutureAttackStatusChange : BaseBattleFieldStatusChange
{
    private BattlePokemon SourcePokemon;
    public FutureAttackStatusChange(BattlePokemon InSourcePokemon, bool InIsPlayer) : base(InIsPlayer)
    {
        SourcePokemon = InSourcePokemon;
    }

    public BattlePokemon GetSourcePokemon()
    {
        return SourcePokemon;
    }

    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(SourceEvent.GetEventType() == EventType.RemoveBattleFieldStatusChange && TimePoint == ETimePoint.BeforeRemoveBattleFieldStatusChange)
        {
            RemoveBattleFieldStatusChangeEvent CastedEvent = (RemoveBattleFieldStatusChangeEvent)SourceEvent;
            if(CastedEvent.GetReferenceFieldStatus().BaseStatusChange == this)
            {
                return true;
            }
        }

        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        List<Event> NewEvents = new List<Event>();
        DamageSkill CastSkill = (DamageSkill)InManager.GetFutureAttackSkill();
        BattleSkill UseBattleSkill = new BattleSkill(InManager.GetFutureAttackSkill(), EMasterSkill.None, SourcePokemon);
        if(InManager.IsPokemonInField(SourcePokemon))
        {
            List<ETarget> TargetList = new List<ETarget>();
            if(IsPlayer)
            {   
                TargetList.Add(ETarget.P0);                
            }
            else
            {
                TargetList.Add(ETarget.E0);                
            }
            SkillEvent NewSkillEvent = new SkillEvent(InManager, UseBattleSkill, SourcePokemon, TargetList);
            NewEvents.Add(NewSkillEvent);
        }
        else
        {
            int Damage = 1;
            BattlePokemon TargetPokemon = InManager.GetBattlePokemons()[0];
            if(IsPlayer == false)
            {
                TargetPokemon = InManager.GetBattlePokemons()[1];
            }
            double Factor;            
            Damage = UseBattleSkill.DamagePhase(InManager, SourcePokemon, TargetPokemon, false, out Factor);
            if(Factor == 0.0)
            {
                DamageEvent damageEvent = new DamageEvent(TargetPokemon, Damage, "未来攻击", Factor);
                NewEvents.Add(damageEvent);
            }
        }
        return NewEvents;
    }

}
public class WishStatusChange : BaseBattleFieldStatusChange
{
    private BattlePokemon SourcePokemon;
    public WishStatusChange(BattlePokemon InSourcePokemon, bool InIsPlayer) : base(InIsPlayer)
    {
        SourcePokemon = InSourcePokemon;
    }

    public BattlePokemon GetSourcePokemon()
    {
        return SourcePokemon;
    }

    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(SourceEvent.GetEventType() == EventType.RemoveBattleFieldStatusChange && TimePoint == ETimePoint.BeforeRemoveBattleFieldStatusChange)
        {
            RemoveBattleFieldStatusChangeEvent CastedEvent = (RemoveBattleFieldStatusChangeEvent)SourceEvent;
            if(CastedEvent.GetReferenceFieldStatus().BaseStatusChange == this)
            {
                return true;
            }
        }

        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        List<Event> NewEvents = new List<Event>();
        BattlePokemon TargetPokemon = InManager.GetBattlePokemons()[0];
        if(IsPlayer == false)
        {
            TargetPokemon = InManager.GetBattlePokemons()[1];
        }
        HealEvent healEvent = new HealEvent(TargetPokemon, SourcePokemon.GetMaxHP() / 2, "祈愿");
        NewEvents.Add(healEvent);
        return NewEvents;
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