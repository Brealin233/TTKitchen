using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : NetworkBehaviour, IKitchenObjectParent
{
    public static PlayerController localInstance { get; private set; }

    public static event EventHandler anyPlayerSpawnEvent;
    public static event EventHandler anyPlayerSoundEvent;
    public event EventHandler chopSoundEvent;
    public event EventHandler<BaseCounterSelectedEventArgs> BaseCounterSelectedEvent;

    public class BaseCounterSelectedEventArgs : EventArgs
    {
        public BaseCounter baseCounter;
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 10f;

    [FormerlySerializedAs("clearCounterLayerMask")] [SerializeField]
    private LayerMask baseCounterLayerMask;

    [SerializeField] private Transform kitchenObjectParentPoint;


    private BaseCounter baseCounterSelected;
    private KitchenObject kitchenObject;
    private Vector3 lastMoveDir;
    private bool isWalking;
    private const float PLAYER_HEIGHT = 2f;
    private const float MOVE_REDIUS = .7f;
    private const float RAY_DISTANCE = 2f;

    private void Start()
    {
        GameInputManager.Instance.inputInteractHandler += OnInputInteractHandler;
        GameInputManager.Instance.inputInteractAlternateHandler += OninputInteractAlternateHandler;
    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        HandleMovement();
        HandleInteract();
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            localInstance = this;
        }

        anyPlayerSpawnEvent?.Invoke(this, EventArgs.Empty);
    }

    private void OninputInteractAlternateHandler(object sender, EventArgs e)
    {
        if (Physics.Raycast(transform.position, lastMoveDir, out RaycastHit raycastHit, RAY_DISTANCE,
                baseCounterLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                baseCounter.InteractAlternatePlayer(this);
            }
        }
    }

    private void HandleMovementServerAuth()
    {
        Vector2 inputVector = GameInputManager.Instance.GetMovementVectorNormalized();
        HandleMovementServerRpc(inputVector);
    }

    [ServerRpc(RequireOwnership = false)]
    private void HandleMovementServerRpc(Vector2 inputVector)
    {
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        isWalking = moveDir != Vector3.zero;

        float moveDistance = moveSpeed * Time.deltaTime;

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * PLAYER_HEIGHT,
            MOVE_REDIUS, moveDir, moveDistance);

        if (!canMove)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDirX.x != 0 && !Physics.CapsuleCast(transform.position,
                transform.position + Vector3.up * PLAYER_HEIGHT,
                MOVE_REDIUS, moveDirX, moveDistance);

            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.y).normalized;
                canMove = moveDirZ.y != 0 && !Physics.CapsuleCast(transform.position,
                    transform.position + Vector3.up * PLAYER_HEIGHT,
                    MOVE_REDIUS, moveDirZ, moveDistance);

                if (canMove)
                {
                    moveDir = moveDirZ;
                }
            }
        }

        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }


        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    private void OnInputInteractHandler(object sender, EventArgs e)
    {
        if (TTKitchenGameManager.Instance.GetGameStart())
        {
            TTKitchenGameManager.Instance.SetGameStartState();
            IntroduceUI.Instance.SetHide();
        }

        Vector2 inputVector = GameInputManager.Instance.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastMoveDir = moveDir;
        }

        if (Physics.Raycast(transform.position, lastMoveDir, out RaycastHit raycastHit, RAY_DISTANCE,
                baseCounterLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                baseCounter.InteractPlayer(this);
            }
        }
    }

    private void HandleInteract()
    {
        Vector2 inputVector = GameInputManager.Instance.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        isWalking = moveDir != Vector3.zero;

        if (moveDir != Vector3.zero)
        {
            lastMoveDir = moveDir;
        }

        if (Physics.Raycast(transform.position, lastMoveDir, out RaycastHit raycastHit, RAY_DISTANCE,
                baseCounterLayerMask))
        {
            SetBaseCounter(
                raycastHit.transform.TryGetComponent(out BaseCounter baseCounter) ? baseCounter : null);
        }
        else
        {
            SetBaseCounter(null);
        }
    }

    private void HandleMovement()
    {
        Vector2 inputVector = GameInputManager.Instance.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        isWalking = moveDir != Vector3.zero;

        float moveDistance = moveSpeed * Time.deltaTime;

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * PLAYER_HEIGHT,
            MOVE_REDIUS, moveDir, moveDistance);

        if (!canMove)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDirX.x != 0 && !Physics.CapsuleCast(transform.position,
                transform.position + Vector3.up * PLAYER_HEIGHT,
                MOVE_REDIUS, moveDirX, moveDistance);

            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.y).normalized;
                canMove = moveDirZ.y != 0 && !Physics.CapsuleCast(transform.position,
                    transform.position + Vector3.up * PLAYER_HEIGHT,
                    MOVE_REDIUS, moveDirZ, moveDistance);

                if (canMove)
                {
                    moveDir = moveDirZ;
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

    private void SetBaseCounter(BaseCounter baseCounterSelected)
    {
        this.baseCounterSelected = baseCounterSelected;

        BaseCounterSelectedEvent?.Invoke(this, new BaseCounterSelectedEventArgs()
        {
            baseCounter = baseCounterSelected
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectParentPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        chopSoundEvent?.Invoke(this, EventArgs.Empty);
        anyPlayerSoundEvent?.Invoke(this, EventArgs.Empty);

        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }

    public static void ResetStaticData()
    {
        anyPlayerSpawnEvent = null;
        anyPlayerSoundEvent = null;
    }
}