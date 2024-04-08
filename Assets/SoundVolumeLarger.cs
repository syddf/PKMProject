using UnityEngine;

public class SoundVolumeLarger : MonoBehaviour
{
    public float volumeMultiplier;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 1.0f * volumeMultiplier; // Adjust volume here
    }
}