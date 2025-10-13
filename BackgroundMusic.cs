using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic backgroundMusic;

    private AudioSource audioSource;

    void Awake() // ✅ kapital di sini penting
    {
        if (backgroundMusic == null)
        {
            backgroundMusic = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetVolume(float volume)
    {
        if (audioSource != null)
            audioSource.volume = volume; // range 0.0f - 1.0f
    }

    public void Mute()
    {
        if (audioSource != null)
            audioSource.volume = 0f;
    }

    public void RestoreVolume()
    {
        if (audioSource != null)
            audioSource.volume = 1f;
    }
}
