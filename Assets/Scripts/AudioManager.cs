using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Manager;

    public Sound[] Sounds;
    public AudioMixerGroup Mixer;

    void Awake()
    {
        if (Manager == null)
        {
            Manager = this;
        }

        SetSoundValues();
    }

    private void Start()
    {
        //Level Theme.
        PlaySound("Woods_BackgroundMusic");
    }


    private void SetSoundValues()
    {
        foreach (Sound sound in Sounds)
        {
            sound.Source = gameObject.AddComponent<AudioSource>();

            sound.Source.clip = sound.Clip;
            sound.Source.volume = sound.Volume;
            sound.Source.pitch = sound.Pitch;
            sound.Source.loop = sound.Loop;

            sound.Source.outputAudioMixerGroup = Mixer;
        }
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(Sounds, sound => sound.Name == name);

        if(s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
        }
        else
        {
            s.Source.Play();
        }
    }

    public void Stop(string name)
    {
        Sound song = Array.Find(Sounds, sound => sound.Name == name);
        song.Source.Stop();
    }
}
