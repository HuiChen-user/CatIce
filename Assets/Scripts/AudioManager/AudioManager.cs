using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum AudioType
{
    Hand,
    Interact,
    Start,
    Throw,
    Walk,
    Radio,
    TV,
    UI_Select,
    UI_Tap
}
public class AudioManager : MonoBehaviour
{
    [Header("音频数据")] public List<AudioData> audioData;
    private Dictionary<AudioType, AudioData> _audioDict;
        
    [Serializable]
    public struct AudioData
    {
        public AudioType AudioType;
        public AudioClip[] audioClip;
        public AudioSource AudioSource;
    }
    
    private static AudioManager _instance;

    public static AudioManager instance
    {
        get
        {
            if (_instance != null)
            {
                _instance = FindObjectOfType<AudioManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }
    
    //确保实例在场景切换中不会被摧毁
    private void Awake()
    {
       
            

            // 确保 OnEnable 能被触发
            inAudioSource();
        

    }

    private void OnEnable()
    {
        Debug.Log("OnEnable");
        AudioEvent.OnPlayAudio += PlayerAudio;
    }

    private void OnDisable()
    {
        Debug.Log("Disable");
        AudioEvent.OnPlayAudio -= PlayerAudio;
    }

    private void inAudioSource()
    {
        _audioDict = new Dictionary<AudioType, AudioData>(); 
        foreach (var audio  in audioData)
        {
            int i = Random.Range(0, audio.audioClip.Length);
            if (audio.AudioSource!=null)
            {
                audio.AudioSource.clip = audio.audioClip[i];
            }

            _audioDict[audio.AudioType] = audio;
        }
    }

    private void PlayerAudio(AudioType audioType)
    {
        if (_audioDict.TryGetValue(audioType,out AudioData audioData))
        {
            _audioDict[audioType].AudioSource.Play();
            Debug.Log(audioType);
        }
    }
}
