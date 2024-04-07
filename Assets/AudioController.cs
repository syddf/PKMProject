using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioClip firstClip;
    public AudioClip secondClip;

    private AudioSource audioSource;
    void Start()
    {
        // 获取AudioSource组件
        audioSource = GetComponent<AudioSource>();

        // 播放第一段音频
        PlayFirstClip();
    }

    void PlayFirstClip()
    {
        // 设置第一段音频并播放
        audioSource.clip = firstClip;
        audioSource.Play();

        // 等待第一段音频播放完成后，开始播放第二段音频
        Invoke("PlaySecondClip", firstClip.length);
    }

    void PlaySecondClip()
    {
        // 设置第二段音频并循环播放
        audioSource.clip = secondClip;
        audioSource.loop = true;
        audioSource.Play();
    }
}