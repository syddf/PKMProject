using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotationY : MonoBehaviour
{
    public Transform TargetTransform;
    public void OnEnable()
    {
        if(TargetTransform.position.z < 0)
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
