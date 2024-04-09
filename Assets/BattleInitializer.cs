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
    public void InitBattleResources(PokemonTrainer PlayerTrainer, PokemonTrainer EnemyTrainer)
    {
        PlayerTrainer.IsPlayer = true;
        EnemyTrainer.IsPlayer = false;
        if(PlayerTrainer.TrainerSkill)
        {
            GameObject PlayerTrainerSkillObj = Instantiate(PlayerTrainer.TrainerSkill.gameObject, new Vector3(0, 0, 0), Quaternion.identity);
            PlayerTrainer.TrainerSkill = PlayerTrainerSkillObj.GetComponent<BaseTrainerSkill>();
            PlayerTrainerSkillObj.transform.parent = TrainerSkillsRoot.transform;
        }
        if(EnemyTrainer.TrainerSkill)
        {
            GameObject EnemyTrainerSkillObj = Instantiate(EnemyTrainer.TrainerSkill.gameObject, new Vector3(0, 0, 0), Quaternion.identity);
            EnemyTrainer.TrainerSkill = EnemyTrainerSkillObj.GetComponent<BaseTrainerSkill>();
            EnemyTrainerSkillObj.transform.parent = TrainerSkillsRoot.transform;
        }
        PlayerTrainer.TrainerSkill.SetTrainer(PlayerTrainer);
        EnemyTrainer.TrainerSkill.SetTrainer(EnemyTrainer);

        for(int Index = 0; Index < 6; Index++)
        {
            SpawnBattlePokemon(PlayerTrainer, Index);
            SpawnBattlePokemon(EnemyTrainer, Index);
        }
    }

    public GameObject SpawnBattlePokemonModel(PokemonTrainer Trainer, int PokemonIndex)
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
        instantiatedPrefab.transform.parent = NewModel.transform;
        instantiatedPrefab.transform.localPosition = Vector3.zero;
        instantiatedPrefab.transform.localRotation = prefab.transform.rotation;
        NewModel.AddComponent<PokemonAnimationController>();
        NewModel.GetComponent<PokemonAnimationController>().PkmAnimator = instantiatedPrefab.GetComponent<Animator>();
        NewModel.AddComponent<PokemonReceiver>();
        NewModel.GetComponent<PokemonReceiver>().BodyTransform = instantiatedPrefab.GetComponent<InitPokemonComponets>().CenterPosition;
        NewModel.GetComponent<PokemonReceiver>().TouchHitTransform = instantiatedPrefab.GetComponent<InitPokemonComponets>().TouchHitPosition;
        NewModel.AddComponent<PokemonScaleAnimation>();
        NewModel.AddComponent<PokemonMoveAnimation>();
        NewModel.AddComponent<PokemonRotationAnimation>();
        return NewModel;
    }

    public void SpawnSkillAnimations(PokemonTrainer Trainer, int PokemonIndex)
    {
        BagPokemon Pkm = Trainer.BagPokemons[PokemonIndex];
        for(int Index = 0; Index < 4; Index++)
        {
            GameObject prefab = Pkm.GetBaseSkill(Index).gameObject;
            GameObject instantiatedPrefab = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
            instantiatedPrefab.transform.parent = SkillsRoot.transform;
            BaseSkill NewSkill = instantiatedPrefab.GetComponent<BaseSkill>();
            Pkm.SetBaseSkill(Index, instantiatedPrefab.GetComponent<BaseSkill>());
            GameObject instantiatedSkill = Instantiate(NewSkill.SkillAnimation.gameObject, new Vector3(0, 0, 0), NewSkill.SkillAnimation.transform.rotation);
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
        BaseAbility Ability = Pkm.GetAbility();
        if(Ability)
        {
            GameObject prefab = Ability.gameObject;
            GameObject instantiatedPrefab = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
            instantiatedPrefab.transform.parent = AbilitiesSkillRoot.transform;
            Pkm.SetAbility(instantiatedPrefab.GetComponent<BaseAbility>());   
        }
    }
    public void SpawnItem(PokemonTrainer Trainer, int PokemonIndex)
    {
        BagPokemon Pkm = Trainer.BagPokemons[PokemonIndex];
        BaseItem Item = Pkm.GetItem();
        if(Item)
        {
            GameObject prefab = Item.gameObject;
            GameObject instantiatedPrefab = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
            instantiatedPrefab.transform.parent = ItemsRoot.transform;
            Pkm.SetItem(instantiatedPrefab.GetComponent<BaseItem>());   
        }
    }
    public void SpawnBattlePokemon(PokemonTrainer Trainer, int PokemonIndex)
    {
        if(Trainer.BagPokemons[PokemonIndex])
        {
            SpawnSkillAnimations(Trainer, PokemonIndex);
            SpawnAbility(Trainer, PokemonIndex);
            SpawnItem(Trainer, PokemonIndex);
            GameObject ModelObject = SpawnBattlePokemonModel(Trainer, PokemonIndex);
            GameObject newPokemon = new GameObject(Trainer.TrainerName + "_" + Trainer.BagPokemons[PokemonIndex].GetName());
            newPokemon.transform.parent = BattlePokemonRoot.transform;
            BattlePokemon battlePokemon = newPokemon.AddComponent<BattlePokemon>();
            battlePokemon.SetBattlePokemonData(Trainer.BagPokemons[PokemonIndex], Trainer, ModelObject);
            Trainer.BattlePokemons[PokemonIndex] = battlePokemon;
            ModelObject.SetActive(false);
        }
    }
}
