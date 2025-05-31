using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("移动设置")]
    [Tooltip("角色基础移动速度（单位：单位/秒）")]
    public float moveSpeed = 5f;

    [Header("跳跃设置")]
    [Tooltip("跳跃时瞬时向上的初速度（单位：N）")]
    public float jumpForce = 12f;

    [Header("地面层")]
    [Tooltip("地面层级 Mask，确保地面所在 Layer 包含在此 Mask 中。")]
    public LayerMask whatIsGround;

    private Rigidbody2D rb;
    private float moveInput;       // 水平移动值
    private bool isGrounded;       // 是否正站在地面上
    private bool jumpPressed;      // 是否按下跳跃键
    private bool isFacingRight = true;  // 角色是否面向右边

    // 公开朝向状态的只读属性
    public bool IsFacingRight => isFacingRight;

    private ArmGet _armGet;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // 只有在允许移动时才获取输入
        if (ArmGet.canMove)
        {
            //获取水平输入
            moveInput = Input.GetAxisRaw("Horizontal");

            //是否按下跳跃键
            if (Input.GetButtonDown("Jump"))
            {
                jumpPressed = true;
            }
        }
        else
        {
            // 当不允许移动时，清除所有输入
            moveInput = 0;
            jumpPressed = false;
        }
    }

    private void FixedUpdate()
    {
        // 只有在允许移动时才执行移动
        if (ArmGet.canMove)
        {
            //水平移动
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

            if (jumpPressed && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpPressed = false;
            }

            //角色翻转
            if (moveInput > 0.01f && !isFacingRight)
            {
                FlipCharacter();
            }
            else if (moveInput < -0.01f && isFacingRight)
            {
                FlipCharacter();
            }
        }
        else
        {
            // 当不允许移动时，保持垂直速度（重力）但停止水平移动
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    // 公开翻转方法供外部调用
    public void FlipCharacter()
    {
        // 翻转角色朝向
        isFacingRight = !isFacingRight;
        
        // 使用旋转而不是缩放来翻转角色
        Vector3 rotation = transform.eulerAngles;
        rotation.y = isFacingRight ? 0f : 180f;
        transform.eulerAngles = rotation;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
