using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter {
    
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    public override void Interact(Player player) {
        bool playerHasKitchenObject = player.KitchenObject != null;
        bool hasKitchenObject = KitchenObject != null;

        if (playerHasKitchenObject && !hasKitchenObject) {
            if (HasRecipeForInput(player.KitchenObject.GetKitchenObjectSO())) {
                // give to counter
                player.KitchenObject?.SetKitchenObjectParent(this);
            }
        } else if (hasKitchenObject && !playerHasKitchenObject) {
            // give to player
            KitchenObject?.SetKitchenObjectParent(player);
        }
    }

    public override void InteractAlternate(Player player) {
        if (KitchenObject == null) {
            return;
        }
        KitchenObjectSO newKitchenObjectSO = GetOutputForInput(KitchenObject.GetKitchenObjectSO());
        if (newKitchenObjectSO) {
            KitchenObject.DestroySelf();
            KitchenObject.SpawnKitchenObject(newKitchenObjectSO, this);
        }
    }

    private bool HasRecipeForInput(KitchenObjectSO kitchenObjectSO) {
        return GetOutputForInput(kitchenObjectSO) != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
            if (cuttingRecipeSO.input == inputKitchenObjectSO) {
                return cuttingRecipeSO.output;
            }
        }
        return null;
    }

}
