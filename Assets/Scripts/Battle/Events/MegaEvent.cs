using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
public class MegaEvent : EventAnimationPlayer, Event
{
    private BattlePokemon ReferencePokemon;
    private BattleManager ReferenceManager;
    public MegaEvent(BattleManager InManager, BattlePokemon InReferencePokemon)
    {
        ReferencePokemon = InReferencePokemon;
        ReferenceManager = InManager;
    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        if(InBattleManager.GetBattleEnd() == true) return false;
        return true;
    }
    public override void OnAnimationFinished()
    {
        ReferencePokemon.GetMegaPokemonModel().GetComponent<PokemonAnimationController>().AfterMega();
    }
    public override void InitAnimation()
    {
        TimelineAnimationManager Timelines = TimelineAnimationManager.GetGlobalTimelineAnimationManager();

        PlayableDirector MessageDirector = Timelines.MessageAnimation;
        TimelineAnimation MessageTimeline = new TimelineAnimation(MessageDirector);
        MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", ReferencePokemon.GetName() + "携带的超级石与钥石产生了共鸣！");
        AddAnimation(MessageTimeline);  

        TimelineAnimation MegaTimeline = new TimelineAnimation(Timelines.MegaAnimation);
        MegaTimeline.SetTrackObject("OriginPokemonModel", ReferencePokemon.GetOriginPokemonModel());
        MegaTimeline.SetTrackObject("MegaPokemonModel", ReferencePokemon.GetMegaPokemonModel());

        GameObject Cameras = GameObject.Find("Cameras");
        GameObject EnemyCamera = Cameras.GetComponent<SubObjects>().SubObject1;
        GameObject PlayerCamera = Cameras.GetComponent<SubObjects>().SubObject2;
        if(ReferencePokemon.GetIsEnemy())
        {
            MegaTimeline.SetTrackObject("TargetCamera", EnemyCamera);
        }
        else
        {
            MegaTimeline.SetTrackObject("TargetCamera", PlayerCamera);
        }
        MegaTimeline.SetSignalReceiver("TargetPokemon", ReferencePokemon.GetMegaPokemonModel());
        MegaTimeline.SetSignalParameter("PokemonCry", "AudioSignal", "Pokemon", ReferencePokemon.GetEnName() + "-mega");
        Vector3 position = ReferencePokemon.GetPokemonModel().transform.position;
        position.y = 0.3f;
        Timelines.MegaAnimation.gameObject.transform.position = position;
        AddAnimation(MegaTimeline);
    }

    public void Process(BattleManager InManager)
    {
        if(!ShouldProcess(InManager)) return;
        InManager.AddAnimationEvent(this);

        InManager.TranslateTimePoint(ETimePoint.BeforeMegaEvolution, this);
        ReferencePokemon.MegaEvolution();
        InManager.TranslateTimePoint(ETimePoint.AfterMegaEvolution, this);
    }

    public EventType GetEventType()
    {
        return EventType.MegaEvolution;
    }
}
