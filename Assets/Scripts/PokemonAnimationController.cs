using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonAnimationController : MonoBehaviour
{
    public Animator PkmAnimator;
    public void BeginAttack1()
    {
        PkmAnimator.SetBool("Attack1", true);
    }

    public void EndAttack1()
    {
        PkmAnimator.SetBool("Attack1", false);
    }

    public void BeginAttack2()
    {
        PkmAnimator.SetBool("Attack2", true);
    }

    public void EndAttack2()
    {
        PkmAnimator.SetBool("Attack2", false);
    }

    public void BeginTakenDamage()
    {
        PkmAnimator.SetBool("TakenDamage", true);
    }

    public void EndTakenDamage()
    {
        PkmAnimator.SetBool("TakenDamage", false);
    }

}
