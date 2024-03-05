using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter {

    [SerializeField] protected KitchenObjectSO kitchenObjectSO;

    public event EventHandler OnPlayerGrabbedObject;

    public override void Interact(Player player) {
        if (player.KitchenObject != null) {
            return;
        }
        KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
        OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
    }

}
