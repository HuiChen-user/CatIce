using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkAnimation : MonoBehaviour
{
    private Animator animator;
    private Transform parentTransform;
    private PlayerMovement playerMovement;
    // Start is called before the first frame update
    void Start()
    {
        parentTransform = transform.parent;
        if (parentTransform != null)
        {
            playerMovement = parentTransform.GetComponent<PlayerMovement>();
        }
        
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isMoving = (playerMovement.moveInput != 0);
        animator.SetBool("isWalking", isMoving);
    }
}
