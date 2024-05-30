using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectPkm : BagPokemonSkillAI
{
    public override double GetSkillPriorityFactor(BaseSkill InSkill, BattleManager InManager, BattlePokemon ReferencePokemon, Event InPlayerAction)
    {
        if(InSkill.GetSkillName() == "守住" && InPlayerAction.GetEventType() == EventType.UseSkill)
        {
            SkillEvent CastEvent = (SkillEvent)InPlayerAction;
            if(CastEvent.GetSkill().IsDamageSkill())
            {
                BattleSkill ReferenceSkill = CastEvent.GetSkill();
                double Factor;
                int Damage = ReferenceSkill.DamagePhase(InManager, CastEvent.GetSourcePokemon(), ReferencePokemon, false, out Factor);
                if(Damage > (ReferencePokemon.GetHP() / 2))
                {
                    return 999999.0;
                }
            }
        }
        return 1.0;
    }
}