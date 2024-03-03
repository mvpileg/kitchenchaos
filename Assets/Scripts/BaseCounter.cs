using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCounter : MonoBehaviour, IKitchenObjectParent {

    public KitchenObject KitchenObject { get; set; }

    [SerializeField] private Transform counterTopPoint;

    public Transform HoldPosition {
        get {
            return counterTopPoint;
        }
    }

    public abstract void Interact(Player player);

}
