using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStaticData : MonoBehaviour
{
    private void Awake()
    {
        CuttingCounter.ResetStaticData();
        GarbageCounter.ResetStaticData();
        ClearCounter.ResetStaticData();
        TimeClockUI.ResetStaticData();
    }
}
