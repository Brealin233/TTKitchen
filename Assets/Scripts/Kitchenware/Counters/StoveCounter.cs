using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class StoveCounter : BaseCounter, IWasVisualCounter
{
    public event EventHandler<IWasVisualCounter.counterVisualEventClass> counterVisualEvent;
    public event EventHandler<StoveCounterVisualEvent> stoveCounterVisualEvent;

    public class StoveCounterVisualEvent
    {
        public State state;
    }

    public enum State
    {
        Idle,
        Cooking,
        Fried,
        Burned
    }

    [SerializeField] private List<FryingObjectSO> fryingObjectSOs;
    [SerializeField] private List<BurnedObjectSO> burnedObjectSOs;
    private FryingObjectSO fryingObjectSO;
    private BurnedObjectSO burnedObjectSO;

    public NetworkVariable<State> state = new NetworkVariable<State>(State.Idle);
    private NetworkVariable<float> fryingTime = new (0f);
    private NetworkVariable<float> burnedTime = new NetworkVariable<float>(0f);
    
    public override void OnNetworkSpawn()
    {
        fryingTime.OnValueChanged += FryingTimeOnValueChanged;
        burnedTime.OnValueChanged += burnedTimeOnValueChanged;
        state.OnValueChanged += StateOnValueChanged;
    }

    private void StateOnValueChanged(State previousValue, State newValue)
    {
        if (state.Value == State.Idle || state.Value == State.Burned)
        {
            counterVisualEvent?.Invoke(this, new IWasVisualCounter.counterVisualEventClass()
            {
                fillAmount = 0
            });
        }
        
        stoveCounterVisualEvent?.Invoke(this, new StoveCounterVisualEvent
        {
            state = state.Value
        });
    }

    private void burnedTimeOnValueChanged(float previousValue, float newValue)
    {
        var burnedTimer = burnedObjectSO != null ? burnedObjectSO.burnedTimerMax : 1f;
        
        counterVisualEvent?.Invoke(this, new IWasVisualCounter.counterVisualEventClass()
        {
            fillAmount = burnedTime.Value / burnedTimer
        });
    }

    private void FryingTimeOnValueChanged(float previousValue, float newValue)
    {
        var fryingTimer = fryingObjectSO != null ? fryingObjectSO.fryingTimerMax : 1f;
        
        counterVisualEvent?.Invoke(this, new IWasVisualCounter.counterVisualEventClass()
        {
            fillAmount = fryingTime.Value / fryingTimer
        });
    }

    private void Update()
    {
        if (!IsServer)
        {
            return;
        }
        
        switch (state.Value)
        {
            case State.Idle:
                break;
            case State.Cooking:
                fryingTime.Value += Time.deltaTime;

                fryingObjectSO = GetInputFryingObject(GetKitchenObject().GetKitchenObjectSO());

                StoveFizzleSound.Instance.SetStoveSound();

                if (fryingTime.Value > fryingObjectSO.fryingTimerMax)
                {
                    // DestroyKitchenObject(GetKitchenObject());
                    KitchenObject.DestoryKitchenObject(GetKitchenObject());

                    KitchenObject.SpawnKitchenObject(fryingObjectSO.outputKitchenObject.GetKitchenObjectSO(), this);

                    state.Value = State.Fried;
                }

                break;
            case State.Fried:
                burnedTime.Value += Time.deltaTime;

                burnedObjectSO = GetInputBurnedObject();
                
                if (burnedTime.Value > burnedObjectSO.burnedTimerMax)
                {
                    // todo: fried state can move anywhere and the statebar alive
                    state.Value = State.Burned;
                    // DestroyKitchenObject(GetKitchenObject());
                    KitchenObject.DestoryKitchenObject(GetKitchenObject());
                    
                    KitchenObject.SpawnKitchenObject(burnedObjectSO.outputKitchenObject.GetKitchenObjectSO(), this);
                    
                    burnedTime.Value = 0f;
                }

                StoveFizzleSound.Instance.SetStoveSound();

                break;
            case State.Burned:
                break;
        }
    }

    public override void InteractPlayer(PlayerController playerController)
    {
        if (!HasKitchenObject())
        {
            if (playerController.HasKitchenObject())
            {
                if (HasRecipeKitchenObject(playerController.GetKitchenObject()))
                {
                    KitchenObject kitchenObject = playerController.GetKitchenObject();
                    kitchenObject.SetKitchenObjectParent(this);

                    InteractPlayerServerRpc(MultipleKitchenObejctNetwork.Instance.GetKitchenObjectSOIndexInList(kitchenObject.GetKitchenObjectSO()));
                }
            }
        }
        else
        {
            if (playerController.HasKitchenObject())
            {
                if (playerController.GetKitchenObject()
                    .GetPlateKitchenObject(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        state.Value = State.Idle;
                        
                        KitchenObject.DestoryKitchenObject(GetKitchenObject());
                    }
                }
            }
            else
            {
                KitchenObject kitchenObject = GetKitchenObject();
                kitchenObject.SetKitchenObjectParent(playerController);
                
                SetMeetParentServerRpc();
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetMeetParentServerRpc()
    {
        state.Value = State.Idle;
        SetMeetParentClientRpc();
    }

    [ClientRpc]
    private void SetMeetParentClientRpc()
    {
        ClearKitchenObject();
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractPlayerServerRpc(int kitchenIndex)
    {
        fryingTime.Value = 0f;
        state.Value = State.Cooking;
        InteractPlayerClientRpc(kitchenIndex);
    }

    [ClientRpc]
    private void InteractPlayerClientRpc(int kitchenIndex)
    {
       fryingObjectSO = GetInputFryingObject(MultipleKitchenObejctNetwork.Instance.GetKitchenObjectSOFromIndex(kitchenIndex));
       burnedObjectSO = GetInputBurnedObject();
    }

    private bool HasRecipeKitchenObject(KitchenObject kitchenObject)
    {
        foreach (FryingObjectSO fryingObjectSO in fryingObjectSOs)
        {
            if (fryingObjectSO.inputKitchenObject.GetKitchenObjectSO() == kitchenObject.GetKitchenObjectSO())
            {
                return true;
            }
        }

        return false;
    }

    private FryingObjectSO GetInputFryingObject(KitchenObjectSO kitchenObjectSO)
    {
        foreach (FryingObjectSO fryingObjectSO in fryingObjectSOs)
        {
            if (fryingObjectSO.inputKitchenObject.GetKitchenObjectSO() == kitchenObjectSO)
            {
                return fryingObjectSO;
            }
        }

        return null;
    }

    private BurnedObjectSO GetInputBurnedObject()
    {
        foreach (BurnedObjectSO burnedObject in burnedObjectSOs)
        {
            return burnedObject;
        }

        return null;
    }

    public bool IsFried()
    {
        return state.Value == State.Fried;
    }
}