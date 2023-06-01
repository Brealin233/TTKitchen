using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class StoveFizzleSound : MonoBehaviour
{
    public static StoveFizzleSound Instance { get; private set; }
    
    [SerializeField] private StoveCounter stoveCounter;
    
    public event EventHandler stoveFizzleSoundEvent;

    private void Awake()
    {
        Instance = this;
    }

    public void SetStoveSound()
    {
        if (GetStoveSoundState())
        {
            stoveFizzleSoundEvent?.Invoke(this,EventArgs.Empty);
        }
    }

    private bool GetStoveSoundState()
    {
        return stoveCounter.state.Value == StoveCounter.State.Cooking || stoveCounter.state.Value == StoveCounter.State.Fried;
    }
}
