using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Booom : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerArmController.canExtend=true;
            PlayerArmController.canMove=true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        } 
    }
}
