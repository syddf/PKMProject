using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonReceiver : ParameterizedSignalReceiver
{
    public Transform BodyTransform;
    public override void Process(SignalWithParams InSignal)
    {
        string NewState = InSignal.GetParamValue("AnimationState");
        PokemonAnimationController AnimController = this.gameObject.GetComponent<PokemonAnimationController>();
        if(NewState == "BeginAttack1")
        {
            AnimController.BeginAttack1();
        }
        if(NewState == "EndAttack1")
        {
            AnimController.EndAttack1();
        }
        if(NewState == "BeginAttack2")
        {
            AnimController.BeginAttack2();
        }
        if(NewState == "EndAttack2")
        {
            AnimController.EndAttack2();
        }
        if(NewState == "BeginTakenDamage")
        {
            AnimController.BeginTakenDamage();
        }
        if(NewState == "EndTakenDamage")
        {
            AnimController.EndTakenDamage();
        }
    }
}
