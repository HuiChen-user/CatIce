using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tv : MonoBehaviour
{
    public Transform playerTransform;  // 玩家的Transform
    public float maxDistance = 10f;    // 最大影响距离
    public float minVolume = 0.1f;     // 最小音量
    public float maxVolume = 1f;       // 最大音量

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
    }

    void Update()
    {
        if (playerTransform != null && audioSource != null)
        {
            // 计算玩家与音频源的距离
            float distance = Vector2.Distance(transform.position, playerTransform.position);

            // 根据距离计算音量（线性插值）
            float volume = Mathf.Lerp(maxVolume, minVolume, distance / maxDistance);

            // 确保音量在合理范围内
            volume = Mathf.Clamp(volume, minVolume, maxVolume);

            // 应用音量
            audioSource.volume = volume;
        }
    }
}
