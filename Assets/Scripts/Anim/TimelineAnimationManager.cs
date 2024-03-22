using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineAnimationManager : MonoBehaviour
{
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
    public PlayableDirector[] Player1SkillAnimation = new PlayableDirector[4];
    public PlayableDirector[] Player2SkillAnimation = new PlayableDirector[4];
    public PlayableDirector[] Player3SkillAnimation = new PlayableDirector[4];
    public PlayableDirector[] Player4SkillAnimation = new PlayableDirector[4];
    public PlayableDirector[] Player5SkillAnimation = new PlayableDirector[4];
    public PlayableDirector[] Player6SkillAnimation = new PlayableDirector[4];
    public PlayableDirector[] Enemy1SkillAnimation = new PlayableDirector[4];
    public PlayableDirector[] Enemy2SkillAnimation = new PlayableDirector[4];
    public PlayableDirector[] Enemy3SkillAnimation = new PlayableDirector[4];
    public PlayableDirector[] Enemy4SkillAnimation = new PlayableDirector[4];
    public PlayableDirector[] Enemy5SkillAnimation = new PlayableDirector[4];
    public PlayableDirector[] Enemy6SkillAnimation = new PlayableDirector[4];
}
