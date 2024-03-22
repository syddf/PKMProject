using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;
using System;
using System.ComponentModel;

public class SignalWithParams : Marker, INotification, INotificationOptionProvider
{
    public string sigName;
    public string[] ParamsName = new string[16];
    public string[] ParamsValue = new string[16];
    public string GetParamValue(string InParamName)
    {
        for(int Index = 0; Index < 16; Index++)
        {
            if(ParamsName[Index] == InParamName)
            {
                return ParamsValue[Index];
            }
        }
        return "";
    }
    public PropertyName id { get; }
    NotificationFlags INotificationOptionProvider.flags => NotificationFlags.TriggerOnce | NotificationFlags.TriggerInEditMode;
}
