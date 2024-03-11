using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlateKitchenObject : KitchenObject {

    [SerializeField] private List<KitchenObjectSO> allowableKitchenObjectSOs;

    private List<KitchenObjectSO> kitchenObjectSOs;

    private void Awake() {
        kitchenObjectSOs = new List<KitchenObjectSO>();
    }

    public bool TryGiveKitchenObject(KitchenObject kitchenObject) {
        KitchenObjectSO kitchenObjectSO = kitchenObject.GetKitchenObjectSO();
        if (!allowableKitchenObjectSOs.Contains(kitchenObjectSO)) {
            return false;
        }
        if (kitchenObjectSOs.Contains(kitchenObjectSO)) {
            return false;
        } else {
            kitchenObjectSOs.Add(kitchenObjectSO);
            return true;
        }
    }

}
