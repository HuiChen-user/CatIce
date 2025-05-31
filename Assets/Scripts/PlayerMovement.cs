using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("移动参数")]
    [Tooltip("角色左右移动速度（单位：单位/秒）")]
    public float moveSpeed = 5f;

    [Header("跳跃参数")]
    [Tooltip("跳跃的瞬间向上的冲力（单位：N）")]
    public float jumpForce = 12f;

    [Header("地面检测")]
    [Tooltip("地面层级 Mask，确保地面对象的 Layer 被包含在此 Mask 中。")]
    public LayerMask whatIsGround;

    private Rigidbody2D rb;
    private float moveInput;       // 水平移动值
    private bool isGrounded;       // 是否正站在地面上
    private bool jumpPressed;      // 是否按下跳跃键

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //获取水平输入
        moveInput = Input.GetAxisRaw("Horizontal");

        //是否按下跳跃键
        if (Input.GetButtonDown("Jump"))
        {
            jumpPressed = true;
        }
    }

    private void FixedUpdate()
    {
       

        
        //水平移动
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (jumpPressed && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // 只要是接触地面，就可以跳跃
            jumpPressed = false; // 重置跳跃请求
        }

        //角色朝向
        if (moveInput > 0.01f)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (moveInput < -0.01f)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true; // 如果碰撞到地面，就认为在地面上
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = false; // 离开地面时，标记为不在地面上
        }
    }
}
