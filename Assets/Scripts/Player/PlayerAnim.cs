using Unity.Netcode;
using UnityEngine;

public class PlayerAnim : NetworkBehaviour
{
    private PlayerController playerController;
    private Animator animator;
    
    private const string IS_WALKING = "IsWalking";

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        
        animator.SetBool(IS_WALKING,playerController.IsWalking());
    }
}
