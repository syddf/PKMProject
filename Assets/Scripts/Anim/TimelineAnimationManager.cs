using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineAnimationManager : MonoBehaviour
{    
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private static GameObject GlobalTimelineAnimationManager;
    public static TimelineAnimationManager GetGlobalTimelineAnimationManager()
    {
        if(GlobalTimelineAnimationManager == null)
        {
            GlobalTimelineAnimationManager = GameObject.Find("G_Anims");
        }
        return GlobalTimelineAnimationManager.GetComponent<TimelineAnimationManager>();
    }
    public PlayableDirector BattleStartAnimation;
    public PlayableDirector AbilityStateAnimation;
    public PlayableDirector BuffAnimation;
    public PlayableDirector DebuffAnimation;
    public PlayableDirector MessageAnimation;
    public PlayableDirector DefeatedAnimation;
    public PlayableDirector SwitchWhenDefeatedAnimation;
    public PlayableDirector SwitchAnimation;
    public PlayableDirector SwitchTerrainAnimation;
    public PlayableDirector ResetTerrainAnimation;
    public PlayableDirector HealAnimation;
    public PlayableDirector DamageAnimation;
}
