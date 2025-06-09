using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tv : MonoBehaviour
{
    public Transform playerTransform;  // ��ҵ�Transform
    public float maxDistance = 10f;    // ���Ӱ�����
    public float minVolume = 0.1f;     // ��С����
    public float maxVolume = 1f;       // �������

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
    }

    void Update()
    {
        if (playerTransform != null && audioSource != null)
        {
            // �����������ƵԴ�ľ���
            float distance = Vector2.Distance(transform.position, playerTransform.position);

            // ���ݾ���������������Բ�ֵ��
            float volume = Mathf.Lerp(maxVolume, minVolume, distance / maxDistance);

            // ȷ�������ں���Χ��
            volume = Mathf.Clamp(volume, minVolume, maxVolume);

            // Ӧ������
            audioSource.volume = volume;
        }
    }
}
