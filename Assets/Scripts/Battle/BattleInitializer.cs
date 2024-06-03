using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class BattleInitializer : MonoBehaviour
{
    public GameObject BattlePokemonRoot;
    public GameObject ModelsRoot;
    public GameObject SkillsRoot;
    public GameObject TrainerSkillsRoot;
    public GameObject AbilitiesSkillRoot;
    public GameObject ItemsRoot;
    public Transform PlayerTransform;
    public Transform EnemyTransform;

    public GameObject SkillAnimationUIReceiver;
    public GameObject SkillAnimationCameraShakeReceiver;
    public GameObject PostProcessVolumeReceiver;
    public List<GameObject> CreatedGameObject = new List<GameObject>();
    private PokemonTrainer CurrentPlayerTrainer;
    private PokemonTrainer CurrentEnemyTrainer;
    public List<GameObject> PlayerOriginalBagPokemons = new List<GameObject>();
    public List<GameObject> EnemyOriginalBagPokemons = new List<GameObject>();
    public List<BaseItem> GemItemList = new List<BaseItem>();
    public void InitBattleResources(PokemonTrainer PlayerTrainer, PokemonTrainer EnemyTrainer)
    {
        CurrentPlayerTrainer = PlayerTrainer;
        CurrentEnemyTrainer = EnemyTrainer;
        PlayerOriginalBagPokemons.Clear();
        EnemyOriginalBagPokemons.Clear();
        PlayerOriginalBagPokemons.Add(PlayerTrainer.BagPokemons[0].gameObject);
        PlayerOriginalBagPokemons.Add(PlayerTrainer.BagPokemons[1].gameObject);
        PlayerOriginalBagPokemons.Add(PlayerTrainer.BagPokemons[2].gameObject);
        PlayerOriginalBagPokemons.Add(PlayerTrainer.BagPokemons[3].gameObject);
        PlayerOriginalBagPokemons.Add(PlayerTrainer.BagPokemons[4].gameObject);
        PlayerOriginalBagPokemons.Add(PlayerTrainer.BagPokemons[5].gameObject);
        EnemyOriginalBagPokemons.Add(EnemyTrainer.BagPokemons[0].gameObject);
        EnemyOriginalBagPokemons.Add(EnemyTrainer.BagPokemons[1].gameObject);
        EnemyOriginalBagPokemons.Add(EnemyTrainer.BagPokemons[2].gameObject);
        EnemyOriginalBagPokemons.Add(EnemyTrainer.BagPokemons[3].gameObject);
        EnemyOriginalBagPokemons.Add(EnemyTrainer.BagPokemons[4].gameObject);
        EnemyOriginalBagPokemons.Add(EnemyTrainer.BagPokemons[5].gameObject);

        for(int Index = 0; Index < 6; Index++)
        {
            PlayerTrainer.BagPokemons[Index] = Instantiate(PlayerTrainer.BagPokemons[Index].gameObject, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<BagPokemon>();;
            EnemyTrainer.BagPokemons[Index] = Instantiate(EnemyTrainer.BagPokemons[Index].gameObject, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<BagPokemon>();;
        }

        PlayerTrainer.IsPlayer = true;
        EnemyTrainer.IsPlayer = false;
        if(PlayerTrainer.TrainerSkill)
        {
            GameObject PlayerTrainerSkillObj = Instantiate(PlayerTrainer.TrainerSkill.gameObject, new Vector3(0, 0, 0), Quaternion.identity);
            PlayerTrainer.TrainerSkill = PlayerTrainerSkillObj.GetComponent<BaseTrainerSkill>();
            PlayerTrainerSkillObj.transform.parent = TrainerSkillsRoot.transform;
            CreatedGameObject.Add(PlayerTrainerSkillObj);
        }
        if(EnemyTrainer.TrainerSkill)
        {
            GameObject EnemyTrainerSkillObj = Instantiate(EnemyTrainer.TrainerSkill.gameObject, new Vector3(0, 0, 0), Quaternion.identity);
            EnemyTrainer.TrainerSkill = EnemyTrainerSkillObj.GetComponent<BaseTrainerSkill>();
            EnemyTrainerSkillObj.transform.parent = TrainerSkillsRoot.transform;
            CreatedGameObject.Add(EnemyTrainerSkillObj);
        }
        PlayerTrainer.TrainerSkill.SetTrainer(PlayerTrainer);
        EnemyTrainer.TrainerSkill.SetTrainer(EnemyTrainer);
            
        for(int Index = 0; Index < 6; Index++)
        {
            SpawnItem(PlayerTrainer, Index);
            SpawnItem(EnemyTrainer, Index);
        }
        for(int Index = 0; Index < 6; Index++)
        {
            SpawnBattlePokemon(PlayerTrainer, Index);
            SpawnBattlePokemon(EnemyTrainer, Index);
        }
    }

    public GameObject SpawnBattlePokemonModel(PokemonTrainer Trainer, int PokemonIndex, out GameObject OutMegaModel)
    {
        GameObject NewModel = new GameObject(Trainer.TrainerName + "_" + Trainer.BagPokemons[PokemonIndex].GetName());
        if(Trainer.IsPlayer)
        {
            NewModel.transform.position = PlayerTransform.position;
            NewModel.transform.rotation = PlayerTransform.rotation;
        }
        else
        {
            NewModel.transform.position = EnemyTransform.position;
            NewModel.transform.rotation = EnemyTransform.rotation;
        }
        NewModel.transform.parent = ModelsRoot.transform;
        string prefabPath = "Models/" + Trainer.BagPokemons[PokemonIndex].GetName() + "/Prefabs/pkmModel";
        GameObject prefab = Resources.Load<GameObject>(prefabPath);
        GameObject instantiatedPrefab = Instantiate(prefab, new Vector3(0, 0, 0), prefab.transform.rotation);
        CreatedGameObject.Add(instantiatedPrefab);
        instantiatedPrefab.transform.parent = NewModel.transform;
        instantiatedPrefab.transform.localPosition = Vector3.zero;
        instantiatedPrefab.transform.localRotation = prefab.transform.rotation;
        NewModel.AddComponent<PokemonAnimationController>();
        NewModel.GetComponent<PokemonAnimationController>().PkmAnimator = instantiatedPrefab.GetComponent<Animator>();
        NewModel.GetComponent<PokemonAnimationController>().ModelRootInitializer = instantiatedPrefab.GetComponent<InitPokemonComponets>();
        NewModel.AddComponent<PokemonReceiver>();
        NewModel.GetComponent<PokemonReceiver>().BodyTransform = instantiatedPrefab.GetComponent<InitPokemonComponets>().CenterPosition;
        NewModel.GetComponent<PokemonReceiver>().TouchHitTransform = instantiatedPrefab.GetComponent<InitPokemonComponets>().TouchHitPosition;
        NewModel.AddComponent<PokemonScaleAnimation>();
        NewModel.AddComponent<PokemonMoveAnimation>();
        NewModel.AddComponent<PokemonRotationAnimation>();


        OutMegaModel = null;
        if(Trainer.BagPokemons[PokemonIndex].GetCanMega())
        {
            GameObject MegaModel = new GameObject(Trainer.TrainerName + "_" + Trainer.BagPokemons[PokemonIndex].GetName() + "-Mega");
            if(Trainer.IsPlayer)
            {
                MegaModel.transform.position = PlayerTransform.position;
                MegaModel.transform.rotation = PlayerTransform.rotation;
            }
            else
            {
                MegaModel.transform.position = EnemyTransform.position;
                MegaModel.transform.rotation = EnemyTransform.rotation;
            }
            MegaModel.transform.parent = ModelsRoot.transform;
            prefabPath = "Models/" + Trainer.BagPokemons[PokemonIndex].GetName() + "-Mega" + "/Prefabs/pkmModel";
            GameObject megaPrefab = Resources.Load<GameObject>(prefabPath);
            GameObject instantiatedMegaPrefab = Instantiate(megaPrefab, new Vector3(0, 0, 0), megaPrefab.transform.rotation);
            CreatedGameObject.Add(instantiatedMegaPrefab);
            instantiatedMegaPrefab.transform.parent = MegaModel.transform;
            instantiatedMegaPrefab.transform.localPosition = Vector3.zero;
            instantiatedMegaPrefab.transform.localRotation = megaPrefab.transform.rotation;
            MegaModel.AddComponent<PokemonAnimationController>();
            MegaModel.GetComponent<PokemonAnimationController>().PkmAnimator = instantiatedMegaPrefab.GetComponent<Animator>();
            MegaModel.GetComponent<PokemonAnimationController>().ModelRootInitializer = instantiatedMegaPrefab.GetComponent<InitPokemonComponets>();
            MegaModel.AddComponent<PokemonReceiver>();
            MegaModel.GetComponent<PokemonReceiver>().BodyTransform = instantiatedMegaPrefab.GetComponent<InitPokemonComponets>().CenterPosition;
            MegaModel.GetComponent<PokemonReceiver>().TouchHitTransform = instantiatedMegaPrefab.GetComponent<InitPokemonComponets>().TouchHitPosition;
            MegaModel.AddComponent<PokemonScaleAnimation>();
            MegaModel.AddComponent<PokemonMoveAnimation>();
            MegaModel.AddComponent<PokemonRotationAnimation>();
            MegaModel.SetActive(false);

            OutMegaModel = MegaModel;
        }
        return NewModel;
    }

    public void SpawnSkillAnimations(PokemonTrainer Trainer, int PokemonIndex)
    {
        BagPokemon Pkm = Trainer.BagPokemons[PokemonIndex];
        for(int Index = 0; Index < 4; Index++)
        {
            GameObject prefab = Pkm.GetBaseSkill(Index).gameObject;
            GameObject instantiatedPrefab = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
            CreatedGameObject.Add(instantiatedPrefab);
            instantiatedPrefab.transform.parent = SkillsRoot.transform;
            BaseSkill NewSkill = instantiatedPrefab.GetComponent<BaseSkill>();
            Pkm.SetBaseSkill(Index, instantiatedPrefab.GetComponent<BaseSkill>());
            GameObject instantiatedSkill = Instantiate(NewSkill.SkillAnimation.gameObject, new Vector3(0, 0, 0), NewSkill.SkillAnimation.transform.rotation);
            CreatedGameObject.Add(instantiatedSkill);
            NewSkill.SkillAnimation = instantiatedSkill.GetComponent<PlayableDirector>();
            NewSkill.transform.parent = SkillsRoot.transform;
            TimelineAsset timeline = (TimelineAsset)NewSkill.SkillAnimation.playableAsset;
            foreach (var track in timeline.GetOutputTracks())
            {
                if (track is TrackWithParameterizedSignal Track)
                {
                    if(Track.name == "BattleUI")
                        NewSkill.SkillAnimation.SetGenericBinding(Track, SkillAnimationUIReceiver);
                    if(Track.name == "CameraShake")
                        NewSkill.SkillAnimation.SetGenericBinding(Track, SkillAnimationCameraShakeReceiver);
                    if(Track.name == "PostProcess")
                        NewSkill.SkillAnimation.SetGenericBinding(Track, PostProcessVolumeReceiver);
                }
            }
        }
    }

    public void SpawnAbility(PokemonTrainer Trainer, int PokemonIndex)
    {
        BagPokemon Pkm = Trainer.BagPokemons[PokemonIndex];
        BaseAbility Ability = Pkm.GetAbility(false);
        if(Ability)
        {
            GameObject prefab = Ability.gameObject;
            GameObject instantiatedPrefab = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
            CreatedGameObject.Add(instantiatedPrefab);
            instantiatedPrefab.transform.parent = AbilitiesSkillRoot.transform;
            Pkm.SetAbility(instantiatedPrefab.GetComponent<BaseAbility>(), false);   
        }

        BaseAbility MegaAbility = Pkm.GetAbility(true);
        if(MegaAbility)
        {
            GameObject prefab = MegaAbility.gameObject;
            GameObject instantiatedPrefab = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
            CreatedGameObject.Add(instantiatedPrefab);
            instantiatedPrefab.transform.parent = AbilitiesSkillRoot.transform;
            Pkm.SetAbility(instantiatedPrefab.GetComponent<BaseAbility>(), true);   
        }
    }
    public void SpawnItem(PokemonTrainer Trainer, int PokemonIndex)
    {
        BagPokemon Pkm = Trainer.BagPokemons[PokemonIndex];
        BaseItem Item = Pkm.GetItem();
        if(CurrentEnemyTrainer.TrainerName == "大吾")
        {
            if(Trainer == CurrentPlayerTrainer)
            {
                if(Item.ItemName.Contains("进化石") == false)
                {
                    Item = GemItemList[(int)Pkm.GetType0(false)];
                }
            }
        }
        if(Item)
        {
            GameObject prefab = Item.gameObject;
            GameObject instantiatedPrefab = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
            CreatedGameObject.Add(instantiatedPrefab);
            instantiatedPrefab.transform.parent = ItemsRoot.transform;
            Pkm.SetItem(instantiatedPrefab.GetComponent<BaseItem>());   
        }
    }
    public void ClearAllObjects()
    {
        foreach(var Obj in CreatedGameObject)
        {
            GameObject.Destroy(Obj);
        }
        CreatedGameObject.Clear();
        for(int Index = 0; Index < 6; Index++)
        {
            GameObject.Destroy(CurrentPlayerTrainer.BagPokemons[Index].gameObject);
            GameObject.Destroy(CurrentEnemyTrainer.BagPokemons[Index].gameObject);
            CurrentPlayerTrainer.BagPokemons[Index] = PlayerOriginalBagPokemons[Index].GetComponent<BagPokemon>();
            CurrentEnemyTrainer.BagPokemons[Index] = EnemyOriginalBagPokemons[Index].GetComponent<BagPokemon>();
        }
    }
    public void SpawnBattlePokemon(PokemonTrainer Trainer, int PokemonIndex)
    {
        if(Trainer.BagPokemons[PokemonIndex])
        {
            SpawnSkillAnimations(Trainer, PokemonIndex);
            SpawnAbility(Trainer, PokemonIndex);
            GameObject MegaModel;
            GameObject ModelObject = SpawnBattlePokemonModel(Trainer, PokemonIndex, out MegaModel);
            GameObject newPokemon = new GameObject(Trainer.TrainerName + "_" + Trainer.BagPokemons[PokemonIndex].GetName());
            CreatedGameObject.Add(newPokemon);
            newPokemon.transform.parent = BattlePokemonRoot.transform;
            BattlePokemon battlePokemon = newPokemon.AddComponent<BattlePokemon>();
            int LevelChangedValue = 0;
            if(Trainer.TrainerSkill.GetSkillName() == "石头收藏家")
            {
                foreach(var bagPokemon in CurrentPlayerTrainer.BagPokemons)
                {
                    if(bagPokemon.GetItem() && bagPokemon.GetItem().ItemName.Contains("石"))
                    {
                        LevelChangedValue++;
                    }
                }
                foreach(var bagPokemon in CurrentEnemyTrainer.BagPokemons)
                {
                    if(bagPokemon.GetItem() && bagPokemon.GetItem().ItemName.Contains("石"))
                    {
                        LevelChangedValue++;
                    }
                }
            }
            if(BattleManager.StaticManager.HasSpecialRule("特殊规则(卡露妮)") && Trainer == CurrentEnemyTrainer)
            {
                bool[] HasTypeArray = new bool[19]{false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, false};
                foreach(var bagPokemon in CurrentPlayerTrainer.BagPokemons)
                {
                    HasTypeArray[(int)bagPokemon.GetType0(false) + 1] = true;
                    HasTypeArray[(int)bagPokemon.GetType1(false) + 1] = true;
                }
                foreach(var bagPokemon in CurrentEnemyTrainer.BagPokemons)
                {
                    HasTypeArray[(int)bagPokemon.GetType0(false) + 1] = true;
                    HasTypeArray[(int)bagPokemon.GetType1(false) + 1] = true;
                }
                for(int Index = 1; Index <= 18; Index++)
                {
                    if(HasTypeArray[Index] == true)
                    {
                        LevelChangedValue++;
                    }
                }
            }
            battlePokemon.SetBattlePokemonData(Trainer.BagPokemons[PokemonIndex], Trainer, ModelObject, MegaModel, LevelChangedValue);
            Trainer.BattlePokemons[PokemonIndex] = battlePokemon;
            ModelObject.SetActive(false);
        }
    }
}
