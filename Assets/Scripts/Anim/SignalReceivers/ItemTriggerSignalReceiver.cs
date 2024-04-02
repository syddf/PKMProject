using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTriggerSignalReceiver : ParameterizedSignalReceiver
{
    public override void Process(SignalWithParams InSignal)
    {
        string ItemName = InSignal.GetParamValue("ItemName");
        AbilityStateInUI AbilityStateUI = this.gameObject.GetComponent<AbilityStateInUI>();
        AbilityStateUI.Text.text = ItemName;
        AbilityStateUI.Sprite.sprite = PokemonSpritesManager.ItemSprites[ItemName];
    }
}
