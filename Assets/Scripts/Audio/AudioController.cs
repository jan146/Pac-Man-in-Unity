using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioController : MonoBehaviour
{

    public Sound[] Sounds;

    void Awake()
    {

        foreach (Sound z in Sounds) {

            z.Source = gameObject.AddComponent<AudioSource>();
            z.Source.clip = z.Clip;
            z.Source.pitch = 1;
            z.Source.volume = OptionsController.Volume;

        }

    }

    public void PlaySound(string name) {

        foreach (Sound z in Sounds) {

            if (z.Name == name)
            {

                z.Source.pitch = 1;
                z.Source.volume = OptionsController.Volume;
                if (name == "Ambient1" || name == "Ambient2") z.Source.loop = true;
                z.Source.Play();

            }

        }

    }

    public void StopSound(string name) {

        foreach (Sound z in Sounds)
        {

            if (z.Name == name)
            {

                z.Source.Stop();

            }

        }

    }

}