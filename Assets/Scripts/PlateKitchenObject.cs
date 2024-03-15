using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlateKitchenObject : KitchenObject {

    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;

    public class OnIngredientAddedEventArgs: EventArgs {
        public KitchenObjectSO kitchenObjectSO;
    }

    [SerializeField] private List<KitchenObjectSO> allowableKitchenObjectSOs;

    private List<KitchenObjectSO> kitchenObjectSOs;

    private void Awake() {
        kitchenObjectSOs = new List<KitchenObjectSO>();
    }

    public bool TryGiveKitchenObject(KitchenObject kitchenObject) {
        KitchenObjectSO kitchenObjectSO = kitchenObject.GetKitchenObjectSO();
        if (!allowableKitchenObjectSOs.Contains(kitchenObjectSO)) {
            Debug.Log("This kitchenObjectSO is not allowed");
            return false;
        }
        if (kitchenObjectSOs.Contains(kitchenObjectSO)) {
            Debug.Log("Already have this kitchenObjectSO");
            return false;
        } else {
            kitchenObject.DestroySelf();
            kitchenObjectSOs.Add(kitchenObjectSO);
            Debug.Log("Adding kitchenObjectSO");
;
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs {
                kitchenObjectSO = kitchenObjectSO
            });
            return true;
        }
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList() {
        return kitchenObjectSOs;
    }

}
