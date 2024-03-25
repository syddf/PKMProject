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
    protected EType SkillType;
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
    public EType GetSkillType() => SkillType;
    public ERange GetSkillRange() => SkillRange;
    public PlayableDirector GetSkillAnimation() => SkillAnimation;
    public virtual void AfterSkillEffectEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {

    }

    public virtual bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        return true;
    }

    public string GetSkillName() => SkillName;
}
