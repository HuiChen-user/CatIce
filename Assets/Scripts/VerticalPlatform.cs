using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalPlatform : MonoBehaviour
{
    [Header("移动速度（单位：单位/sec）")]
    public float speed = 2f;

    [Header("上下移动范围（从初始位置起算）")]
    public float moveDistance = 3f;

    // 记录平台初始位置
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        // 让 y 轴在 [0, moveDistance] 区间里来回（周期为 2*moveDistance）
        float yOffset = Mathf.PingPong(Time.time * speed, moveDistance);
        // 计算当前的实际位置
        transform.position = new Vector3(startPosition.x, startPosition.y + yOffset, startPosition.z);
    }
}
