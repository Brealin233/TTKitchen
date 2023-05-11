using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class TimeClockUI : MonoBehaviour
{
    [SerializeField] private Transform fillClockBackGround;
    [SerializeField] private Transform fillClockPercent;
    
    private void Start()
    {
        Hide();
    }

    private void Update()
    {
        if (TTKitchenGameManager.Instance.IseInGameState())
        {
            Show();
            
            fillClockPercent.GetComponent<Image>().fillAmount = 1 - 
                (TTKitchenGameManager.Instance.GetInGameTime() / TTKitchenGameManager.Instance.GetInGameTimeMax());
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        fillClockBackGround.gameObject.SetActive(true);
        fillClockPercent.gameObject.SetActive(true);
    }

    private void Hide()
    {
        fillClockBackGround.gameObject.SetActive(false);
        fillClockPercent.gameObject.SetActive(false);
    }
}
