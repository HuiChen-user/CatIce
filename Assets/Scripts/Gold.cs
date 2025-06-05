using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gold : MonoBehaviour
{
    public static bool isPass=false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isPass = true;
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene("Level1");
        }
    }
}
