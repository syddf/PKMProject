using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonAnimationController : MonoBehaviour
{
    public float Duration = 0.5f;
    private float Timer = 0.0f;
    private Vector3 Big = new Vector3(1.0f, 1.0f, 1.0f);
    private Vector3 Small = new Vector3(0.2f, 0.2f, 0.2f);
    private bool Play = false;
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

    public void Defeated()
    {
        PkmAnimator.SetBool("Dead", true);
    }
    public void EndDefeated()
    {
        PkmAnimator.SetBool("Dead", false);
    }
    public void Reset()
    {
        PkmAnimator.Play("Entry", 0, 0.0f);
        PkmAnimator.Update(0f);
    }

    public void Update()
    {
        if(Play)
        {
            Timer += Time.deltaTime;
            float Ratio = Timer / Duration;
            if(Ratio <= 1.0f)
            {
                Vector3 newScale = Vector3.Lerp(Big, Small, Ratio);
                this.gameObject.transform.localScale = newScale;
                float newHeight = newScale.y * 1.5f;
                float newBottomYPosition = 0.0f + newHeight;
                this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, newBottomYPosition, this.gameObject.transform.position.z);

            }
            else
            {
                Play = false;
                Timer = 0.0f;
            }
        }
    }
    public void ReturnToBall()
    {
        Play = true;
    }
}
