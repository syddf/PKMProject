using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ParameterizedSignalReceiver : MonoBehaviour, INotificationReceiver
{
    public void OnNotify(Playable origin, INotification notification, object context)
    {
        if (notification is SignalWithParams)
        {
            SignalWithParams signal = (SignalWithParams)notification;
            Process(signal);
        }        
    }

    public virtual void Process(SignalWithParams InSignal)
    {
    }
}
