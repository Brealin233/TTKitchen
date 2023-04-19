using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameInputManager gameInputManager;

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private LayerMask clearCounterLayerMask;

    private Vector3 lastMoveDir;
    private bool isWalking;
    private const float PLAYERHEIGHT = 2f;
    private const float MOVEREDIUS = .7f;
    private const float RAYREDIUS = 2f;
    private const float RAYDISTANCE = 2f;


    private void Start()
    {
        gameInputManager.inputInteractHandler += OnInputInteractHandler;
    }


    private void Update()
    {
        HandleMovement();
        // HandleInteract();
    }
    
    private void OnInputInteractHandler(object sender, EventArgs e)
    {
        Vector2 inputVector = gameInputManager.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastMoveDir = moveDir;
        }
        //Physics.Raycast(transform.position,moveDir,out RaycastHit raycastHit,RAYDISTANCE,clearCounterLayerMask)
    
        if (Physics.CapsuleCast(transform.position,transform.position + Vector3.up * PLAYERHEIGHT,RAYREDIUS,lastMoveDir,out RaycastHit raycastHit,RAYDISTANCE,clearCounterLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                clearCounter.InteractPlayer();
            }
        }
    }

    // todo:delete this method?
    // private void HandleInteract()
    // {
    //     Vector2 inputVector = gameInputManager.GetMovementVectorNormalized();
    //     Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
    //     isWalking = moveDir != Vector3.zero;
    //
    //     if (moveDir != Vector3.zero)
    //     {
    //         lastMoveDir = moveDir;
    //     }
    //     
    //     if (Physics.Raycast(transform.position,lastMoveDir,out RaycastHit raycastHit,RAYDISTANCE,clearCounterLayerMask))
    //     {
    //         if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
    //         {
    //             // clearCounter.InteractPlayer();
    //         }
    //     }
    // }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInputManager.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        isWalking = moveDir != Vector3.zero;

        float moveDistance = moveSpeed * Time.deltaTime;

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * PLAYERHEIGHT,
            MOVEREDIUS, moveDir, moveDistance);

        if (!canMove)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * PLAYERHEIGHT,
                MOVEREDIUS, moveDirX, moveDistance);

            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.y).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * PLAYERHEIGHT,
                    MOVEREDIUS, moveDirZ, moveDistance);

                if (canMove)
                {
                    moveDir = moveDirZ;
                }
                else
                {
                    // todo: cannot move any
                }
            }
        }

        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }


        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    public bool IsWalking()
    {
        return isWalking;
    }
}