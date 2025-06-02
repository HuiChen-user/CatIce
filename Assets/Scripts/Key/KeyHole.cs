using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyHole : MonoBehaviour
{
    [Header("Ҫ����Կ��")]
    public Transform keyTransform;

    [Header("ƥ�������ֵ")]
    public float matchDistance = 0.5f;

    [Header("ƥ���¼�")]
    public UnityEvent<KeyHole> onKeyMatched;

    [HideInInspector]
    public bool IsMatched = false;

    private void Update()
    {
        if (IsMatched || keyTransform == null)
            return;

        // ÿ֡�������
        float dist = Vector2.Distance(keyTransform.position, transform.position);
        if (dist <= matchDistance)
        {
            IsMatched = true;
            onKeyMatched.Invoke(this);
            keyTransform.transform.position = this.transform.position;
            Destroy(keyTransform.GetComponent<Collider2D>()); // ��ֹ�ٴδ���
            keyTransform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
    }

    // �� Scene ��ͼ����ӻ�ƥ�䷶Χ
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = IsMatched ? Color.green * 0.5f : Color.red * 0.5f;
        Gizmos.DrawSphere(transform.position, matchDistance);
    }
}
