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
    public static bool ForbidChangeWeather = false;

    public EWeather GetWeatherType()
    {
        return NewWeatherType;
    }

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
        if(OriginWeatherType == NewWeatherType) return false;
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
        OriginWeatherType = InManager.GetWeatherType();
        if(!ShouldProcess(InManager)) return;
        InManager.TranslateTimePoint(ETimePoint.BeforeChangeWeather, this);
        if(InManager.GetWeatherType() == NewWeatherType || ForbidChangeWeather)
        {
            bSuccessed = false;
        }
        else
        {
            int Turn = 5;
            if(SourcePokemon != null && SourcePokemon.HasItem("冰冷岩石") && NewWeatherType == EWeather.Snow)
            {
                Turn = 8;
            }
            if(SourcePokemon != null && SourcePokemon.HasItem("沙沙岩石") && NewWeatherType == EWeather.Sand)
            {
                Turn = 8;
            }
            if(SourcePokemon != null && SourcePokemon.HasItem("炽热岩石") && NewWeatherType == EWeather.SunLight)
            {
                Turn = 8;
            }
            if(SourcePokemon != null && SourcePokemon.HasItem("潮湿岩石") && NewWeatherType == EWeather.Rain)
            {
                Turn = 8;
            }
            if(NewWeatherType == EWeather.Snow && InManager.HasSpecialRule("特殊规则(得抚)"))
            {
                Turn = 999;
                ForbidChangeWeather = true;
            }
            if(NewWeatherType == EWeather.Sand && InManager.HasSpecialRule("特殊规则(南厦)"))
            {
                Turn = 999;
                ForbidChangeWeather = true;
            }
            InManager.SetWeather(NewWeatherType, Turn);
            if(NewWeatherType != EWeather.None)
            {
                InManager.RecordChangeWeather();
            }
            bSuccessed = true;
        }
        InManager.AddAnimationEvent(this);
        InManager.TranslateTimePoint(ETimePoint.AfterChangeWeather, this);
        if(bSuccessed && NewWeatherType != EWeather.None)
        {
            if(InManager.HasSpecialRule("特殊规则(奇巴纳)"))
            {
                BattlePokemon TargetPokemon = InManager.GetBattlePokemons()[0];
                if(TargetPokemon.IsDead() == false)
                {
                    DamageEvent damageEvent = new DamageEvent(TargetPokemon, TargetPokemon.GetMaxHP() / 2, "特殊规则(奇巴纳)");
                    damageEvent.Process(InManager);
                }
            }
        }
    }

    public EventType GetEventType()
    {
        return EventType.WeatherChange;
    }

}
