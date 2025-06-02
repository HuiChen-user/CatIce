using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    
    

    private bool isOpen = false;

    /// <summary>
    /// 由 LevelManager 调用，当所有 KeyHole 都匹配后执行开门
    /// </summary>
    public void OpenDoor()
    {
        if (isOpen) return;
        isOpen = true;
   
        this.gameObject.SetActive(false);
            
        
    }
}
