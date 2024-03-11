using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClearCounter : BaseCounter {

    public override void Interact(Player player) {
        bool playerHasKitchenObject = player.KitchenObject != null;
        bool hasKitchenObject = KitchenObject != null;

        if (playerHasKitchenObject && !hasKitchenObject) {
            // give to counter
            player.KitchenObject?.SetKitchenObjectParent(this);
        } else if (hasKitchenObject && !playerHasKitchenObject) {
            // give to player
            KitchenObject?.SetKitchenObjectParent(player);
        } else if (playerHasKitchenObject && hasKitchenObject) {
            if (player.KitchenObject.TryGetPlate(out PlateKitchenObject playerPlate)) {
                if (playerPlate.TryGiveKitchenObject(KitchenObject)) {
                    KitchenObject.DestroySelf();
                }
            } else if (KitchenObject.TryGetPlate(out PlateKitchenObject counterPlate)) {
                if (counterPlate.TryGiveKitchenObject(player.KitchenObject)) {
                    player.KitchenObject.DestroySelf();
                }   
            }
        }
    }

}