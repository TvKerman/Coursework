using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicInBattle : MonoBehaviour
{
    public List<AudioClip> audioClips;
    private AudioSource music;

    private void Awake()
    {
        music = GetComponent<AudioSource>();
        PlayRandomMusic();
    }

    void Start()
    {
        music.Play();
    }

    void Update()
    {
        if (music.isPlaying == false)
        {
            PlayRandomMusic();
            music.Play();
        }
        
    }

    private void PlayRandomMusic()
    {
        music.clip = audioClips[Random.Range(0, audioClips.Count)];
    }
}
