using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HookAttachment : MonoBehaviour
{

    [Header("��������")]
    [Tooltip("��������")]
    public float throwForce = 10f; // ��������

    private GameObject grabbedObject; // ��ǰץȡ������
    private bool isGrabbing;         // �Ƿ�����ץȡ����
    private bool ignoreNextCollision; // ������һ����ײ��־

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isGrabbing&&PlayerArmController.canExtend)
        {
            ThrowObject();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ��������˺��Ա�־�������˴���ײ
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

        // ����Ϊ���ӵ�������
        obj.transform.SetParent(transform);

        // �����������������
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.simulated = false;
        }

        // ����λ��
        obj.transform.localPosition = Vector3.zero;
        AudioEvent.RaiseOnPlayAudio(AudioType.Interact);
    }

    void ThrowObject()
    {
        if (grabbedObject == null) return;

        // ������ӹ�ϵ
        grabbedObject.transform.SetParent(null);

        // ����������������
        Rigidbody2D rb = grabbedObject.GetComponent<Rigidbody2D>();
        //rb.gravityScale = 0;
        if (rb != null)
        {
            rb.simulated = true;

            // ������귽��
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (new Vector2(mousePosition.x, mousePosition.y) -
                                (Vector2)transform.position);
            direction = direction.normalized;

            // ʩ������
            rb.AddForce(direction * throwForce, ForceMode2D.Impulse);
        }

        // ���ú�����һ����ײ��־
        ignoreNextCollision = true;

        // ����״̬
        grabbedObject = null;
        isGrabbing = false;
        AudioEvent.RaiseOnPlayAudio(AudioType.Throw);
    }
}
