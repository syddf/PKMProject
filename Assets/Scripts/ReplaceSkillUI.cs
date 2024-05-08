using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceSkillUI : MonoBehaviour
{
    public GameObject Skill0Obj;
    public GameObject Skill1Obj;
    public GameObject Skill2Obj;
    public GameObject Skill3Obj;

    public GameObject Skill4Obj;
    public GameObject Skill5Obj;
    public GameObject Skill6Obj;
    public GameObject Skill7Obj;

    public GameObject SkillDesc1;
    public GameObject SkillDesc2;
    public SavedData PlayerData;

    public PokemonTrainer CurrentTrainer;
    public BagPokemon CurrentPokemon;

    public int Replace1Index;
    public int Replace2Index;

    public GameObject ConfirmButton;

    public void UpdateSkillUI(BagPokemon InPokemon, GameObject SkillObj, int SkillIndex)
    {
        if(InPokemon.GetSkillPoolSkill(SkillIndex) == null)
        {
            SkillObj.SetActive(false);
            return;
        }
        SkillObj.SetActive(true);
        SkillObj.GetComponent<SkillReplaceUI>().SetSkill(InPokemon.GetSkillPoolSkill(SkillIndex));
    }
    public void SetCurrentTrainer(PokemonTrainer InTrainer, BagPokemon InPokemon)
    {
        CurrentTrainer = InTrainer;
        CurrentPokemon = InPokemon;
        Replace1Index = -1;
        Replace2Index = -1;
        Skill0Obj.GetComponent<SkillReplaceUI>().Reset();
        Skill1Obj.GetComponent<SkillReplaceUI>().Reset();
        Skill2Obj.GetComponent<SkillReplaceUI>().Reset();
        Skill3Obj.GetComponent<SkillReplaceUI>().Reset();
        Skill4Obj.GetComponent<SkillReplaceUI>().Reset();
        Skill5Obj.GetComponent<SkillReplaceUI>().Reset();
        Skill6Obj.GetComponent<SkillReplaceUI>().Reset();
        Skill7Obj.GetComponent<SkillReplaceUI>().Reset();
        UpdateUI();
    }

    public void OnClickLine1(int Index)
    {
        Replace1Index = Index;
        Skill0Obj.GetComponent<SkillReplaceUI>().Reset();
        Skill1Obj.GetComponent<SkillReplaceUI>().Reset();
        Skill2Obj.GetComponent<SkillReplaceUI>().Reset();
        Skill3Obj.GetComponent<SkillReplaceUI>().Reset();
        UpdateUI();
    }

    public void OnClickLine2(int Index)
    {
        Replace2Index = Index;
        Skill4Obj.GetComponent<SkillReplaceUI>().Reset();
        Skill5Obj.GetComponent<SkillReplaceUI>().Reset();
        Skill6Obj.GetComponent<SkillReplaceUI>().Reset();
        Skill7Obj.GetComponent<SkillReplaceUI>().Reset();
        UpdateUI();
    }
    private int GetItemIndex(PokemonTrainer InTrainer, BaseItem InItem)
    {
        int Index = 0;
        for(Index = 0; Index < 6; Index++)
        {
            if(InTrainer.BagPokemons[Index].GetItem() == InItem)
            {
                return Index;
            }
        }
        return -1;
    }

    public int GetSkillIndex(BagPokemonOverrideData OverrideData, int Index)
    {
        if(OverrideData.SkillIndex0 == Index) return 0;
        if(OverrideData.SkillIndex1 == Index) return 1;
        if(OverrideData.SkillIndex2 == Index) return 2;
        if(OverrideData.SkillIndex3 == Index) return 3;
        if(OverrideData.SkillIndex4 == Index) return 4;
        if(OverrideData.SkillIndex5 == Index) return 5;
        if(OverrideData.SkillIndex6 == Index) return 6;
        if(OverrideData.SkillIndex7 == Index) return 7;
        return -1;
    }

    public void SetSkillIndex(ref BagPokemonOverrideData OverrideData, int Index, int NewIndex)
    {
        if(Index == 0) OverrideData.SkillIndex0 = NewIndex;
        if(Index == 1) OverrideData.SkillIndex1 = NewIndex;
        if(Index == 2) OverrideData.SkillIndex2 = NewIndex;
        if(Index == 3) OverrideData.SkillIndex3 = NewIndex;
        if(Index == 4) OverrideData.SkillIndex4 = NewIndex;
        if(Index == 5) OverrideData.SkillIndex5 = NewIndex;
        if(Index == 6) OverrideData.SkillIndex6 = NewIndex;
        if(Index == 7) OverrideData.SkillIndex7 = NewIndex;
    }

    public void WriteSkill()
    {
        BagPokemonOverrideData OverrideData = new BagPokemonOverrideData();
        OverrideData.Overrided = true;
        OverrideData.ReplaceTrainerName = CurrentTrainer.TrainerName;
        OverrideData.ReplacePokemonName = CurrentPokemon.GetPokemonName();
        OverrideData.SkillIndex0 = 0;
        OverrideData.SkillIndex1 = 1;
        OverrideData.SkillIndex2 = 2;
        OverrideData.SkillIndex3 = 3;
        OverrideData.SkillIndex4 = 4;
        OverrideData.SkillIndex5 = 5;
        OverrideData.SkillIndex6 = 6;
        OverrideData.SkillIndex7 = 7;
        OverrideData.Nature = CurrentPokemon.GetNature();
        OverrideData.ItemIndex = GetItemIndex(CurrentTrainer, CurrentPokemon.GetItem());

        if(PlayerData.SavedPlayerData.OverrideData.ContainsKey(CurrentTrainer.TrainerName))
        {
            if(PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName].ContainsKey(CurrentPokemon.GetPokemonName()))
            {
                OverrideData = PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName][CurrentPokemon.GetPokemonName()];
            }
            else
            {
                PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName].Add(CurrentPokemon.GetPokemonName(), OverrideData);
            }
        }
        else
        {
            PlayerData.SavedPlayerData.OverrideData.Add(CurrentTrainer.TrainerName, new SerializableDictionary<string, BagPokemonOverrideData>());
        }                

        int Ind1 = GetSkillIndex(OverrideData, Replace1Index);
        int Ind2 = GetSkillIndex(OverrideData, Replace2Index);
        SetSkillIndex(ref OverrideData, Ind1, Replace2Index);
        SetSkillIndex(ref OverrideData, Ind2, Replace1Index);
        PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName][CurrentPokemon.GetPokemonName()] = OverrideData;
    }

    public void UpdateUI()
    {
        int Skill0Index = 0;
        int Skill1Index = 1;
        int Skill2Index = 2;
        int Skill3Index = 3;
        int Skill4Index = 4;
        int Skill5Index = 5;
        int Skill6Index = 6;
        int Skill7Index = 7;

        BagPokemon ReferencePokemon = CurrentPokemon;
        if(PlayerData.SavedPlayerData.OverrideData.ContainsKey(CurrentTrainer.TrainerName))
        {
            if(PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName].ContainsKey(CurrentPokemon.GetPokemonName()))
            {
                if(PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName][CurrentPokemon.GetPokemonName()].Overrided)
                {
                    Skill0Index = PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName][CurrentPokemon.GetPokemonName()].SkillIndex0;
                    Skill1Index = PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName][CurrentPokemon.GetPokemonName()].SkillIndex1;
                    Skill2Index = PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName][CurrentPokemon.GetPokemonName()].SkillIndex2;
                    Skill3Index = PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName][CurrentPokemon.GetPokemonName()].SkillIndex3;
                    Skill4Index = PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName][CurrentPokemon.GetPokemonName()].SkillIndex4;
                    Skill5Index = PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName][CurrentPokemon.GetPokemonName()].SkillIndex5;
                    Skill6Index = PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName][CurrentPokemon.GetPokemonName()].SkillIndex6;
                    Skill7Index = PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName][CurrentPokemon.GetPokemonName()].SkillIndex7;

                    string TrainerName = PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName][CurrentPokemon.GetPokemonName()].ReplaceTrainerName;
                    string PokemonName = PlayerData.SavedPlayerData.OverrideData[CurrentTrainer.TrainerName][CurrentPokemon.GetPokemonName()].ReplacePokemonName;

                    GameObject TrainerObj = GameObject.Find("SingleBattle/AllTrainers/" + TrainerName);
                    PokemonTrainer Trainer = TrainerObj.GetComponent<PokemonTrainer>();
                    foreach(var BagPkm in Trainer.BagPokemons)
                    {
                        if(BagPkm.GetPokemonName() == PokemonName)
                        {
                            ReferencePokemon = BagPkm;
                            break;
                        }
                    }
                }
            }
        }

        int[] IndexArray = new int[8]{
            Skill0Index, 
            Skill1Index,
            Skill2Index, 
            Skill3Index,
            Skill4Index, 
            Skill5Index,
            Skill6Index, 
            Skill7Index,
        };

        UpdateSkillUI(ReferencePokemon, Skill0Obj, Skill0Index);
        UpdateSkillUI(ReferencePokemon, Skill1Obj, Skill1Index);
        UpdateSkillUI(ReferencePokemon, Skill2Obj, Skill2Index);
        UpdateSkillUI(ReferencePokemon, Skill3Obj, Skill3Index);
        UpdateSkillUI(ReferencePokemon, Skill4Obj, Skill4Index);
        UpdateSkillUI(ReferencePokemon, Skill5Obj, Skill5Index);
        UpdateSkillUI(ReferencePokemon, Skill6Obj, Skill6Index);
        UpdateSkillUI(ReferencePokemon, Skill7Obj, Skill7Index);

        if(Replace1Index != -1 && Replace2Index != -1)
        {
            SkillDesc1.SetActive(true);
            SkillDesc2.SetActive(true);
            SkillDesc1.GetComponent<SkillDescUI>().SetSkill(ReferencePokemon.GetSkillPoolSkill(IndexArray[Replace1Index]));
            SkillDesc2.GetComponent<SkillDescUI>().SetSkill(ReferencePokemon.GetSkillPoolSkill(IndexArray[Replace2Index]));
            ConfirmButton.SetActive(true);
        }
        else
        {
            SkillDesc1.SetActive(false);
            SkillDesc2.SetActive(false);
            ConfirmButton.SetActive(false);
        }
    }

    public void OnClickConfirm()
    {
        WriteSkill();
        UpdateUI();
    }

}
