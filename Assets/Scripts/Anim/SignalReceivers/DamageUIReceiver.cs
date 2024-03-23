using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class DamageUIReceiver : ParameterizedSignalReceiver
{
    public override void Process(SignalWithParams InSignal)
    {
        string DamageStr = InSignal.GetParamValue("Damage");
        string TargetPokemon = InSignal.GetParamValue("Target");
        string Effective = InSignal.GetParamValue("Effective");
        int Damage = Convert.ToInt32(DamageStr);
        BattleUI BattleUIScript = this.gameObject.GetComponent<BattleUI>();
        if(TargetPokemon == "Enemy1")
        {
            BattleUIScript.EnemyInfo1.DamageUI(Damage);
        }
        else if(TargetPokemon == "Player1")
        {
            BattleUIScript.PlayerInfo1.DamageUI(Damage);
        }
        if(Effective == "Super")
        {
            AudioManager.GetGlobalAudioManager().SuperEffectiveDamage.Play();
        }
        else if(Effective == "Normal")
        {
            AudioManager.GetGlobalAudioManager().NormalEffectiveDamage.Play();
        }
        else if(Effective == "Not")
        {
            AudioManager.GetGlobalAudioManager().NotEffectiveDamage.Play();
        }
    }
}
