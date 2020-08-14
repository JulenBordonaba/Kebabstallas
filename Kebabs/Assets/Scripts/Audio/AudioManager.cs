using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioManager
{

    public enum Sound {
        HIT,
        BOTTLECRASH,
        EXPLOSIONPATO,
        RAYONEGRO,
        HUESO,
        RISA,
        CARTEL,
        MAGIAAZUL,
        BOMBACORAZON,
        VELOCIDAD,
        ATAQUEUP,
        EXPLOSIONAZUL,
        VICTORIA,
        DERROTA,
        PEDETE
    }

    
    
    public static void PlaySound(Sound sound)
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource aus = soundGameObject.AddComponent<AudioSource>();
        SoundTypeVolume tipoVolumen = soundGameObject.AddComponent<SoundTypeVolume>();
        tipoVolumen.soundType = SoundType.effect;
        tipoVolumen.audioSource = aus;
        
        soundGameObject.AddComponent<FinishSound>();
        aus.PlayOneShot(GetAudioClip(sound));
    }

    private static AudioClip GetAudioClip (Sound sound)
    {
        foreach (SceneSounds.SoundAudioClip soundAudioClip in SceneSounds.i.soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
            
        }
        Debug.LogError("Sound" + sound + " not found");
        return null;
    }
}
