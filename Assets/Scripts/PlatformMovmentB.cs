using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovmentB : MonoBehaviour
{
    
    private float moveSpeed = 2f; // 移动速度，可在Inspector中调整

    private Vector3 startPosition;
    private float movementRange = 2.5f; // 移动范围（12.5 - 10 = 2.5）
    private float startX = 10f; // 起始X坐标

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // 使用PingPong在10-12.5范围内来回移动
        float newX = startX + Mathf.PingPong(Time.time * moveSpeed, movementRange);
        transform.position = new Vector3(newX, startPosition.y, startPosition.z);
    }
}
