using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CuttingCounter : BaseCounter {
    
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;
    public class OnProgressChangedEventArgs : EventArgs {
        public float progressNormalized;
    }

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;

    public override void Interact(Player player) {
        bool playerHasKitchenObject = player.KitchenObject != null;
        bool hasKitchenObject = KitchenObject != null;

        if (playerHasKitchenObject && !hasKitchenObject) {
            // give to counter
            player.KitchenObject.SetKitchenObjectParent(this);
            ResetCuttingProgress();
        } else if (hasKitchenObject && !playerHasKitchenObject) {
            // give to player
            KitchenObject.SetKitchenObjectParent(player);
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
        bool isDoneCutting = CutIngredient(cuttingRecipeSO);

        if (isDoneCutting) {
            ResetCuttingProgress();
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

    private void ResetCuttingProgress() {
        cuttingProgress = 0;
        OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs {
            progressNormalized = 0f
        });
    }

    private bool CutIngredient(CuttingRecipeSO cuttingRecipeSO) {
        cuttingProgress++;
        OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs {
            progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
        });
        return cuttingProgress >= cuttingRecipeSO.cuttingProgressMax;
    }

}
