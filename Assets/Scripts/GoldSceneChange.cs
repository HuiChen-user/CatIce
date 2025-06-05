using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoldSceneChange : MonoBehaviour
{
    public GameObject well;
    public Transform wellroot;
    private void Update()
    {
        if (Gold.isPass)
        {
            well.tag = "Ground";
            well.transform.position =wellroot.position;
            Rigidbody2D wellrb=well.GetComponent<Rigidbody2D>();
            if (wellrb != null)
            {
                wellrb.bodyType =RigidbodyType2D.Static;
            }
           
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
            SceneManager.LoadScene("Gold");
        }
    }
}
