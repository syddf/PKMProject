using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerSkillTriggerReceiver : ParameterizedSignalReceiver
{
    public override void Process(SignalWithParams InSignal)
    {
        string TrainerSkillName = InSignal.GetParamValue("TrainerSkillName");
        AbilityStateInUI AbilityStateUI = this.gameObject.GetComponent<AbilityStateInUI>();
        AbilityStateUI.Text.text = TrainerSkillName;
    }
}
