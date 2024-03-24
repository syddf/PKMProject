using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectAutoLookAt : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target; // 目标Transform

    void Update()
    {
        if(target != null)
        {
            transform.LookAt(target.position); // 使当前GameObject的Z轴朝向目标Transform
        }
    }
}
