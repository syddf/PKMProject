using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioClip firstClip;
    public AudioClip secondClip;
    public AudioSource secondSource;

    private AudioSource audioSource;
    public float delaySeconds;
    void Start()
    {
        // 获取AudioSource组件
        audioSource = GetComponent<AudioSource>();
        firstClip.LoadAudioData();
        secondClip.LoadAudioData();
        // 播放第一段音频
        PlayFirstClip();
    }
    public void OnDisable()
    {
        CancelInvoke("PlaySecondClip");
    }

    public void OnEnable()
    {
        if(audioSource && firstClip)
        {
            PlayFirstClip();
        }
    }

    void PlayFirstClip()
    {
        // 设置第一段音频并播放
        secondSource.Stop();
        audioSource.clip = firstClip;
        audioSource.Stop();
//        audioSource.time = 0;
        audioSource.Play();
        // 等待第一段音频播放完成后，开始播放第二段音频
        Invoke("PlaySecondClip", firstClip.length + delaySeconds);
    }

    void PlaySecondClip()
    {
        // 设置第二段音频并循环播放
        audioSource.Stop();
//        secondSource.time = 0;
        secondSource.clip = secondClip;
        secondSource.loop = true;
        secondSource.Play();
    }
}