using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodHammer : DamageAndSelfDamageSkill
{
    public override int SelfDamageRatio()
    {
        return 3;
    }
}
