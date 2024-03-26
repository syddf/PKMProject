using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionWithObject : MonoBehaviour
{
    public Transform target; // 目标Transform
    void OnEnable()
    {
        if(target != null)
        {
            transform.position = target.position; // 使当前GameObject的Z轴朝向目标Transform
        }
    }
    void Update()
    {
        if(target != null)
        {
            transform.position = target.position; // 使当前GameObject的Z轴朝向目标Transform
        }
    }
}
