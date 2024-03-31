using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonMoveAnimation : MonoBehaviour
{
    public float Duration = 0.5f;
    public float Speed = 1.0f;
    private bool Play = false;
    private Vector3 Origin;
    private Vector3 Prev;
    private bool Return;
    private Rigidbody RB;
    private float Timer = 0.0f;

    private List<GameObject> HideWhenTouched;

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

            InitPokemonComponets InitScript = child.GetComponent<InitPokemonComponets>();
            if(InitScript != null)
            {
                HideWhenTouched = InitScript.HideWhenTouched;
            }
        }
    }
    public void Update()
    {
        Timer += Time.deltaTime;
        if(Play)
        {
            if(Timer < Duration)
            {
                this.gameObject.transform.position = Vector3.Lerp(Prev, Origin, Timer / Duration);
            }
            else
            {
                this.gameObject.transform.position = Origin;
            }
            if(Timer >= Duration)
            {
                Play = false;
            }
        }
    }

    public void BeginMoveForward()
    {
        Prev = this.gameObject.transform.position;
        Origin = new Vector3(0, Prev.y, 0);
        RB.isKinematic = true; 
        Play = true;
        Return = false;
        Timer = 0.0f;

        foreach(var hideObj in HideWhenTouched)
        {
            hideObj.SetActive(false);
        }
    }

    public void EndMoveForward()
    {
        RB.isKinematic = false;
        Play = true;
        Timer = 0.0f;
        Return = true;
        Origin = Prev;
        Prev = this.gameObject.transform.position;
        foreach(var hideObj in HideWhenTouched)
        {
            hideObj.SetActive(true);
        }
    }

    public void BeginTouched()
    {
        RB.isKinematic = true; 
        foreach(var hideObj in HideWhenTouched)
        {
            hideObj.SetActive(false);
        }
    }
    public void EndTouched()
    {
        RB.isKinematic = false; 
        foreach(var hideObj in HideWhenTouched)
        {
            hideObj.SetActive(true);
        }
    }

}
