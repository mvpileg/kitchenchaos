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
        }
    }

}