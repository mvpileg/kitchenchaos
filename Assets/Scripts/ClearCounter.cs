using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : MonoBehaviour, IKitchenObjectParent {

    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private Transform counterTopPoint;

    public KitchenObject KitchenObject { get; set; }
    public Transform HoldPosition {
        get {
            return counterTopPoint;
        }
    }

    public void Interact(Player player) {
        if (KitchenObject == null) {
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab, counterTopPoint);
            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(this);
        } else {
            // Give the object to the player
            KitchenObject.SetKitchenObjectParent(player);
        }

    }

}