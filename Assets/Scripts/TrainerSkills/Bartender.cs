using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bartender : BaseTrainerSkill
{
    private int Counter = 0;
    public void Start()
    {
        Counter = 0;
    }

    public override bool ShouldTrigger(ETimePoint TimePoint, Event SourceEvent)
    {
        if(TimePoint != ETimePoint.AfterSkillEffect)
        {
            return false;
        }

        if(SourceEvent.GetEventType() == EventType.UseSkill)
        {
            SkillEvent CastedEvent = (SkillEvent)SourceEvent;
            BattleManager ReferenceManager = CastedEvent.GetReferenceManager();
            BattlePokemon SourcePokemon = CastedEvent.GetSourcePokemon();
            BattleSkill ReferenceSkill = CastedEvent.GetSkill();
            if(ReferenceSkill.GetReferenceSkill().GetSkillType(SourcePokemon) == EType.Fire || 
               ReferenceSkill.GetReferenceSkill().GetSkillType(SourcePokemon) == EType.Water || 
               ReferenceSkill.GetReferenceSkill().GetSkillType(SourcePokemon) == EType.Grass)
            {
                return true;
            }
        }

        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        List<Event> NewEvents = new List<Event>();
        Counter += 1;
        if(Counter == 7)
        {
            System.Random rnd = new System.Random();
            int Random = rnd.Next(0, 6);
            for(int Index = 0; Index < 6; Index++)
            {
                int TarRandom = (Index + Random) % 6;
                BattlePokemon CurPokemon = InManager.GetBattlePokemons()[1];
                if(ReferenceTrainer.IsPlayer)
                {
                    CurPokemon = InManager.GetBattlePokemons()[0];
                }
                if(ReferenceTrainer.BattlePokemons[TarRandom].IsDead() == true && CurPokemon != ReferenceTrainer.BattlePokemons[TarRandom])
                {
                    BattlePokemon RevivePkm = ReferenceTrainer.BattlePokemons[TarRandom];
                    int NewHP = RevivePkm.GetMaxHP();
                    RevivePkm.Revive(NewHP);

                    List<string> Messages = new List<string>();
                    Messages.Add(RevivePkm.GetName() + "恢复了状态！");
                    NewEvents.Add(new MessageAnimationFakeEvent(Messages));
                    break;
                }
            }
            Counter = 0;
            List<string> ClearMessages = new List<string>();
            ClearMessages.Add("训练家技能-调酒师的计数清零了！");
            NewEvents.Add(new MessageAnimationFakeEvent(ClearMessages));
        }
        else
        {
            List<string> CounterMessages = new List<string>();
            CounterMessages.Add("训练家技能-调酒师的计数为" + Counter.ToString() + "！");
            NewEvents.Add(new MessageAnimationFakeEvent(CounterMessages));
        }
        return NewEvents;
    }

}
