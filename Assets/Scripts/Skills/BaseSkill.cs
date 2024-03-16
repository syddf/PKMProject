using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESkillClass
{
    PhysicalMove,
    SpecialMove,
    StatusMove
}

public enum EType
{
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
    [SerializeField]
    protected BattlePokemon ReferencePokemon;
}
