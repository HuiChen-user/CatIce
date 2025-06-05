using System;
using Unity.VisualScripting;
using UnityEngine;

public class CircularRotation : MonoBehaviour
{
    
    private float rotationSpeed = 30f; // 旋转速度

    
    public Vector2 fixedPosition = Vector2.zero; // 物体固定位置

  

    public void Circular()
    {
       
        
        transform.position = new Vector3(fixedPosition.x, fixedPosition.y, transform.position.z);
        transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
    }
} 