using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeTrigger : MonoBehaviour
{
    public RenderSettings settings;
    private MainMusic music;
    public Material Hell;
    public Material Ok;

    void Start()
    {
        music = FindObjectOfType<MainMusic>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            RenderSettings.skybox = Hell;
            music.PlayHellTheme();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            RenderSettings.skybox = Ok;
            music.PlayRandomAmbient();
        }
    }
}
