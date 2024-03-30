using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildCharge : DamageAndSelfDamageSkill
{
    public override int SelfDamageRatio()
    {
        return 4;
    }
}
