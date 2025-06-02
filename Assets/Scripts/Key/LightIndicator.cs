using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightIndicator : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("ƥ���ʹ�õ���ɫ")]
    public Color greenColor = Color.green;

    private Color redColor;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
      
        // ��¼��ʼ��ɫ
        redColor = sr.color;
    }

    /// <summary>
    /// �����л�����ɫ
    /// </summary>
    public void TurnGreen()
    {
        sr.color = greenColor;
       
    }

    /// <summary>
    /// �������óɺ�ɫ
    /// </summary>
    public void TurnRed()
    {
        sr.color = redColor;
    }
}
