using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    private Transform targetTransform;

    private void LateUpdate()
    {
        if (targetTransform == null)
        {
            return;
        }

        transform.parent = targetTransform.parent;
        transform.position = targetTransform.position;
        transform.rotation = targetTransform.rotation;
    }
    
    public void GetTargetTransform(Transform targetTransform)
    {
        this.targetTransform = targetTransform;
    }
}
