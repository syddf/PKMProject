using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AISkillEntry
{
    public BaseSkill ReferenceSkill;
    public int Priority;
}

public class EnemyAI
{
    BattlePokemon ReferencePokemon;
    BattleManager ReferenceBattleManager;
    PokemonTrainer ReferenceTrainer;
    public EnemyAI(BattlePokemon InPokemon, BattleManager InManager, PokemonTrainer InTrainer)
    {
        ReferencePokemon = InPokemon;
        ReferenceBattleManager = InManager;
        ReferenceTrainer = InTrainer;
    }

    public BattlePokemon GetNextPokemon(BattlePokemon OutPokemon)
    {
        BattlePokemon[] BattlePokemons = ReferenceTrainer.BattlePokemons;
        while(true)
        {
            System.Random rnd = new System.Random();
            int RandNum = rnd.Next(0, 6);
            if(BattlePokemons[RandNum] != null && BattlePokemons[RandNum] != OutPokemon && BattlePokemons[RandNum].IsDead() == false)
            {
                return BattlePokemons[RandNum];
            }
        }
    }

    public void AddUseSkillEvent(BaseSkill InSkill, List<Event> InEvents)
    {
        BattleSkill UseBattleSkill = new BattleSkill(InSkill, EMasterSkill.None, ReferencePokemon);
        if(InSkill.GetSkillRange() != ERange.None)
        {
            List<ETarget> TargetList = new List<ETarget>();
            TargetList.Add(ETarget.P0);
            InEvents.Add(new SkillEvent(ReferenceBattleManager, UseBattleSkill, UseBattleSkill.GetReferencePokemon(), TargetList));
        }
        else
        {
            List<ETarget> TargetList = new List<ETarget>();
            TargetList.Add(ETarget.None);
            InEvents.Add(new SkillEvent(ReferenceBattleManager, UseBattleSkill, UseBattleSkill.GetReferencePokemon(), TargetList));
        }
    }

    public void GenerateEnemyEvent(List<Event> InEvents, BattleManager InManager, Event InPlayerAction)
    {
        HashSet<BaseSkill> ForbiddenSkillSet = ReferencePokemon.GetForbiddenBattleSkills(InManager);
        BaseSkill[] Skills = ReferencePokemon.GetReferenceSkill();
        List<AISkillEntry> EntryList = new List<AISkillEntry>();
        BattlePokemon TargetPokemon = InManager.GetTargetPokemon(ETarget.P0);

        if(InPlayerAction.GetEventType() == EventType.Switch)
        {
            SwitchEvent CastedEvent = (SwitchEvent)InPlayerAction;
            System.Random rnd = new System.Random();
            int RandNum = rnd.Next(0, 2);
            if(RandNum == 0)
            {
                TargetPokemon = CastedEvent.GetInPokemon();
            }
        }

        for(int Index = 0; Index < 4; Index++)
        {
            if(!ForbiddenSkillSet.Contains(Skills[Index]))
            {
                AISkillEntry SkillEntry = new AISkillEntry();
                SkillEntry.ReferenceSkill = Skills[Index];
                SkillEntry.Priority = 100;
                string Reason = "";
                if(!Skills[Index].JudgeIsEffective(InManager, ReferencePokemon, TargetPokemon, out Reason))
                {
                    SkillEntry.Priority = -999;
                }
                EntryList.Add(SkillEntry);
            }
        }

        List<int> KillSkillIndex = new List<int>();

        for(int Index = 0; Index < EntryList.Count; Index++)
        {
            AISkillEntry Entry = EntryList[Index];
            if(Entry.Priority != -999)
            {
                ESkillClass skillClass = Entry.ReferenceSkill.GetSkillClass();
                if(skillClass == ESkillClass.StatusMove)
                {

                }
                else
                {
                    BattleSkill UseBattleSkill = new BattleSkill(Entry.ReferenceSkill, EMasterSkill.None, TargetPokemon);
                    double Factor;
                    int Damage = UseBattleSkill.DamagePhase(InManager, ReferencePokemon, TargetPokemon, false, out Factor);
                    if(Damage >= TargetPokemon.GetHP())
                    {
                        KillSkillIndex.Add(Index);
                        Entry.Priority = 999;
                    }
                    else if(Damage >= (TargetPokemon.GetMaxHP() / 2))
                    {
                        double Ratio = (double)Damage / TargetPokemon.GetMaxHP();
                        Entry.Priority = (int)Mathf.Lerp(100.0f, 500.0f, (float)Ratio);
                    }
                    else
                    {
                        double Ratio = (double)Damage / (TargetPokemon.GetMaxHP() / 2);
                        Entry.Priority = (int)Mathf.Lerp(10.0f, 100.0f, (float)Ratio);
                    }
                    EntryList[Index] = Entry;
                }
            }
        }

        if(KillSkillIndex.Count > 0)
        {
            System.Random rnd = new System.Random();
            int RandNum = rnd.Next(0, KillSkillIndex.Count);
            int SkillIndex = KillSkillIndex[RandNum];
            AddUseSkillEvent(EntryList[SkillIndex].ReferenceSkill, InEvents);
            return;
        }
        else
        {
            List<AISkillEntry> SkillPriorityLargeThanZero = new List<AISkillEntry>();
            int TotalValue = 0;
            for(int Index = 0; Index < EntryList.Count; Index++)
            {
                AISkillEntry Entry = EntryList[Index];
                if(Entry.Priority > 0)
                {
                    SkillPriorityLargeThanZero.Add(Entry);
                    TotalValue += Entry.Priority;
                }
            }

            if(TotalValue > 0)
            {
                System.Random rnd = new System.Random();
                int RandNum = rnd.Next(0, TotalValue);

                int Accu = 0;
                for(int Index = 0; Index < SkillPriorityLargeThanZero.Count; Index++)
                {
                    Accu += SkillPriorityLargeThanZero[Index].Priority;
                    if(Accu >= RandNum)
                    {
                        AddUseSkillEvent(SkillPriorityLargeThanZero[Index].ReferenceSkill, InEvents);
                        return;
                    }
                }
            }
            else
            {
                System.Random rnd = new System.Random();
                int RandNum = rnd.Next(0, EntryList.Count);
                AddUseSkillEvent(EntryList[RandNum].ReferenceSkill, InEvents);
                return;
            }
        }
    }
}
