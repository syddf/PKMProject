using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScienceChangeFuture : BaseTrainerSkill
{
    private bool HasTriggered;
    
    public void Start()
    {
        HasTriggered = false;
    }
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(HasTriggered)
        {
            return false;
        }
        
        if(TimePoint != ETimePoint.AfterSkillEffect)
        {
            return false;
        }

        if(SourceEvent.GetEventType() == EventType.UseSkill)
        {
            SkillEvent CastedEvent = (SkillEvent)SourceEvent;
            BattleManager ReferenceManager = CastedEvent.GetReferenceManager();
            if(CastedEvent.GetSourcePokemon().GetIsEnemy() != ReferenceTrainer.IsPlayer)
            {
                if(CastedEvent.GetSkill().GetReferenceSkill().GetSkillType(CastedEvent.GetSourcePokemon()) == EType.Electric)
                {
                    return ReferenceManager.GetTerrainType() != EBattleFieldTerrain.Electric;
                }
            }
        }

        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        List<Event> NewEvents = new List<Event>();
        SkillEvent CastedEvent = (SkillEvent)SourceEvent;
        TerrainChangeEvent newEvent = new TerrainChangeEvent(CastedEvent.GetSourcePokemon(), InManager, EBattleFieldTerrain.Electric);
        NewEvents.Add(newEvent);
        HasTriggered = true;
        return NewEvents;
    }

}
