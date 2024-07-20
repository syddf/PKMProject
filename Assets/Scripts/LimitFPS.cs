using UnityEngine;

public class LimitFPS : MonoBehaviour
{
    void Start()
    {
        // 将目标帧率设置为 120
        Application.targetFrameRate = 120;
        if (!Application.isEditor)
        {
            // 禁止在后台运行
            Application.runInBackground = false;
        }
    }
}