using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightIndicator : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("匹配后使用的颜色")]
    public Color greenColor = Color.green;

    private Color redColor;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
      
        // 记录初始颜色
        redColor = sr.color;
    }

    /// <summary>
    /// 将灯切换成绿色
    /// </summary>
    public void TurnGreen()
    {
        sr.color = greenColor;
       
    }

    /// <summary>
    /// 将灯重置成红色
    /// </summary>
    public void TurnRed()
    {
        sr.color = redColor;
    }
}
