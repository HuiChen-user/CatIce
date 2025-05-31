using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmGet : MonoBehaviour
{
    [Header("引用设置")]
    [Tooltip("线的Transform组件引用")]
    public Transform armLine;  
    [Tooltip("手的Transform组件引用")]
    public Transform armHand;   
    [Tooltip("伸长速度（单位：每秒移动的单位数）")]
    public float extendSpeed = 5f;  
    
    private Vector3 targetPosition;   // 目标位置
    private bool isMoving = false;    // 是否正在移动
    private bool isExtending = true;  // true为伸展，false为缩回
    private Vector3 initialHandPosition; // 手的初始位置
    private float initialLineScale;    // 线的初始缩放值
    private float targetDistance;      // 目标距离
    private float currentExtendedDistance; // 当前已伸展的距离
    
    // Start is called before the first frame update
    void Start()
    {
        if (armHand != null)
        {
            initialHandPosition = armHand.localPosition;
        }
        if (armLine != null)
        {
            initialLineScale = armLine.localScale.x;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 检测鼠标点击
        if (Input.GetMouseButtonDown(0))
        {
            // 获取鼠标点击的世界坐标
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0; // 确保在2D平面上
            
            // 计算方向并旋转手臂
            Vector3 direction = (mousePos - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            
            // 设置目标位置和开始伸展
            targetPosition = mousePos;
            // 计算需要伸展的总距离
            targetDistance = Vector3.Distance(transform.position, targetPosition);
            currentExtendedDistance = 0f; // 重置当前伸展距离
            isMoving = true;
            isExtending = true;
            
            // 重置手和线的位置
            if (armHand != null)
            {
                armHand.localPosition = initialHandPosition;
            }
            if (armLine != null)
            {
                Vector3 scale = armLine.localScale;
                scale.x = initialLineScale;
                armLine.localScale = scale;
            }
        }
        
        // 处理移动逻辑
        if (isMoving)
        {
            if (isExtending)
            {
                ExtendArm();
            }
            else
            {
                RetractArm();
            }
        }
    }
    
    void ExtendArm()
    {
        if (armLine == null || armHand == null)
        {
            return;
        }
        
        // 计算当前手的位置到目标的距离
        float remainingDistance = targetDistance - currentExtendedDistance;
        
        if (remainingDistance > 0.1f)
        {
            // 计算这一帧应该移动的距离
            float frameExtendDistance = extendSpeed * Time.deltaTime;
            // 确保不会过度伸展
            frameExtendDistance = Mathf.Min(frameExtendDistance, remainingDistance);
            
            // 更新当前伸展距离
            currentExtendedDistance += frameExtendDistance;
            
            // 更新线的长度
            Vector3 lineScale = armLine.localScale;
            lineScale.x = initialLineScale + currentExtendedDistance;
            armLine.localScale = lineScale;
            
            // 更新手的位置
            Vector3 handPos = armHand.localPosition;
            handPos.x = currentExtendedDistance;
            armHand.localPosition = handPos;
        }
        else
        {
            // 确保手精确地到达目标位置
            currentExtendedDistance = targetDistance;
            
            // 更新线的最终长度
            Vector3 lineScale = armLine.localScale;
            lineScale.x = initialLineScale + targetDistance;
            armLine.localScale = lineScale;
            
            // 更新手的最终位置
            Vector3 handPos = armHand.localPosition;
            handPos.x = targetDistance;
            armHand.localPosition = handPos;
            
            // 开始缩回
            isExtending = false;
        }
    }

    void RetractArm()
    {
        if (armLine == null || armHand == null)
        {
            return;
        }

        if (currentExtendedDistance > 0.1f)
        {
            // 计算这一帧应该缩回的距离
            float frameRetractDistance = extendSpeed * Time.deltaTime;
            // 确保不会过度缩回
            frameRetractDistance = Mathf.Min(frameRetractDistance, currentExtendedDistance);
            
            // 更新当前伸展距离
            currentExtendedDistance -= frameRetractDistance;
            
            // 更新线的长度
            Vector3 lineScale = armLine.localScale;
            lineScale.x = initialLineScale + currentExtendedDistance;
            armLine.localScale = lineScale;
            
            // 更新手的位置
            Vector3 handPos = armHand.localPosition;
            handPos.x = currentExtendedDistance;
            armHand.localPosition = handPos;
        }
        else
        {
            // 确保完全缩回到初始位置
            currentExtendedDistance = 0f;
            
            // 重置线的长度
            Vector3 lineScale = armLine.localScale;
            lineScale.x = initialLineScale;
            armLine.localScale = lineScale;
            
            // 重置手的位置
            armHand.localPosition = initialHandPosition;
            
            // 结束移动
            isMoving = false;
        }
    }
}
