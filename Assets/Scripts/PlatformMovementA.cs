using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovementA : MonoBehaviour
{
    
    private float moveSpeed = 2f; // 移动速度

    private Vector3 startPosition;

   
    void Start()
    {
        startPosition = transform.position;
    }

    
    void Update()
    {
        // 使用PingPong在0-4范围内来回移动
        float newY = Mathf.PingPong(Time.time * moveSpeed, 4f);
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
