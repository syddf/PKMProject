using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static GameObject GlobalAudioManager;
    public static AudioManager GetGlobalAudioManager()
    {
        if(GlobalAudioManager == null)
        {
            GlobalAudioManager = GameObject.Find("G_Audios");
        }
        return GlobalAudioManager.GetComponent<AudioManager>();
    }
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public AudioSource SuperEffectiveDamage;
    public AudioSource NormalEffectiveDamage;
    public AudioSource NotEffectiveDamage;
    
}
