using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    
    private float rotationSpeed = 50f;
    public HoleRotation _holeRotation;
    
    void Update()
    {
        if (_holeRotation.isRotating)
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactivity"))
        {
            isRotating = true;
        }
    }*/
}
