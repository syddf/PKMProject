using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTriggerSignalReceiver : ParameterizedSignalReceiver
{
    public override void Process(SignalWithParams InSignal)
    {
        string AbilityName = InSignal.GetParamValue("AbilityName");
        AbilityStateInUI AbilityStateUI = this.gameObject.GetComponent<AbilityStateInUI>();
        AbilityStateUI.Text.text = AbilityName;
        int Index = Convert.ToInt32(InSignal.GetParamValue("PokemonIndex"));
        AbilityStateUI.Sprite.sprite = PokemonSpritesManager.PKMSprites[Index];
    }
}
