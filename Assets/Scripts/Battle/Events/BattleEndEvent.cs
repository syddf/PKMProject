using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BattleEndEvent : EventAnimationPlayer, Event
{
    private bool Win;
    private string SpecialReason = "";
    public BattleEndEvent(bool InWin)
    {
        Win = InWin;
    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        if(InBattleManager.GetBattleEnd() == true) return false;
        return true;
    }

    public override void InitAnimation()
    {
        TimelineAnimationManager Timelines = TimelineAnimationManager.GetGlobalTimelineAnimationManager();
        PlayableDirector MessageDirector = Timelines.MessageDelayAnimation;
        TimelineAnimation MessageTimeline = new TimelineAnimation(MessageDirector);
        if(Win)
        {
            MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", "你战胜了对手！");
        }
        else
        {
            if(SpecialReason != "")
            {
                MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", SpecialReason);
            }
            else
            {
                MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", "你被击败了！");
            }
        }
        AddAnimation(MessageTimeline);
    }

    public void Process(BattleManager InManager)
    {        
        if(!ShouldProcess(InManager)) return;
        InManager.AddAnimationEvent(this);
        if(Win)
        {
            if(InManager.HasSpecialRule("特殊规则(翔太)"))
            {
                int Counter = 0;
                PokemonTrainer Player = InManager.GetPlayerTrainer();
                foreach(var BattlePkm in Player.BattlePokemons)
                {
                    if(BattlePkm.IsDead() == true)
                    {
                        Counter++;
                    }
                }

                if(Counter > 1)
                {
                    SpecialReason = "玩家陷入濒死的宝可梦超过1只！对战失败！";
                    Win = false;
                }
            }

            if(InManager.HasSpecialRule("特殊规则(福爷)"))
            {
                if(InManager.GetHealedValue(true) < 300)
                {
                    SpecialReason = "玩家宝可梦的总回复量未达到要求！对战失败！";
                    Win = false;
                }
                else if(InManager.GetHealedValue(false) > 300)
                {
                    SpecialReason = "对手宝可梦的总回复量超过了要求！对战失败！";
                    Win = false;
                }
            }

            if(InManager.HasSpecialRule("特殊规则(天桐)"))
            {
                string PkmName = "";
                PokemonTrainer Player = InManager.GetPlayerTrainer();
                foreach(var BattlePkm in Player.BattlePokemons)
                {
                    if(BattlePkm.GetBeatPokemonCount() == 0)
                    {
                        Win = false;
                        PkmName += "(";
                        PkmName += BattlePkm.GetName();
                        PkmName += ")";
                    }
                }

                if(Win == false)
                {
                    SpecialReason = PkmName + "未击败至少一只宝可梦！对战失败！";
                }
            }

            if(InManager.HasSpecialRule("特殊规则(奇巴纳)"))
            {
                int Counter = InManager.GetWeatherChangCounter();
                if(Counter < 8)
                {
                    Win = false;
                    SpecialReason = "天气切换次数未达到8次！对战失败！";
                }
            }
        }
        InManager.SetBattleEnd(true, Win);
    }

    public EventType GetEventType()
    {
        return EventType.BattleEnd;
    }
}
