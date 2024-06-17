using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmileBlade : BaseTrainerSkill
{
    private bool[] Triggered = new bool[6];
    public SmileBlade()
    {
        Triggered[0] = false;
        Triggered[1] = false;
        Triggered[2] = false;
        Triggered[3] = false;
        Triggered[4] = false;
        Triggered[5] = false;
    }
    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(TimePoint != ETimePoint.AfterTakenDamage)
        {
            return false;
        }

        if(SourceEvent.GetEventType() == EventType.UseSkill)
        {
            SkillEvent CastedEvent = (SkillEvent)SourceEvent;
            int Damage = CastedEvent.GetCurrentDamage();
            BattlePokemon SkillReferencePokemon = CastedEvent.GetCurrentProcessTargetPokemon();
            BattlePokemon SourcePokemon = CastedEvent.GetSourcePokemon();
            if(Damage > 0 && 
               SkillReferencePokemon.GetReferenceTrainer() == this.ReferenceTrainer && 
               SourcePokemon.IsDead() == false && BattleManager.StaticManager.IsPokemonInField(SourcePokemon) &&
               Triggered[SkillReferencePokemon.GetReferenceTrainer().GetPokemonIndex(SkillReferencePokemon)] == false)
            {
                EStatusChange []AllStatus = new EStatusChange[5];
                AllStatus[0] = EStatusChange.Poison;
                AllStatus[1] = EStatusChange.Paralysis;
                AllStatus[2] = EStatusChange.Drowsy;
                AllStatus[3] = EStatusChange.Frostbite;
                AllStatus[4] = EStatusChange.Burn;
                bool CanTrigger = false;
                for(int Index = 0; Index < 5; Index ++)
                {
                    EStatusChange NewStatus = AllStatus[Index];
                    if(SetPokemonStatusChangeEvent.IsStatusChangeEffective(BattleManager.StaticManager, SourcePokemon, null, NewStatus) == true)
                    {
                        CanTrigger = true;
                        break;
                    }
                }
                if(CanTrigger)
                {
                    Triggered[SkillReferencePokemon.GetReferenceTrainer().GetPokemonIndex(SkillReferencePokemon)] = true;
                    return true;
                }
            }
        }

        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        SkillEvent CastedEvent = (SkillEvent)SourceEvent;
        BattlePokemon SourcePokemon = CastedEvent.GetSourcePokemon();
        List<Event> NewEvents = new List<Event>();

        System.Random rnd = new System.Random();
        int Random = rnd.Next(0, 5);
        EStatusChange []AllStatus = new EStatusChange[5];
        AllStatus[0] = EStatusChange.Poison;
        AllStatus[1] = EStatusChange.Paralysis;
        AllStatus[2] = EStatusChange.Drowsy;
        AllStatus[3] = EStatusChange.Frostbite;
        AllStatus[4] = EStatusChange.Burn;

        EStatusChange NewStatus = AllStatus[Random];

        for(int Index = 0; Index < 5; Index ++)
        {
            if(SetPokemonStatusChangeEvent.IsStatusChangeEffective(InManager, SourcePokemon, null, NewStatus) == false)
            {
                Random = (Random + 1) % 5;
                NewStatus = AllStatus[Random];
            }
            else
            {
                break;
            }
        }

        SetPokemonStatusChangeEvent setStatChangeEvent = new SetPokemonStatusChangeEvent(SourcePokemon, null, InManager, NewStatus, 1, false);
        NewEvents.Add(setStatChangeEvent);
        return NewEvents;
    }

}
