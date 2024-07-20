using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class AudioManager : MonoBehaviour
{
    bool UseCry1 = false;
    private static GameObject GlobalAudioManager;
    private Dictionary<string, AudioClip> pkmCryClips = new Dictionary<string, AudioClip>();
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

    public void Start()
    {
        string audioFolderPath = Path.Combine(Application.dataPath, "PokemonCries");
        string[] files = Directory.GetFiles(audioFolderPath);

        // 初始化音频文件数组
        pkmCryClips = new Dictionary<string, AudioClip>();

        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].EndsWith(".wav") || files[i].EndsWith(".mp3") || files[i].EndsWith(".ogg"))
            {
                StartCoroutine(LoadAudioCoroutine(files[i]));
            }
        }
    }


    IEnumerator LoadAudioCoroutine(string filePath)
    {
        // 使用UnityWebRequestMultimedia加载音频文件
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file:///" + filePath, AudioType.UNKNOWN))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                // 成功加载音频文件
                AudioClip audioClip = DownloadHandlerAudioClip.GetContent(www);
                // 在这里使用音频文件
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                fileName = char.ToUpper(fileName[0]) + fileName.Substring(1);
                pkmCryClips.Add(fileName, audioClip);
            }
            else
            {
                // 加载音频文件失败
                Debug.LogError("Failed to load audio file: " + filePath + "\nError: " + www.error);
            }
        }
    }

    public void PlayPkmCry(string pkmName, bool Dead)
    {
        // 检查是否存在该音频文件
        if (pkmCryClips.ContainsKey(pkmName))
        {
            if(UseCry1)
            {
                Cry1Source.pitch = 1.0f;
                if(Dead)
                {
                    Cry1Source.pitch = 0.67f;
                }
                Cry1Source.clip = pkmCryClips[pkmName];
                Cry1Source.Play();
            }
            else
            {   
                Cry2Source.pitch = 1.0f;
                if(Dead)
                {
                    Cry2Source.pitch = 0.67f;
                }
                Cry2Source.clip = pkmCryClips[pkmName];
                Cry2Source.Play();
            }
            UseCry1 = !UseCry1;
        }
        else
        {
            Debug.LogError("Audio file not found: " + pkmName);
        }
    }

    public void PlayTrickRoomAudio()
    {
        TrickRoomAudio.Play();
    }


    public AudioSource SuperEffectiveDamage;
    public AudioSource NormalEffectiveDamage;
    public AudioSource NotEffectiveDamage;
    public AudioSource Cry1Source;
    public AudioSource Cry2Source;
    public AudioSource TrickRoomAudio;
}
