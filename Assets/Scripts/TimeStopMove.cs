using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopMove : MonoBehaviour
{
    public static bool isMove;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isMove = true;
        }
    }
}
