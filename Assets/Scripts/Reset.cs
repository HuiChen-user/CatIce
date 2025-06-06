using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour
{
  
    
        public void Rs()
        {
            PlayerArmController.canExtend = true;
            PlayerArmController.canMove = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
}
