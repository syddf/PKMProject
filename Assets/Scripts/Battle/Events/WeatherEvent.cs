using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class WeatherChangeEvent : EventAnimationPlayer, Event
{
    private EWeather OriginWeatherType;
    private EWeather NewWeatherType;
    private BattlePokemon SourcePokemon;
    private BattleManager ReferenceBattleManager;
    private bool bSuccessed = false;

    public string GetWeatherTypeName()
    {
        if(NewWeatherType == EWeather.SunLight) return "大晴天";
        if(NewWeatherType == EWeather.Rain) return "下雨";
        if(NewWeatherType == EWeather.Snow) return "下雪";
        if(NewWeatherType == EWeather.Sand) return "沙暴";
        return "";
    }

    public GameObject GetWeatherParticlesObject(EWeather WeatherType)
    {
        GameObject WeatherRoot = GameObject.Find("Weather");
        if(WeatherType == EWeather.SunLight)
        {
            WeatherRoot.GetComponent<SubObjects>().SubObject1.transform.position = new Vector3(1.9f, -7f, 0.0f);
            return WeatherRoot.GetComponent<SubObjects>().SubObject1;
        }
        if(WeatherType == EWeather.Rain)
        {
            return WeatherRoot.GetComponent<SubObjects>().SubObject2;
        }
        if(WeatherType == EWeather.Snow)
        {
            return WeatherRoot.GetComponent<SubObjects>().SubObject3;
        }
        if(WeatherType == EWeather.Sand)
        {
            return WeatherRoot.GetComponent<SubObjects>().SubObject4;
        }
        return null;
    }

    public WeatherChangeEvent(BattlePokemon InSourcePokemon, BattleManager InBattleManager, EWeather InNewWeatherType)
    {
        ReferenceBattleManager = InBattleManager;
        SourcePokemon = InSourcePokemon;   
        NewWeatherType = InNewWeatherType;
    }

    public bool ShouldProcess(BattleManager InBattleManager)
    {
        if(InBattleManager.GetBattleEnd() == true) return false;
        return true;
    }

    public override void InitAnimation()
    {
        TimelineAnimationManager Timelines = TimelineAnimationManager.GetGlobalTimelineAnimationManager();
        if(bSuccessed)
        {
            GetWeatherParticlesObject(EWeather.SunLight).SetActive(false);
            GetWeatherParticlesObject(EWeather.Rain).SetActive(false);
            GetWeatherParticlesObject(EWeather.Snow).SetActive(false);
            GetWeatherParticlesObject(EWeather.Sand).SetActive(false);
            if(NewWeatherType == EWeather.None)
            {
                PlayableDirector MessageDirector = Timelines.MessageAnimation;
                TimelineAnimation MessageTimeline = new TimelineAnimation(MessageDirector);
                MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", "天气复原了！");
                AddAnimation(MessageTimeline);      
            }
            else
            {
                GetWeatherParticlesObject(NewWeatherType).SetActive(true);
                PlayableDirector MessageDirector = Timelines.MessageAnimation;
                TimelineAnimation MessageTimeline = new TimelineAnimation(MessageDirector);
                MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", "天气变成了" + GetWeatherTypeName() + "！");
                AddAnimation(MessageTimeline);   
            }
        }
        else
        {
            PlayableDirector MessageDirector = Timelines.MessageAnimation;
            TimelineAnimation MessageTimeline = new TimelineAnimation(MessageDirector);
            MessageTimeline.SetSignalParameter("SignalObject", "MessageSignal", "MessageText", "切换" + GetWeatherTypeName() + "失败了！");
            AddAnimation(MessageTimeline);            
        }
    }

    public void Process(BattleManager InManager)
    {
        if(!ShouldProcess(InManager)) return;
        InManager.TranslateTimePoint(ETimePoint.BeforeChangeWeather, this);
        OriginWeatherType = InManager.GetWeatherType();
        if(InManager.GetWeatherType() == NewWeatherType)
        {
            bSuccessed = false;
        }
        else
        {
            InManager.SetWeather(NewWeatherType, 5);
            bSuccessed = true;
        }
        InManager.AddAnimationEvent(this);
        InManager.TranslateTimePoint(ETimePoint.AfterChangeWeather, this);
    }

    public EventType GetEventType()
    {
        return EventType.WeatherChange;
    }

}
