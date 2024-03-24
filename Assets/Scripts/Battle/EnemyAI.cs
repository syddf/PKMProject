using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        for(int Index = 0; Index < 6; Index++)
        {
            if(BattlePokemons[Index] != null && BattlePokemons[Index] != OutPokemon && BattlePokemons[Index].IsDead() == false)
            {
                return BattlePokemons[Index];
            }
        }
        return null;
    }

    public void AddUseSkillEvent(BaseSkill InSkill, List<Event> InEvents)
    {
        BattleSkill UseBattleSkill = new BattleSkill(InSkill, EMasterSkill.None, ReferencePokemon);
        if(InSkill.GetSkillRange() != ERange.None)
        {
            List<BattlePokemon> TargetList = ReferenceBattleManager.GetOpppoitePokemon(ReferencePokemon);
            InEvents.Add(new SkillEvent(UseBattleSkill, UseBattleSkill.GetReferencePokemon(), TargetList));
        }
        else
        {
            List<BattlePokemon> TargetList = new List<BattlePokemon>();
            InEvents.Add(new SkillEvent(UseBattleSkill, UseBattleSkill.GetReferencePokemon(), TargetList));
        }
    }

    public void GenerateEnemyEvent(List<Event> InEvents)
    {
        BaseSkill[] Skills = ReferencePokemon.GetReferenceSkill();
        for(int Index = 0; Index < 4; Index++)
        {
            if(Skills[Index] != null)
            {
                if(ReferencePokemon.GetSkillPP(Skills[Index]) > 0)
                {
                    AddUseSkillEvent(Skills[Index], InEvents);
                    return;
                }
            }
        }
    }
}
