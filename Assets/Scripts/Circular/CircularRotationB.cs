using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularRotationB : MonoBehaviour
{
    private float rotationSpeed = 30f; // 旋转速度
    public void CircularB()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
