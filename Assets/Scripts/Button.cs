using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public GameObject EditPanel;
    public GameObject CreditPanel;
   
    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
        AudioEvent.RaiseOnPlayAudio(AudioType.UI_Tap);
    }
    public void ExitGame()
    {
        Application.Quit();
        AudioEvent.RaiseOnPlayAudio(AudioType.UI_Tap);
    }
    public void Credit()
    {
        CreditPanel.SetActive(true);
        EditPanel.SetActive(false);
        AudioEvent.RaiseOnPlayAudio(AudioType.UI_Tap);
    }
    public void Edit()
    {
        EditPanel.SetActive(true);
        CreditPanel.SetActive(false);
        AudioEvent.RaiseOnPlayAudio(AudioType.UI_Tap);
    }

    public void QuitPanel()
    {
        CreditPanel.SetActive(false);
        EditPanel.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CreditPanel.SetActive(false);
            EditPanel.SetActive(false);
        }
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
