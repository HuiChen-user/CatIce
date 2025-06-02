using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyHole : MonoBehaviour
{
    [Header("要检测的钥匙")]
    public Transform keyTransform;

    [Header("匹配距离阈值")]
    public float matchDistance = 0.5f;

    [Header("匹配事件")]
    public UnityEvent<KeyHole> onKeyMatched;

    [HideInInspector]
    public bool IsMatched = false;

    private void Update()
    {
        if (IsMatched || keyTransform == null)
            return;

        // 每帧计算距离
        float dist = Vector2.Distance(keyTransform.position, transform.position);
        if (dist <= matchDistance)
        {
            IsMatched = true;
            onKeyMatched.Invoke(this);
            keyTransform.transform.position = this.transform.position;
            Destroy(keyTransform.GetComponent<Collider2D>()); // 禁止再次触发
            keyTransform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
    }

    // 在 Scene 视图里可视化匹配范围
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = IsMatched ? Color.green * 0.5f : Color.red * 0.5f;
        Gizmos.DrawSphere(transform.position, matchDistance);
    }
}
