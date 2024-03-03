using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter {

    [SerializeField] private KitchenObjectSO cutKitchenObjectSO;

    public override void Interact(Player player) {
        bool playerHasKitchenObject = player.KitchenObject != null;
        bool hasKitchenObject = KitchenObject != null;

        if (playerHasKitchenObject && !hasKitchenObject) {
            // give to counter
            player.KitchenObject?.SetKitchenObjectParent(this);
        } else if (hasKitchenObject && !playerHasKitchenObject) {
            // give to player
            KitchenObject?.SetKitchenObjectParent(player);
        }
    }

    public override void InteractAlternate(Player player) {
        if (KitchenObject != null) {
            KitchenObject.DestroySelf();
            KitchenObject.SpawnKitchenObject(cutKitchenObjectSO, this);
        }
    }

}
