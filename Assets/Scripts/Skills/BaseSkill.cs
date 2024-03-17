using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    None,
    Normal,
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
    protected string SkillName;
    [SerializeField]
    protected string SkillDescription;
    protected int MinCount;
    protected int MaxCount;
    public ESkillClass GetSkillClass() => SkillClass;
    public int GetPower() => Power;
    public int GetPP() => PP;
    public int GetAccuracy() => Accuracy;
    public int GetMinCount() => MinCount;
    public int GetMaxCount() => MaxCount;
}
