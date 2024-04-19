using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonScaleAnimation : MonoBehaviour
{
    public float Duration = 0.2f;
    private float Timer = 0.0f;
    private Vector3 Big = new Vector3(1.0f, 1.0f, 1.0f);
    private Vector3 Small = new Vector3(0.0f, 0.0f, 0.0f);
    private bool Play = false;
    private Rigidbody RB;
    private Vector3 Pre;
    private Vector3 End;
    private bool ResetRigidBody;
    public void Start()
    {
        Transform[] children = GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            Rigidbody rb = child.GetComponent<Rigidbody>();
            if (rb != null)
            {
                RB = rb;
            }
        }
    }
    public void Update()
    {
        if(Play)
        {
            Timer += Time.deltaTime;
            float Ratio = Timer / Duration;
            if(Ratio <= 1.0f)
            {
                Vector3 newScale = Vector3.Lerp(Pre, End, Ratio);
                this.gameObject.transform.localScale = newScale;

            }
            else
            {
                if(ResetRigidBody)
                {
                    RB.isKinematic = false; 
                }
                this.gameObject.transform.localScale = End;
                Play = false;
                Timer = 0.0f;
            }
        }
    }

    public void ToSmall()
    {
        RB.isKinematic = true; 
        ResetRigidBody = false;
        Play = true;
        Timer = 0.0f;
        Pre = Big;
        End = Small;
    }

    public void ToBig()
    {
        RB.isKinematic = true; 
        ResetRigidBody = true;
        Play = true;
        Timer = 0.0f;
        Pre = Small;
        End = Big;
    }
}
