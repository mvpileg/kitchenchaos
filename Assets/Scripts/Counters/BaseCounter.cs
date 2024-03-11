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

    protected virtual bool CanAccept(KitchenObject kitchenObject) {
        return true;
    }

    protected virtual void ObjectWasAdded() {}

    protected virtual void ObjectWasRemoved() {}

    public virtual void Interact(Player player) {
        bool playerHasKitchenObject = player.KitchenObject != null;
        bool hasKitchenObject = KitchenObject != null;
        bool canAccept = CanAccept(player.KitchenObject);

        if (playerHasKitchenObject && !hasKitchenObject && canAccept) {
            // give to counter
            player.KitchenObject.SetKitchenObjectParent(this);
            ObjectWasAdded();
        } else if (hasKitchenObject && !playerHasKitchenObject) {
            // give to player
            KitchenObject.SetKitchenObjectParent(player);
            ObjectWasRemoved();
        } else if (playerHasKitchenObject && hasKitchenObject) {
            if (player.KitchenObject.TryGetPlate(out PlateKitchenObject playerPlate)) {
                // try to add counter object to player plate
                if (playerPlate.TryGiveKitchenObject(KitchenObject)) {
                    ObjectWasRemoved();
                }
            } else if (KitchenObject.TryGetPlate(out PlateKitchenObject counterPlate)) {
                // try to add player object to counter plate
                counterPlate.TryGiveKitchenObject(player.KitchenObject);
            }
        }
    }

    public virtual void InteractAlternate(Player player) {
        Debug.Log("InteractAlternate not implemented");
    }

}
