using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioEvent : MonoBehaviour
{
    public static UnityAction<AudioType> OnPlayAudio;

    public static void RaiseOnPlayAudio(AudioType audioType)
    {
        OnPlayAudio?.Invoke(audioType);
    }
}
