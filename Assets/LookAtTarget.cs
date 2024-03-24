using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    public Transform target; // 目标Transform

    void Update()
    {
        if(target != null)
        {
            transform.LookAt(target.position); // 使当前GameObject的Z轴朝向目标Transform
        }
    }
}
