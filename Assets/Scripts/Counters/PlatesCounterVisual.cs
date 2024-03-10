using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour {

    [SerializeField] private PlatesCounter platesCounter;

    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;

    private List<GameObject> plateVisualGameObjects;

    private void Awake() {
        plateVisualGameObjects = new List<GameObject>();
    }

    private void Start() {
        platesCounter.OnPlateCountChanged += PlatesCounter_OnPlateCountChanged;
    }

    private void PlatesCounter_OnPlateCountChanged(object sender, PlatesCounter.OnPlateCountChangedEventArgs e) {
        if (!Validate(e)) {
            Debug.LogError("Mis match detected between visual and stored plate count");
            UpdatePlateVisualsFallback(e);
            return;
        }
        switch (e.action) {
            case PlatesCounter.OnPlateCountChangedEventArgs.Action.add:
                AddPlate();
                break;
            case PlatesCounter.OnPlateCountChangedEventArgs.Action.remove:
                RemovePlate();
                break;
        }     
    }

    private void AddPlate() {
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);
        float plateOffsetY = 0.1f;
        plateVisualTransform.localPosition = new Vector3(0, plateOffsetY * plateVisualGameObjects.Count, 0);
        plateVisualGameObjects.Add(plateVisualTransform.gameObject);
    }

    private void RemovePlate() {
        GameObject plateVisual = plateVisualGameObjects[plateVisualGameObjects.Count - 1];
        plateVisualGameObjects.Remove(plateVisual);
        Destroy(plateVisual);
    }

    private bool Validate(PlatesCounter.OnPlateCountChangedEventArgs e) {
        switch (e.action) {
            case PlatesCounter.OnPlateCountChangedEventArgs.Action.add:
                return plateVisualGameObjects.Count + 1 == e.count;
            case PlatesCounter.OnPlateCountChangedEventArgs.Action.remove:
                return plateVisualGameObjects.Count - 1 == e.count;            
        }
        return false;
    }

    private void UpdatePlateVisualsFallback(PlatesCounter.OnPlateCountChangedEventArgs e) {
        foreach(GameObject plateVisualGameObject in plateVisualGameObjects) {
            Destroy(plateVisualGameObject);
        }
        plateVisualGameObjects = new List<GameObject>();
        for(int i = 0; i < e.count; i++) {
            AddPlate();
        }
    }

}
