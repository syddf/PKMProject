using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurroundSound : BaseTrainerSkill
{
    private int SoundCount = 0;
    
    public void Start()
    {
        SoundCount = 0;
    }
    public override double GetPowerFactorWhenAttack(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, DamageSkill InSkill)
    {
        if(SourcePokemon.GetIsEnemy() != ReferenceTrainer.IsPlayer && BattleSkillMetaInfo.IsSoundSkill(InSkill.GetSkillName()))
        {
            return 1.0 + SoundCount * 0.2;
        }
        return 1.0;
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
            if(CastedEvent.GetSourcePokemon().GetIsEnemy() != ReferenceTrainer.IsPlayer)
            {
                if(BattleSkillMetaInfo.IsSoundSkill(CastedEvent.GetSkill().GetSkillName()))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        List<Event> NewEvents = new List<Event>();
        SoundCount += 1;
        List<string> Messages = new List<string>();
        string Player = "对方";
        if(ReferenceTrainer.IsPlayer)
        {
            Player = "己方";
        }
        Messages.Add(ReferenceTrainer.TrainerName + "聚合了宝可梦的歌声！");
        Messages.Add(Player + "宝可梦声音类招式已经获得了" + SoundCount.ToString() + "次强化！");
        MessageAnimationFakeEvent FakeEvent = new MessageAnimationFakeEvent(Messages);
        NewEvents.Add(FakeEvent);
        return NewEvents;
    }

}
