using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStepSound : MonoBehaviour
{
    public static event EventHandler playerStepSoundEvent;

    private float stepTime;
    private float stepTimeMax = .1f;

    private void Update()
    {
        stepTime -= Time.deltaTime;

        if (stepTime < 0f)
        {
            stepTime = stepTimeMax;

            // if (PlayerController.Instance.IsWalking())
            {
                // todo: solve it
                playerStepSoundEvent?.Invoke(this,EventArgs.Empty);
            }
        }
    }
}
