using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class RandomTimelinePlayer : MonoBehaviour
{
    private PlayableDirector playableDirector;
    public PlayableAsset[] timelines;
    private int LastIndex = 0;
    void Start()
    {
        playableDirector = this.GetComponent<PlayableDirector>();
        // 检查是否有可播放的Timeline
    }

    public void RandomPlay()
    {
        if (timelines == null || timelines.Length == 0 || playableDirector == null)
        {
            Debug.LogError("没有指定PlayableDirector或Timeline！");
            return;
        }
        int randomIndex = Random.Range(0, timelines.Length);
        if(LastIndex == randomIndex)
        {
            randomIndex = (randomIndex + 1) % timelines.Length;
        }
        PlayTimeline(timelines[randomIndex]);
        LastIndex = randomIndex;
    }

    void PlayTimeline(PlayableAsset timeline)
    {
        // 将选择的Timeline分配给PlayableDirector
        playableDirector.playableAsset = timeline;

        // 播放选定的Timeline
        playableDirector.Play();
    }
}