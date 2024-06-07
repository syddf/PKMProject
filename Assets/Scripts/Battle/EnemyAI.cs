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
    private double GetLowLevelFactor(string SkillName)
    {
        if(SkillName == "近身战")
            return 0.1;
        if(ReferencePokemon.GetHP() >= (ReferencePokemon.GetMaxHP() / 2))
        {
            if(SkillName == "自我再生" || SkillName == "偷懒" || SkillName == "睡觉")
            {
                return 0.1;
            }
        }
        return 1.0;
    }
    public EnemyAI(BattlePokemon InPokemon, BattleManager InManager, PokemonTrainer InTrainer)
    {
        ReferencePokemon = InPokemon;
        ReferenceBattleManager = InManager;
        ReferenceTrainer = InTrainer;
    }

    public BattlePokemon GetNextPokemon(BattlePokemon OutPokemon)
    {
        TrainerSwitchAI SwitchAI = ReferenceTrainer.gameObject.GetComponent<TrainerSwitchAI>();
        if(SwitchAI)
        {
            return SwitchAI.GetNextPokemon(OutPokemon, ReferenceBattleManager, ReferenceTrainer);
        }
        BattlePokemon[] BattlePokemons = ReferenceTrainer.BattlePokemons;
        System.Random rnd = new System.Random();
        int RandNum = rnd.Next(0, 5);
        int FailedNum = 0;
        while(true)
        {
            if(BattlePokemons[RandNum] != null && BattlePokemons[RandNum] != OutPokemon && BattlePokemons[RandNum].IsDead() == false)
            {
                return BattlePokemons[RandNum];
            }
            RandNum = (RandNum + 1) % 5;
            FailedNum++;
            if(FailedNum >= 5)
            {
                break;
            }
        }
        return BattlePokemons[5];
    }

    public void AddUseSkillEvent(BaseSkill InSkill, List<Event> InEvents)
    {
        if(ReferencePokemon.CanMega())
        {
            InEvents.Add(new MegaEvent(ReferenceBattleManager, ReferencePokemon));
        }
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

    public double GetSkillPriorityFactor(BaseSkill InSkill, BattleManager InManager, BattlePokemon ReferencePokemon, Event InPlayerAction)
    {
        double Factor = 1.0;
        BagPokemonSkillAI PokemonAI = ReferencePokemon.GetSkillAI();
        if(PokemonAI)
        {
            Factor = Factor * PokemonAI.GetSkillPriorityFactor(InSkill, InManager, ReferencePokemon, InPlayerAction);
        }
        return Factor * GetLowLevelFactor(InSkill.GetSkillName());
    }

    private bool IsPlayerDangerous(Event InPlayerAction, BattleManager InManager)
    {
        SkillEvent CastEvent = (SkillEvent)InPlayerAction;
        if(CastEvent.GetSkill().IsDamageSkill())
        {
            BattleSkill ReferenceSkill = CastEvent.GetSkill();
            double Factor;
            int Damage = ReferenceSkill.DamagePhase(InManager, CastEvent.GetSourcePokemon(), ReferencePokemon, false, out Factor);
            if(Damage >= (ReferencePokemon.GetHP() * 0.8))
            {
                return true;
            }
        }
        return false;
    }

    public void GenerateEnemyEvent(List<Event> InEvents, BattleManager InManager, Event InPlayerAction)
    {
        if(ReferencePokemon.CanMega())
        {
            ReferencePokemon.LoadMegaStat();
        }
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

        if(ForbiddenSkillSet.Count == 4)
        {
            AddUseSkillEvent(InManager.GetStruggleSkill(), InEvents);
            return;
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
                    Entry.Priority = (int)(Entry.Priority * GetSkillPriorityFactor(Entry.ReferenceSkill, InManager, ReferencePokemon, InPlayerAction));
                    EntryList[Index] = Entry;
                }
                else
                {
                    BattleSkill UseBattleSkill = new BattleSkill(Entry.ReferenceSkill, EMasterSkill.None, TargetPokemon);
                    double Factor;
                    int Damage = UseBattleSkill.DamagePhase(InManager, ReferencePokemon, TargetPokemon, false, out Factor);
                    if(Damage >= TargetPokemon.GetHP())
                    {
                        List<ETarget> TargetList = new List<ETarget>();
                        TargetList.Add(ETarget.P0);
                        SkillEvent NewSkillEvent = new SkillEvent(ReferenceBattleManager, UseBattleSkill, ReferencePokemon, TargetList);
                        List<Event> TmpEvents = new List<Event>();
                        TmpEvents.Add(InPlayerAction);
                        TmpEvents.Add(NewSkillEvent);
                        TmpEvents.Sort(new EventComparer());
                        if(InPlayerAction.GetEventType() == EventType.Switch || IsPlayerDangerous(InPlayerAction, InManager) == false || TmpEvents[0] == NewSkillEvent)
                        {
                            KillSkillIndex.Add(Index);
                        }
                        Entry.Priority = (int)(999 * GetSkillPriorityFactor(Entry.ReferenceSkill, InManager, ReferencePokemon, InPlayerAction));
                    }
                    else if(Damage >= (TargetPokemon.GetMaxHP() / 2))
                    {
                        double Ratio = (double)Damage / TargetPokemon.GetMaxHP();
                        Entry.Priority = (int)(Mathf.Lerp(100.0f, 500.0f, (float)Ratio) * GetSkillPriorityFactor(Entry.ReferenceSkill, InManager, ReferencePokemon, InPlayerAction));
                    }
                    else
                    {
                        double Ratio = (double)Damage / (TargetPokemon.GetMaxHP() / 2);
                        Entry.Priority = (int)(Mathf.Lerp(10.0f, 100.0f, (float)Ratio) * GetSkillPriorityFactor(Entry.ReferenceSkill, InManager, ReferencePokemon, InPlayerAction));
                    }
                    EntryList[Index] = Entry;
                }
            }
        }

        if(KillSkillIndex.Count > 0)
        {
            int TotalValue = 0;
            for(int Index = 0; Index < KillSkillIndex.Count; Index++)
            {
                AISkillEntry Entry = EntryList[KillSkillIndex[Index]];
                TotalValue += Entry.Priority;
            }
            System.Random rnd = new System.Random();
            int RandNum = rnd.Next(0, TotalValue);

            int Accu = 0;
            for(int Index = 0; Index < KillSkillIndex.Count; Index++)
            {
                Accu += EntryList[KillSkillIndex[Index]].Priority;
                if(Accu >= RandNum)
                {
                    AddUseSkillEvent(EntryList[KillSkillIndex[Index]].ReferenceSkill, InEvents);
                    return;
                }
            }
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
