using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AudioReceiver : ParameterizedSignalReceiver
{
    public override void Process(SignalWithParams InSignal)
    {
        if(InSignal.sigName == "AudioSignal")
        {
            string PkmName = InSignal.GetParamValue("Pokemon");
            string Dead = InSignal.GetParamValue("Dead");
            bool IsDead = false;
            if(Dead == "True")
            {
                IsDead = true;
            }
            if(PkmName != "")
            {
                AudioManager.GetGlobalAudioManager().PlayPkmCry(PkmName, IsDead);
            }
        }
    }
}
