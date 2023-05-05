using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlateKitchenObject : MonoBehaviour
{
    [SerializeField] private PlateCounter plateCounter;
    [SerializeField] private Transform spawnPlatePoint;
    [SerializeField] private GameObject platePrefab;

    public List<GameObject> platesGameObjectsList;

    private void Awake()
    {
        platesGameObjectsList = new List<GameObject>();
    }

    private void Start()
    {
        plateCounter.spawnPlateEvent += OnSpawnPlateEvent;
        plateCounter.removePlateEvent += OnRemovePlateEvent;
    }

    private void OnRemovePlateEvent(object sender, EventArgs e)
    {
        Transform plateTransform = platesGameObjectsList[^1].transform;
        platesGameObjectsList.Remove(platesGameObjectsList[^1]);
        Destroy(plateTransform.gameObject);
    }

    private void OnSpawnPlateEvent(object sender, EventArgs e)
    {
        GameObject plateGameObject = Instantiate(platePrefab, spawnPlatePoint);
        
        plateCounter.kitchenObject = platePrefab.GetComponent<KitchenObject>();
        
        float spawnPlateTransform = .02f;
        plateGameObject.transform.position = new Vector3(spawnPlatePoint.position.x, spawnPlatePoint.position.y + platesGameObjectsList.Count * spawnPlateTransform, spawnPlatePoint.position.z);
        platesGameObjectsList.Add(plateGameObject);
    }
}
