using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class StoveCounter : BaseCounter,IWasVisualCounter
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
    
    private State state;
    private float fryingTimeMax;
    private float burnedTimeMax;

    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                break;
            case State.Cooking:
                fryingTimeMax += Time.deltaTime;
                
                fryingObjectSO = GetInputFryingObject();
                
                counterVisualEvent?.Invoke(this,new IWasVisualCounter.counterVisualEventClass()
                {
                    fillAmount = fryingTimeMax / fryingObjectSO.fryingTimerMax
                });
                
                if (fryingTimeMax > fryingObjectSO.fryingTimerMax)
                {
                    DestroyKitchenObject(GetKitchenObject());

                    KitchenObject.SpawnKitchenObject(fryingObjectSO.outputKitchenObject.GetKitchenObjectSO(),this);
                    stoveCounterVisualEvent?.Invoke(this,new StoveCounterVisualEvent
                    {
                        state = state
                    });
                    
                    
                    
                    state = State.Fried;
                    fryingTimeMax = 0f;
                }
                
                break;
            case State.Fried:
                burnedTimeMax += Time.deltaTime;

                burnedObjectSO = getInputBurnedObject();

                counterVisualEvent?.Invoke(this, new IWasVisualCounter.counterVisualEventClass()
                {
                    fillAmount = burnedTimeMax / burnedObjectSO.burnedTimerMax
                });
                
                if (burnedTimeMax > burnedObjectSO.burnedTimerMax)
                {
                    // todo: fried state can move anywhere and the statebar alive
                    state = State.Burned;
                    DestroyKitchenObject(GetKitchenObject());
                    
                    KitchenObject.SpawnKitchenObject(burnedObjectSO.outputKitchenObject.GetKitchenObjectSO(),this);
                    stoveCounterVisualEvent?.Invoke(this,new StoveCounterVisualEvent
                    {
                        state = state
                    });

                    burnedTimeMax = 0f;
                }
                
                break;
            case State.Burned:
                counterVisualEvent?.Invoke(this, new IWasVisualCounter.counterVisualEventClass()
                {
                    fillAmount = 0
                });
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
                    playerController.GetKitchenObject().SetKitchenObjectParent(this);
                    playerController.ClearKitchenObject();
                    
                    state = State.Cooking;
                    stoveCounterVisualEvent?.Invoke(this,new StoveCounterVisualEvent
                    {
                        state = state
                    });
                }
            }
        }
        else
        {
            if (!playerController.HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(playerController);
                ClearKitchenObject();
                
                state = State.Idle;
                stoveCounterVisualEvent?.Invoke(this,new StoveCounterVisualEvent
                {
                    state = state
                });
                counterVisualEvent?.Invoke(this, new IWasVisualCounter.counterVisualEventClass()
                {
                    fillAmount = 0
                });
            }
        }   
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

    private FryingObjectSO GetInputFryingObject()
    {
        foreach (FryingObjectSO fryingObjectSO in fryingObjectSOs)
        {
            if (fryingObjectSO.inputKitchenObject.GetKitchenObjectSO() == GetKitchenObject().GetKitchenObjectSO())
            {
                return fryingObjectSO;
            }
        }

        return null;
    }
    
    private BurnedObjectSO getInputBurnedObject()
    {
        foreach (BurnedObjectSO burnedObject in burnedObjectSOs)
        {
            // if (burnedObject.inputKitchenObject.GetKitchenObjectSO() == GetKitchenObject().GetKitchenObjectSO())
            // {
                return burnedObject;
            // }
        }

        return null;
    }
}
