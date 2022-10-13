using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MainMusic : MonoBehaviour
{
    public List<AudioClip> audioClips;
    public AudioClip HellTheme;

    private AudioSource music;

    public AudioSource Music {
        get { return music; }
        set { music = value; }
    }

    private void Awake()
    {
        music = GetComponent<AudioSource>();
        PlayRandomAmbient();
    }

    void Update()
    {
        if (music.isPlaying == false)
        {
            PlayRandomAmbient();
        }
    }

    public void PlayRandomAmbient()
    {
        music.Stop();
        music.clip = audioClips[Random.Range(0, audioClips.Count)];
        music.Play();
    }

    public void PlayHellTheme()
    {
        music.Stop();
        music.clip = HellTheme;
        music.Play();
    }
}
