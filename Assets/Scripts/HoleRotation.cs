using UnityEngine;

public class HoleRotation : MonoBehaviour
{
    
    private float rotationSpeed = 30f; // 旋转速度

    
    public Vector2 targetPosition = Vector2.zero; // 目标位置

    public bool isRotating = false; // 是否正在旋转
    private HookAttachment hookAttachment; // 引用HookAttachment组件

    private void Start()
    {
        // 获取HookAttachment组件引用
        hookAttachment = GetComponent<HookAttachment>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 检查碰撞物体是否带有"Hole"标签
        if (other.CompareTag("Hole") && !isRotating)
        {
            // 如果有HookAttachment组件，禁用它
            if (hookAttachment != null)
            {
                hookAttachment.enabled = false;
            }

            // 移动到指定位置并开始旋转
            transform.position = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
            isRotating = true;

            // 如果物体有Rigidbody2D，将其设置为静态
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Static;
            }
        }
    }

    private void Update()
    {
        // 如果已触发旋转，确保位置保持不变并继续旋转
        if (isRotating)
        {
            // 确保位置保持在目标位置
            transform.position = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
            transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
        }
    }
} 