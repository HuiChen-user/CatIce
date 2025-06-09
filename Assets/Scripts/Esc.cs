using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Esc : MonoBehaviour
{
    public GameObject menuUI; // 引用你的菜单界面
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

        // 默认隐藏菜单
        menuUI.SetActive(false);
    }

    private void Update()
    {
        // 检测ESC键按下
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
