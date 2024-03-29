using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonRotationAnimation : MonoBehaviour
{
    public float Duration = 0.3f;
    private float Timer = 0.0f;
    private bool Play = false;
    private Rigidbody RB;
    private Vector3 Pre;
    private Vector3 End;
    public void Update()
    {
        if(Play)
        {
            Timer += Time.deltaTime;
            float Ratio = Timer / Duration;
            if(Ratio <= 1.0f)
            {
                Vector3 newRotation = Vector3.Lerp(Pre, End, Ratio);
                this.gameObject.transform.eulerAngles = newRotation;

            }
            else
            {
                Play = false;
                Timer = 0.0f;
                this.gameObject.transform.eulerAngles = End;
            }
        }
    }

    public void BeginRotation()
    {
        Play = true;
        Timer = 0.0f;
        Pre = this.gameObject.transform.eulerAngles;
        End = this.gameObject.transform.eulerAngles;
        End.y = End.y + 360;
    }

    public void EndRotation()
    {
        Play = false;
        Timer = 0.0f;
        this.gameObject.transform.eulerAngles = Pre;
    }
}
