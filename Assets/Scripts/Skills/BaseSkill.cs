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
    Fight,
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
    protected PlayableDirector SkillAnimation;
    public ESkillClass GetSkillClass() => SkillClass;
    public int GetPower() => Power;
    public int GetPP() => PP;
    public int GetAccuracy() => Accuracy;
    public int GetMinCount() => MinCount;
    public int GetMaxCount() => MaxCount;
    public ERange GetSkillRange() => SkillRange;
    public PlayableDirector GetSkillAnimation() => SkillAnimation;

    public ECaclStatsMode GetAttackAccuracyChangeLevelMode(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        if(TargetPokemon.HasAbility("纯朴"))
        {
            return ECaclStatsMode.IgnoreBuf;
        }
        return ECaclStatsMode.Normal;
    }

    public ECaclStatsMode GetTargetEvasionChangeLevelMode(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        if(TargetPokemon.HasAbility("纯朴"))
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

    public virtual bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        return true;
    }

    public EType GetOriginSkillType()
    {
        return SkillType;
    }
    public EType GetSkillType(BattlePokemon InPokemon)
    {
        if(InPokemon.HasAbility("妖精皮肤") && SkillType == EType.Normal)
        {
            return EType.Fairy;
        }
        if(InPokemon.HasAbility("冰冻皮肤") && SkillType == EType.Normal)
        {
            return EType.Ice;
        }
        if(InPokemon.HasAbility("湿润之声") && BattleSkillMetaInfo.IsSoundSkill(SkillName))
        {
            return EType.Water;
        }
        return SkillType;  
    }
    public string GetSkillName() => SkillName;
}
