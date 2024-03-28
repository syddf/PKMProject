using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraShakeReceiver : ParameterizedSignalReceiver
{
    public override void Process(SignalWithParams InSignal)
    {
        string IntensityText = InSignal.GetParamValue("Intensity");
        float Intensity = Convert.ToSingle(IntensityText);
        string TimeText = InSignal.GetParamValue("Time");
        float Time = Convert.ToSingle(TimeText);
        CameraShake.Instance.ShakeCamera(Intensity, Time);
    }
}
