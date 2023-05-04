using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterEffect : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private GameObject stoveVisualObject;
    [SerializeField] private Transform stoveParticleObject;

    private void Start()
    {
        stoveCounter.stoveCounterVisualEvent += OnStoveCounterVisualEvent;
    }

    private void OnStoveCounterVisualEvent(object sender, StoveCounter.StoveCounterVisualEvent e)
    {
        bool stoveVisualToggle = e.state == StoveCounter.State.Fried || e.state == StoveCounter.State.Cooking;
        stoveVisualObject.SetActive(stoveVisualToggle);
        stoveParticleObject.gameObject.SetActive(stoveVisualToggle);
    }
}
