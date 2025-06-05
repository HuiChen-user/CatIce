using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HookAttachment : MonoBehaviour
{

    [Header("发射设置")]
    [Tooltip("发射力度")]
    public float throwForce = 10f; // 发射力度

    private GameObject grabbedObject; // 当前抓取的物体
    private bool isGrabbing;         // 是否正在抓取物体
    private bool ignoreNextCollision; // 忽略下一次碰撞标志

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isGrabbing&&PlayerArmController.canExtend)
        {
            ThrowObject();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 如果设置了忽略标志，跳过此次碰撞
        if (ignoreNextCollision)
        {
            ignoreNextCollision = false;
            return;
        }

        if (collision.gameObject.CompareTag("Interactivity") && !isGrabbing)
        {
            GrabObject(collision.gameObject);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    void GrabObject(GameObject obj)
    {
        isGrabbing = true;
        grabbedObject = obj;

        // 设置为钩子的子物体
        obj.transform.SetParent(transform);

        // 禁用物体的物理属性
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.simulated = false;
        }

        // 重置位置
        obj.transform.localPosition = Vector3.zero;
    }

    void ThrowObject()
    {
        if (grabbedObject == null) return;

        // 解除父子关系
        grabbedObject.transform.SetParent(null);

        // 重新启用物理属性
        Rigidbody2D rb = grabbedObject.GetComponent<Rigidbody2D>();
        //rb.gravityScale = 0;
        if (rb != null)
        {
            rb.simulated = true;

            // 计算鼠标方向
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (new Vector2(mousePosition.x, mousePosition.y) -
                                (Vector2)transform.position);
            direction = direction.normalized;

            // 施加力度
            rb.AddForce(direction * throwForce, ForceMode2D.Impulse);
        }

        // 设置忽略下一次碰撞标志
        ignoreNextCollision = true;

        // 重置状态
        grabbedObject = null;
        isGrabbing = false;
    }
}
