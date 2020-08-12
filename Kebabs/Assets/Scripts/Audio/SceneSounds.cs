using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSounds : MonoBehaviour
{

    private static SceneSounds _i;

    public static SceneSounds i
    {
        get
        {
            if (_i == null) _i = (Instantiate(Resources.Load("SceneSounds")) as GameObject).GetComponent<SceneSounds>();
            return _i;
        }
    }



    public SoundAudioClip[] soundAudioClipArray;

    [System.Serializable]
    public class SoundAudioClip
    {
        public AudioManager.Sound sound;
        public AudioClip audioClip;
    }
}
