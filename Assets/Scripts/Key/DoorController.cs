using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    
    

    private bool isOpen = false;

    /// <summary>
    /// �� LevelManager ���ã������� KeyHole ��ƥ���ִ�п���
    /// </summary>
    public void OpenDoor()
    {
        if (isOpen) return;
        isOpen = true;
   
        this.gameObject.SetActive(false);
            
        
    }
}
