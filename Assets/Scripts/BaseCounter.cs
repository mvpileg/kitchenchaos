using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCounter : MonoBehaviour, IKitchenObjectParent {

    [SerializeField] protected KitchenObjectSO kitchenObjectSO;
    [SerializeField] private Transform counterTopPoint;

    public KitchenObject KitchenObject { get; set; }
    public Transform HoldPosition {
        get {
            return counterTopPoint;
        }
    }

    public abstract void Interact(Player player);

}
