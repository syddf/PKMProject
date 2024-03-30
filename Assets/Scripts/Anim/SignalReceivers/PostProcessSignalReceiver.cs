using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessSignalReceiver : ParameterizedSignalReceiver
{
    public GameObject NormalVolume;
    public GameObject DarkTimePauseVolume;
    public override void Process(SignalWithParams InSignal)
    {
        string VolumeState = InSignal.GetParamValue("VolumeState");
        if(VolumeState == "DarkTimePause")
        {
            DarkTimePauseVolume.SetActive(true);
            StopTime();
        }
        if(VolumeState == "Reset")
        {
            DarkTimePauseVolume.SetActive(false);
        }
    }

    public float stopDuration = 0.5f;

    // 用于启动时停效果的方法
    public void StopTime()
    {
        StartCoroutine(TimeStopCoroutine());
    }

    // 协程实现短暂的时停
    private IEnumerator TimeStopCoroutine()
    {
        // 将时间缩放设置为0，停止时间
        Time.timeScale = 0;

        // 等待一段真实时间（即便Time.timeScale为0，WaitForSecondsRealtime仍然有效）
        yield return new WaitForSecondsRealtime(stopDuration);

        // 恢复时间缩放为1，恢复正常时间流逝
        Time.timeScale = 1;
    }
}
