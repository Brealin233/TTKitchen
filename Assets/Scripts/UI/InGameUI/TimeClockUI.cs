using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class TimeClockUI : MonoBehaviour
{
    public static event EventHandler promptTextEvent;
    public static void ResetStaticData()
    {
        promptTextEvent = null;
    }
    
    [SerializeField] private Transform fillClockBackGround;
    [SerializeField] private Transform fillClockPercent;
    
    private void Start()
    {
        Hide();
    }

    private void Update()
    {
        if (TTKitchenGameManager.Instance.IsInGameState())
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
        
        promptTextEvent?.Invoke(this,EventArgs.Empty);
    }

    private void Hide()
    {
        fillClockBackGround.gameObject.SetActive(false);
        fillClockPercent.gameObject.SetActive(false);
    }
}
