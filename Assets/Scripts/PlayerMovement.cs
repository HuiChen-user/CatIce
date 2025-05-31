using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("�ƶ�����")]
    [Tooltip("��ɫ�����ƶ��ٶȣ���λ����λ/�룩")]
    public float moveSpeed = 5f;

    [Header("��Ծ����")]
    [Tooltip("��Ծ��˲�����ϵĳ�������λ��N��")]
    public float jumpForce = 12f;

    [Header("������")]
    [Tooltip("����㼶 Mask��ȷ���������� Layer �������ڴ� Mask �С�")]
    public LayerMask whatIsGround;

    private Rigidbody2D rb;
    private float moveInput;       // ˮƽ�ƶ�ֵ
    private bool isGrounded;       // �Ƿ���վ�ڵ�����
    private bool jumpPressed;      // �Ƿ�����Ծ��

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //��ȡˮƽ����
        moveInput = Input.GetAxisRaw("Horizontal");

        //�Ƿ�����Ծ��
        if (Input.GetButtonDown("Jump"))
        {
            jumpPressed = true;
        }
    }

    private void FixedUpdate()
    {
       

        
        //ˮƽ�ƶ�
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (jumpPressed && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // ֻҪ�ǽӴ����棬�Ϳ�����Ծ
            jumpPressed = false; // ������Ծ����
        }

        //��ɫ����
        if (moveInput > 0.01f)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (moveInput < -0.01f)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true; // �����ײ�����棬����Ϊ�ڵ�����
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = false; // �뿪����ʱ�����Ϊ���ڵ�����
        }
    }
}
