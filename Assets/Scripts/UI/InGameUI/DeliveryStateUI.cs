using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryStateUI : MonoBehaviour
{
   public static DeliveryStateUI Instance { get; private set; }
   
   [SerializeField] private TextMeshProUGUI stateText;
   [SerializeField] private Image stateImage;
   [SerializeField] private Sprite successImage;
   [SerializeField] private Sprite failedImage;

   private const string SUCCESS_TEXT = "SUCCESS DELIVERED";
   private const string FAILED_TEXT = "FAILED DELIVERED";

   private bool isShowing;
   private float showTime = 1f;

   private void Awake()
   {
      Instance = this;
   }

   private void Start()
   {
      Hide();
      
      DeliveryManager.Instance.deliverySuccessBoardEvent += OnDeliverySuccessBoardEvent;
      DeliveryManager.Instance.deliveryFailedBoardEvent += OnDeliveryFailedBoardEvent;
   }

   private void Update()
   {
      if (isShowing)
      {
         showTime -= Time.deltaTime;
         
         if (showTime < 0f)
         {
            Hide();
         }
      }
   }

   private void OnDeliveryFailedBoardEvent(object sender, EventArgs e)
   {
      stateImage.sprite = failedImage;
      stateText.text = FAILED_TEXT;
      stateText.color = Color.red;
   }

   private void OnDeliverySuccessBoardEvent(object sender, EventArgs e)
   {
      stateImage.sprite = successImage;
      stateText.text = SUCCESS_TEXT;
      stateText.color = Color.green;
   }

   public void Show()
   {
      isShowing = true;
      foreach (Transform child in transform)
      {
         child.gameObject.SetActive(true);
      }

      showTime = 1f;
   }
   
   private void Hide()
   {
      foreach (Transform child in transform)
      {
         child.gameObject.SetActive(false);
      }

      isShowing = false;
   }
}
