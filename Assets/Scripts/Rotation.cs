using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    
    private float rotationSpeed = 50f;
    private bool isR=false;

    private void Update()
    {
        if (isR)
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }
    public void Ro()
    {
        isR=true;
        
    }
    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactivity"))
        {
            isRotating = true;
        }
    }*/
}
