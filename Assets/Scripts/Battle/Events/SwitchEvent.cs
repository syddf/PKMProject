using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class SwitchEvent : EventAnimationPlayer, Event
{
    private BattlePokemon OutPokemon;
    private BattlePokemon InPokemon;
    private BattleManager ReferenceManager;
    private BattlePokemonStat CloneInPokemon;

    private int HP;
    public SwitchEvent(BattleManager InManager, BattlePokemon InOutPokemon, BattlePokemon InInPokemon)
    {
        OutPokemon = InOutPokemon;
        InPokemon = InInPokemon;
        ReferenceManager = InManager;
    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        if(InBattleManager.GetBattleEnd() == true) return false;
        return true;
    }

    public override void InitAnimation()
    {
        TimelineAnimationManager Timelines = TimelineAnimationManager.GetGlobalTimelineAnimationManager();        
        TimelineAnimation TargetTimeline = new TimelineAnimation(Timelines.SwitchAnimation);
        SubObjects SubObj = Timelines.SwitchWhenDefeatedAnimation.gameObject.GetComponent<SubObjects>();
        InPokemon.GetPokemonModel().transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        if(InPokemon.GetIsEnemy())
        {
            TargetTimeline.SetTrackObject("Pokemon1", null);
            TargetTimeline.SetTrackObject("MonsterBall1", null);
            TargetTimeline.SetTrackObject("Explosion1", null); 
            TargetTimeline.SetSignalParameter("PlayerPokemonCry", "AudioSignal", "Pokemon", InPokemon.GetEnName());         
            TargetTimeline.SetSignalParameter("EnemyPokemonCry", "AudioSignal", "Pokemon", "");                    
        }
        else
        {            
            TargetTimeline.SetTrackObject("Pokemon1", InPokemon.GetPokemonModel());
            TargetTimeline.SetTrackObject("MonsterBall1", SubObj.SubObject2);
            TargetTimeline.SetTrackObject("Explosion1", SubObj.SubObject1);    
            InPokemon.GetPokemonModel().gameObject.transform.position = SubObj.SubObject5.transform.position;        
        }

        if(!InPokemon.GetIsEnemy())
        {
            TargetTimeline.SetTrackObject("Pokemon2", null);
            TargetTimeline.SetTrackObject("MonsterBall2", null);
            TargetTimeline.SetTrackObject("Explosion2", null);    
            TargetTimeline.SetSignalParameter("EnemyPokemonCry", "AudioSignal", "Pokemon", InPokemon.GetEnName()); 
            TargetTimeline.SetSignalParameter("PlayerPokemonCry", "AudioSignal", "Pokemon", "");                    
        }
        else
        {
            TargetTimeline.SetTrackObject("Pokemon2", InPokemon.GetPokemonModel());
            TargetTimeline.SetTrackObject("MonsterBall2", SubObj.SubObject4);
            TargetTimeline.SetTrackObject("Explosion2", SubObj.SubObject3);            
            InPokemon.GetPokemonModel().gameObject.transform.position = SubObj.SubObject6.transform.position;        
        }

        TargetTimeline.SetTrackObject("PokemonActivation", OutPokemon.GetPokemonModel());
        TargetTimeline.SetSignalReceiver("PokemonAnim", OutPokemon.GetPokemonModel());
        if(OutPokemon.GetIsEnemy())
        {
            TargetTimeline.SetTrackObject("Laser", Timelines.SwitchAnimation.gameObject.GetComponent<SubObjects>().SubObject3);
        }
        else
        {
            TargetTimeline.SetTrackObject("Laser", Timelines.SwitchAnimation.gameObject.GetComponent<SubObjects>().SubObject2);
        }
        Timelines.SwitchAnimation.gameObject.GetComponent<SubObjects>().SubObject1.
        GetComponent<PositionWithObject>().target = OutPokemon.GetPokemonModel().GetComponent<PokemonReceiver>().BodyTransform;        
        AddAnimation(TargetTimeline);

        if(InPokemon.GetAbility().GetAbilityName() == "破格")
        {
            PlayableDirector MessageDirector = Timelines.MessageAnimation;
            string Message = InPokemon.GetName() + "打破了常规！";
            TimelineAnimation MessageTimeline = new TimelineAnimation(MessageDirector);
            MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", Message);
            AddAnimation(MessageTimeline);
        }
    }
    public override void OnAnimationFinished()
    {
        if(InPokemon.GetIsEnemy())
        {
            ReferenceManager.UpdateEnemyUI();
            ReferenceManager.UpdatePokemonInfo(InPokemon, CloneInPokemon);
            ReferenceManager.UpdatePokemonStatusChange(InPokemon, CloneInPokemon);
        }
        else
        {
            ReferenceManager.UpdatePlayerUI();
            ReferenceManager.UpdatePokemonInfo(InPokemon, CloneInPokemon);
            ReferenceManager.UpdatePokemonStatusChange(InPokemon, CloneInPokemon);
        }
    }

    public void Process(BattleManager InManager)
    {
        if(!ShouldProcess(InManager)) return;
        InManager.AddAnimationEvent(this);
        if(!InPokemon.GetIsEnemy())
        {
            InManager.SetNewPlayerPokemon(InPokemon);
        }
        else
        {
            InManager.SetNewEnemyPokemon(InPokemon);
        }
        OutPokemon.ClearStatusChange();
        CloneInPokemon = InPokemon.CloneBattlePokemonStats();
        InManager.TranslateTimePoint(ETimePoint.PokemonIn, this);

        if(InPokemon.GetIsEnemy())
        {            
            List<string> Lines = InManager.GetEnemyTrainer().GetLineWhenPokemonFirstIn(InPokemon);
            if(Lines.Count > 0 && InPokemon.GetFirstIn())
            {
                ChatAnimationFakeEvent.AddChatEvent(Lines, false).Process(InManager);
            }
        }

        InPokemon.SwitchIn();
    }

    public EventType GetEventType()
    {
        return EventType.Switch;
    }

    public BattlePokemon GetOutPokemon()
    {
        return OutPokemon;
    }

    public BattlePokemon GetInPokemon()
    {
        return InPokemon;
    }
}

public class SwitchAfterSkillUseEvent : EventAnimationPlayer, Event
{
    private BattlePokemon ReferencePokemon;
    private BattleManager ReferenceManager;

    public void Process(BattleManager InManager)
    {
        if(!ShouldProcess(InManager)) return;
        InManager.AddAnimationEvent(this);
        InManager.SetWaitForSwitchAfterSkillUse(!ReferencePokemon.GetIsEnemy());
    }
    public SwitchAfterSkillUseEvent(BattleManager InManager, BattlePokemon InPokemon)
    {
        ReferenceManager = InManager;
        ReferencePokemon = InPokemon;
    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        if(InBattleManager.GetBattleEnd() == true) return false;
        if(ReferencePokemon.IsDead() == true) return false;
        return true;
    }

    public EventType GetEventType()
    {
        return EventType.SwitchAfterSkillUse;
    }

    public BattlePokemon GetReferencePokemon()
    {
        return ReferencePokemon;
    }
}

public class SwitchWhenDefeatedEvent : EventAnimationPlayer, Event
{
    private BattlePokemon EnemyDefeatedPokemon;
    private BattlePokemon EnemyNewPokemon;
    private BattlePokemon PlayerDefeatedPokemon;
    private BattlePokemon PlayerNewPokemon;
    private BattleManager ReferenceManager;
    private BattlePokemonStat CloneEnemyNewPokemonStats;
    private BattlePokemonStat ClonePlayerNewPokemonStats;
    public SwitchWhenDefeatedEvent(BattleManager InManager, BattlePokemon InEnemyDefeatedPokemon, BattlePokemon InEnemyNewPokemon, BattlePokemon InPlayerDefeatedPokemon, BattlePokemon InPlayerNewPokemon)
    {
        EnemyDefeatedPokemon = InEnemyDefeatedPokemon;
        EnemyNewPokemon = InEnemyNewPokemon;
        PlayerDefeatedPokemon = InPlayerDefeatedPokemon;
        PlayerNewPokemon = InPlayerNewPokemon;
        ReferenceManager = InManager;
        if(EnemyNewPokemon)
            CloneEnemyNewPokemonStats = EnemyNewPokemon.CloneBattlePokemonStats();
        if(PlayerNewPokemon)
            ClonePlayerNewPokemonStats = PlayerNewPokemon.CloneBattlePokemonStats();
    }

    public override void OnAnimationFinished()
    {
        //ReferenceManager.UpdateUI(false);
        if(EnemyNewPokemon)
        {
            ReferenceManager.UpdatePokemonInfo(EnemyNewPokemon, CloneEnemyNewPokemonStats);
            ReferenceManager.UpdateEnemyUI();
            ReferenceManager.UpdatePokemonStatusChange(EnemyNewPokemon, CloneEnemyNewPokemonStats);
        }
        if(PlayerNewPokemon)
        {
            ReferenceManager.UpdatePokemonInfo(PlayerNewPokemon, ClonePlayerNewPokemonStats);
            ReferenceManager.UpdatePlayerUI();
            ReferenceManager.UpdatePokemonStatusChange(PlayerNewPokemon, ClonePlayerNewPokemonStats);
        }
    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        if(InBattleManager.GetBattleEnd() == true) return false;
        return true;
    }

    public void Process(BattleManager InManager)
    {
        if(!ShouldProcess(InManager)) return;
        InManager.AddAnimationEvent(this);
        if(PlayerNewPokemon != null)
        {
            InManager.SetNewPlayerPokemon(PlayerNewPokemon);
            PlayerNewPokemon.SwitchIn();
            PlayerDefeatedPokemon.ClearStatusChange();
        }
        if(EnemyNewPokemon != null)
        {
            InManager.SetNewEnemyPokemon(EnemyNewPokemon);
            EnemyDefeatedPokemon.ClearStatusChange();
        }
        InManager.TranslateTimePoint(ETimePoint.PokemonIn, this);
        if(EnemyNewPokemon != null)
        {
            List<string> Lines = InManager.GetEnemyTrainer().GetLineWhenPokemonFirstIn(EnemyNewPokemon);
            if(Lines.Count > 0 && EnemyNewPokemon.GetFirstIn())
            {
                ChatAnimationFakeEvent.AddChatEvent(Lines, false).Process(InManager);
            }
            EnemyNewPokemon.SwitchIn();
        }

        if(PlayerNewPokemon != null)
        {
            PlayerNewPokemon.SwitchIn();
        }
    }


    public override void InitAnimation()
    {
        TimelineAnimationManager Timelines = TimelineAnimationManager.GetGlobalTimelineAnimationManager();
        TimelineAnimation TargetTimeline = new TimelineAnimation(Timelines.SwitchWhenDefeatedAnimation);
        SubObjects SubObj = Timelines.SwitchWhenDefeatedAnimation.gameObject.GetComponent<SubObjects>();
        TargetTimeline.SetSignalParameter("PlayerPokemonCry", "AudioSignal", "Pokemon", "");
        TargetTimeline.SetSignalParameter("EnemyPokemonCry", "AudioSignal", "Pokemon", "");
        if(!PlayerNewPokemon)
        {
            TargetTimeline.SetTrackObject("Pokemon1", null);
            TargetTimeline.SetTrackObject("MonsterBall1", null);
            TargetTimeline.SetTrackObject("Explosion1", null);            
        }
        else
        {
            
            TargetTimeline.SetTrackObject("Pokemon1", PlayerNewPokemon.GetPokemonModel());
            PlayerNewPokemon.GetPokemonModel().transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            TargetTimeline.SetTrackObject("MonsterBall1", SubObj.SubObject2);
            TargetTimeline.SetTrackObject("Explosion1", SubObj.SubObject1);    
            PlayerNewPokemon.GetPokemonModel().gameObject.transform.position = SubObj.SubObject5.transform.position;        
            TargetTimeline.SetSignalParameter("PlayerPokemonCry", "AudioSignal", "Pokemon", PlayerNewPokemon.GetEnName());
        }

        if(!EnemyNewPokemon)
        {
            TargetTimeline.SetTrackObject("Pokemon2", null);
            TargetTimeline.SetTrackObject("MonsterBall2", null);
            TargetTimeline.SetTrackObject("Explosion2", null);            
        }
        else
        {
            EnemyNewPokemon.GetPokemonModel().transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            TargetTimeline.SetTrackObject("Pokemon2", EnemyNewPokemon.GetPokemonModel());
            TargetTimeline.SetTrackObject("MonsterBall2", SubObj.SubObject4);
            TargetTimeline.SetTrackObject("Explosion2", SubObj.SubObject3);            
            EnemyNewPokemon.GetPokemonModel().gameObject.transform.position = SubObj.SubObject6.transform.position;        
            TargetTimeline.SetSignalParameter("EnemyPokemonCry", "AudioSignal", "Pokemon", EnemyNewPokemon.GetEnName());
        }
        AddAnimation(TargetTimeline);

        if(PlayerNewPokemon)
        {
            if(PlayerNewPokemon.GetAbility().GetAbilityName() == "破格")
            {
                PlayableDirector MessageDirector = Timelines.MessageAnimation;
                string Message = PlayerNewPokemon.GetName() + "打破了常规！";
                TimelineAnimation MessageTimeline = new TimelineAnimation(MessageDirector);
                MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", Message);
                AddAnimation(MessageTimeline);
            }
        }
        if(EnemyNewPokemon)
        {
            if(EnemyNewPokemon.GetAbility().GetAbilityName() == "破格")
            {
                PlayableDirector MessageDirector = Timelines.MessageAnimation;
                string Message = EnemyNewPokemon.GetName() + "打破了常规！";
                TimelineAnimation MessageTimeline = new TimelineAnimation(MessageDirector);
                MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", Message);
                AddAnimation(MessageTimeline);
            }
        }
    }

    public EventType GetEventType()
    {
        return EventType.SwitchAfterDefeated;
    }

    public BattlePokemon GetPlayerNewPokemon()
    {
        return PlayerNewPokemon;
    }

    public BattlePokemon GetEnemyNewPokemon()
    {
        return EnemyNewPokemon;
    }
}

public class SingleBattleGameStartEvent : EventAnimationPlayer, Event
{
    private BattlePokemon PlayerPokemon;
    private BattlePokemon EnemyPokemon;
    public SingleBattleGameStartEvent(BattlePokemon InPlayerPokemon, BattlePokemon InEnemyPokemon)
    {
        PlayerPokemon = InPlayerPokemon;
        EnemyPokemon = InEnemyPokemon;
    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {

        if(InBattleManager.GetBattleEnd() == true) return false;
        return true;
    }

    public void Process(BattleManager InManager)
    {
        if(!ShouldProcess(InManager)) return;
        InManager.AddAnimationEvent(this);
        InManager.TranslateTimePoint(ETimePoint.BattleStart, this);
        
        //Battle Start Animation.
        GameObject battleStartObj = GameObject.Find("Canvas").GetComponent<SubObjects>().SubObject1;
        ActiveHalfSecondAnimationEvent AnimEvent = new ActiveHalfSecondAnimationEvent(battleStartObj);
        AnimEvent.Process(InManager);

        List<string> Lines = InManager.GetEnemyTrainer().GetLineWhenBattleStart();
        if(Lines.Count > 0)
        {
            ChatAnimationFakeEvent.AddChatEvent(Lines, false).Process(InManager);
        }

        Lines = InManager.GetEnemyTrainer().GetLineWhenPokemonFirstIn(EnemyPokemon);
        if(Lines.Count > 0 && EnemyPokemon.GetFirstIn())
        {
            ChatAnimationFakeEvent.AddChatEvent(Lines, false).Process(InManager);
        }

        PlayerPokemon.SwitchIn();
        EnemyPokemon.SwitchIn();
    }

    public override void InitAnimation()
    {
        TimelineAnimationManager Timelines = TimelineAnimationManager.GetGlobalTimelineAnimationManager();
        TimelineAnimation InBattleTimeline = new TimelineAnimation(Timelines.InBattleAnimation);
        AddAnimation(InBattleTimeline);

        TimelineAnimation TargetTimeline = new TimelineAnimation(Timelines.BattleStartAnimation);
        TargetTimeline.SetTrackObject("Pokemon1", PlayerPokemon.GetPokemonModel());
        TargetTimeline.SetTrackObject("Pokemon2", EnemyPokemon.GetPokemonModel());

        TargetTimeline.SetSignalParameter("PlayerPokemonCry", "AudioSignal", "Pokemon", PlayerPokemon.GetEnName());
        TargetTimeline.SetSignalParameter("EnemyPokemonCry", "AudioSignal", "Pokemon", EnemyPokemon.GetEnName());
        AddAnimation(TargetTimeline);

        if(PlayerPokemon)
        {
            if(PlayerPokemon.GetAbility().GetAbilityName() == "破格")
            {
                PlayableDirector MessageDirector = Timelines.MessageAnimation;
                string Message = PlayerPokemon.GetName() + "打破了常规！";
                TimelineAnimation MessageTimeline = new TimelineAnimation(MessageDirector);
                MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", Message);
                AddAnimation(MessageTimeline);
            }
        }
        if(EnemyPokemon)
        {
            if(EnemyPokemon.GetAbility().GetAbilityName() == "破格")
            {
                PlayableDirector MessageDirector = Timelines.MessageAnimation;
                string Message = EnemyPokemon.GetName() + "打破了常规！";
                TimelineAnimation MessageTimeline = new TimelineAnimation(MessageDirector);
                MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", Message);
                AddAnimation(MessageTimeline);
            }
        }
    }

    public EventType GetEventType()
    {
        return EventType.BattleStart;
    }

    public BattlePokemon GetPlayerPokemon()
    {
        return PlayerPokemon;
    }

    public BattlePokemon GetEnemyPokemon()
    {
        return EnemyPokemon;
    }
}