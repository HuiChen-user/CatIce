using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalPlatform : MonoBehaviour
{
    [Header("�ƶ��ٶȣ���λ����λ/sec��")]
    public float speed = 2f;

    [Header("�����ƶ���Χ���ӳ�ʼλ�����㣩")]
    public float moveDistance = 3f;

    // ��¼ƽ̨��ʼλ��
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        // �� x ���� [0, moveDistance] ���������أ�����Ϊ 2*moveDistance��
        float xOffset = Mathf.PingPong(Time.time * speed, moveDistance);
        // ���㵱ǰ��ʵ��λ��
        transform.position = new Vector3(startPosition.x + xOffset, startPosition.y, startPosition.z);
    }
}
