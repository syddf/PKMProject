using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class RandomTimelinePlayer : MonoBehaviour
{
    private PlayableDirector playableDirector;
    public PlayableAsset[] timelines;

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
        PlayTimeline(timelines[randomIndex]);
    }

    void PlayTimeline(PlayableAsset timeline)
    {
        // 将选择的Timeline分配给PlayableDirector
        playableDirector.playableAsset = timeline;

        // 播放选定的Timeline
        playableDirector.Play();
    }
}