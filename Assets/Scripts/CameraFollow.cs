using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;

    public Bounds bounds;

    public Transform stopCameraPosition;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (TimeStopMove.isMove)
        {
            TimeStopCameraMove();
        }
        else if (player.transform.position.x + 9 < bounds.max.x&&
                 player.transform.position.x - 9 > bounds.min.x)
        {
            transform.position = new Vector3(player.transform.position.x, 0, -10);
        }
    }

    public void TimeStopCameraMove()
    {
        Time.timeScale = 0;
        transform.position += new Vector3(0.025f, 0, 0);
        player.transform.position += new Vector3(0.002f, 0, 0);
        if (Vector2.Distance(transform.position, stopCameraPosition.transform.position) < 0.1f)
        {
            TimeStopMove.isMove = false;
            Time.timeScale = 1;
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
}
