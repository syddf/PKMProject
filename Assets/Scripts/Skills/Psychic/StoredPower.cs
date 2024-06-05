using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoredPower : DamageSkill
{
    protected override int GetSkillPower(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        int Count = 0;
        int Level1 = SourcePokemon.GetAccuracyrateLevel(ECaclStatsMode.Normal);
        int Level2 = SourcePokemon.GetEvasionrateLevel(ECaclStatsMode.Normal);
        int Level3 = SourcePokemon.GetAtkChangeLevel();
        int Level4 = SourcePokemon.GetDefChangeLevel();
        int Level5 = SourcePokemon.GetSAtkChangeLevel();
        int Level6 = SourcePokemon.GetSDefChangeLevel();
        int Level7 = SourcePokemon.GetSpeedChangeLevel();
        if(Level1 > 0)
        {
            Count += Level1;
        }
        if(Level2 > 0)
        {
            Count += Level2;
        }
        if(Level3 > 0)
        {
            Count += Level3;
        }
        if(Level4 > 0)
        {
            Count += Level4;
        }
        if(Level5 > 0)
        {
            Count += Level5;
        }
        if(Level6 > 0)
        {
            Count += Level6;
        }
        if(Level7 > 0)
        {
            Count += Level7;
        }
        return Power + Count * 20;
    }
}
