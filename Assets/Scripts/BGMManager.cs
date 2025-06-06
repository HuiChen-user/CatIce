using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    [System.Serializable]
    public class SceneBGM
    {
        public string sceneName;
        public AudioClip bgm;
        public bool loop = true;
        public float volume = 1f;
    }

    public SceneBGM[] sceneBGMs; // ��Inspector������ÿ��������BGM
    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            SceneManager.sceneLoaded += OnSceneLoaded; // �������������¼�
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlaySceneBGM(scene.name);
    }

    public void PlaySceneBGM(string sceneName)
    {
        foreach (var sceneBGM in sceneBGMs)
        {
            if (sceneBGM.sceneName == sceneName)
            {
                // �����ظ�����ͬһ��BGM
                if (audioSource.clip != sceneBGM.bgm || !audioSource.isPlaying)
                {
                    audioSource.clip = sceneBGM.bgm;
                    audioSource.loop = sceneBGM.loop;
                    audioSource.volume = sceneBGM.volume;
                    audioSource.Play();
                }
                return;
            }
        }

        // ���û�ҵ���Ӧ������BGM��ֹͣ����
        audioSource.Stop();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
