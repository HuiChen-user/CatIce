using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Esc : MonoBehaviour
{
    public GameObject EditPanel;
    private bool isOpen=false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&&!isOpen)
        {
            
            EditPanel.SetActive(true);
            
        }
        if (Input.GetKeyDown(KeyCode.Escape) && isOpen)
        {
            
            EditPanel.SetActive(false);
            
        }
    }
}
