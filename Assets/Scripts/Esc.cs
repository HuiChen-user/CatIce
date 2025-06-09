using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Esc : MonoBehaviour
{
    public GameObject menuUI; // ������Ĳ˵�����
    private void Awake()
    {
        menuUI.SetActive(false);
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        if (menuUI == null)
        {
            Debug.LogError("MenuUI reference is not set!");
            this.enabled = false;
            return;
        }

        // Ĭ�����ز˵�
        menuUI.SetActive(false);
    }

    private void Update()
    {
        // ���ESC������
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        bool isMenuActive = menuUI.activeSelf;
        menuUI.SetActive(!isMenuActive);

        
    }
}
