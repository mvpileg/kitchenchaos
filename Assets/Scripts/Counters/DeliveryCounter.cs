using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter {

    public override void Interact(Player player) {
        KitchenObject kitchenObject = player.KitchenObject;
        if (kitchenObject != null && kitchenObject.TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
            plateKitchenObject.DestroySelf();
        }
    }

}
