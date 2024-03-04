using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CuttingCounter : BaseCounter {
    
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;

    public override void Interact(Player player) {
        bool playerHasKitchenObject = player.KitchenObject != null;
        bool hasKitchenObject = KitchenObject != null;

        if (playerHasKitchenObject && !hasKitchenObject) {
            // give to counter
            player.KitchenObject?.SetKitchenObjectParent(this);
            cuttingProgress = 0;
        } else if (hasKitchenObject && !playerHasKitchenObject) {
            // give to player
            KitchenObject?.SetKitchenObjectParent(player);
        }
    }

    public override void InteractAlternate(Player player) {
        if (KitchenObject == null) {
            return;
        }
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(KitchenObject.GetKitchenObjectSO());
        if (cuttingRecipeSO == null) {
            return;
        }
        KitchenObjectSO newKitchenObjectSO = cuttingRecipeSO.output;
        if (newKitchenObjectSO == null) {
            return;
        }

        cuttingProgress++;

        if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax) {
            KitchenObject.DestroySelf();
            KitchenObject.SpawnKitchenObject(newKitchenObjectSO, this);
        }
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
            if (cuttingRecipeSO.input == inputKitchenObjectSO) {
                return cuttingRecipeSO;
            }
        }
        return null;
    }

}
