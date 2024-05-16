using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public enum EMasterSkill
{
    None,
    Strong,
    Agile
}

public enum ESkillClass
{
    PhysicalMove,
    SpecialMove,
    StatusMove
}

public enum EType
{
    None = -1,
    Normal = 0,
    Fighting,
    Flying,
    Poison,
    Ground,
    Rock,
    Bug,
    Ghost,
    Steel,
    Fire,
    Water,
    Grass,
    Electric,
    Psychic,
    Ice,
    Dragon,
    Dark,
    Fairy
}

public enum ERange
{
    None,
    Single,
    AllWithoutTeammates,
    AllWithTeammates
}

public class BaseSkill : MonoBehaviour
{
    [SerializeField]
    protected int Power;
    [SerializeField]
    protected int PP;
    [SerializeField]    
    protected int Accuracy;
    [SerializeField]
    protected ESkillClass SkillClass;
    [SerializeField]
    private EType SkillType;
    [SerializeField]
    protected ERange SkillRange;
    [SerializeField]
    protected string SkillName;
    [SerializeField]
    protected string SkillDescription;
    [SerializeField]
    protected int MinCount;
    [SerializeField]
    protected int MaxCount;
    [SerializeField]
    protected int Priority = 0;
    [SerializeField]
    protected bool AlwaysHit = false;
    [SerializeField]
    public PlayableDirector SkillAnimation;
    public ESkillClass GetSkillClass() => SkillClass;
    public virtual bool GetAlwaysHit(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon) => AlwaysHit;
    public int GetPower() => Power;
    public int GetPP() => PP;
    public int GetAccuracy() => Accuracy;
    public int GetMinCount() => MinCount;
    public int GetMaxCount() => MaxCount;
    public ERange GetSkillRange() => SkillRange;
    public PlayableDirector GetSkillAnimation() => SkillAnimation;
    public virtual int GetCTRatio()
    {
        return 0;
    }
    public virtual int GetAccuracy(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        return Accuracy;
    }
    public ECaclStatsMode GetAttackAccuracyChangeLevelMode(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        if(TargetPokemon.HasAbility("纯朴", InManager, SourcePokemon, TargetPokemon))
        {
            return ECaclStatsMode.IgnoreBuf;
        }
        return ECaclStatsMode.Normal;
    }

    public ECaclStatsMode GetTargetEvasionChangeLevelMode(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        if(TargetPokemon.HasAbility("纯朴", InManager, SourcePokemon, TargetPokemon))
        {
            return ECaclStatsMode.IgnoreDebuf;
        }
        return ECaclStatsMode.Normal;
    }

    public int GetAttackAccuracyChangeLevel(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        return SourcePokemon.GetAccuracyrateLevel(GetAttackAccuracyChangeLevelMode(InManager, SourcePokemon, TargetPokemon));
    }

    public int GetTargetEvasionChangeLevel(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        return TargetPokemon.GetEvasionrateLevel(GetTargetEvasionChangeLevelMode(InManager, SourcePokemon, TargetPokemon));
    }

    public virtual void AfterSkillEffectEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {

    }

    public virtual int AfterSkillEffectEventProbablity(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        return 100;
    }

    public virtual bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, out string Reason)
    {
        Reason = "";
        return true;
    }

    public virtual int GetSkillPriority(BattleManager InManager,BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        return Priority;
    }

    public virtual bool CanBeProtected()
    {
        return true;
    }

    public virtual bool HasHealEffect(BattleManager InManager)
    {
        return false;
    }
    public EType GetOriginSkillType()
    {
        return SkillType;
    }
    public EType GetSkillType(BattlePokemon InPokemon)
    {
        if(InPokemon.HasAbility("妖精皮肤", null, null, InPokemon) && SkillType == EType.Normal)
        {
            return EType.Fairy;
        }
        if(InPokemon.HasAbility("冰冻皮肤", null, null, InPokemon) && SkillType == EType.Normal)
        {
            return EType.Ice;
        }
        if(InPokemon.HasAbility("湿润之声", null, null, InPokemon) && BattleSkillMetaInfo.IsSoundSkill(SkillName))
        {
            return EType.Water;
        }
        return SkillType;  
    }
    public string GetSkillName() => SkillName;
    public string GetSkillDesc() => SkillDescription;
}
