using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class AudioEvent
{
    public static UnityAction<AudioType> OnPlayAudio;

    public static void RaiseOnPlayAudio(AudioType audioType)
    {
        OnPlayAudio?.Invoke(audioType);
    }
}
