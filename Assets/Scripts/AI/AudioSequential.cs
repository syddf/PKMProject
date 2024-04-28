using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSequential : MonoBehaviour
{
    public List<AudioClip> AudioList = new List<AudioClip>();
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        StartCoroutine(PlayTwoClipsSequentially());
    }

    private System.Collections.IEnumerator PlayTwoClipsSequentially()
    {
        foreach(var Audio in AudioList)
        {
            audioSource.PlayOneShot(Audio);
            yield return new WaitForSeconds(Audio.length);
        }
    }
}