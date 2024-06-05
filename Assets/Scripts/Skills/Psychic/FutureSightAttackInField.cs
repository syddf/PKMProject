using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FutureSightAttackInField : DamageSkill
{
    public override bool CanBeProtected()
    {
        return false;
    }
}
