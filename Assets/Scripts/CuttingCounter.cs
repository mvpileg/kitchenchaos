using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CuttingCounter : BaseCounter {
    
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;
    public class OnProgressChangedEventArgs : EventArgs {
        public bool canCut = true;
        public float progressNormalized = 0f;
    }

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;

    public override void Interact(Player player) {
        bool playerHasKitchenObject = player.KitchenObject != null;
        bool hasKitchenObject = KitchenObject != null;

        if (playerHasKitchenObject && !hasKitchenObject) {
            // give to counter
            player.KitchenObject.SetKitchenObjectParent(this);
            ObjectAdded();
        } else if (hasKitchenObject && !playerHasKitchenObject) {
            // give to player
            KitchenObject.SetKitchenObjectParent(player);
            ObjectRemoved();
        }
    }

    public override void InteractAlternate(Player player) {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(KitchenObject);
        if (cuttingRecipeSO == null) {
            return;
        }

        bool isDoneCutting = CutIngredient(cuttingRecipeSO);

        if (isDoneCutting) {
            KitchenObject.DestroySelf();
            KitchenObject.SpawnKitchenObject(cuttingRecipeSO.output, this);
            ObjectAdded();
        }
    }

    private bool CanCut(KitchenObject kitchenObject) {
        return GetCuttingRecipeSOWithInput(kitchenObject) != null;
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObject inputKitchenObject) {
        if (inputKitchenObject == null) {
            return null;
        }
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
            if (cuttingRecipeSO.input == inputKitchenObject.GetKitchenObjectSO()) {
                return cuttingRecipeSO;
            }
        }
        return null;
    }

    private bool CutIngredient(CuttingRecipeSO cuttingRecipeSO) {
        cuttingProgress++;
        OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs {
            progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
        });
        return cuttingProgress >= cuttingRecipeSO.cuttingProgressMax;
    }

    private void ObjectAdded() {
        if (CanCut(KitchenObject)) {
            cuttingProgress = 0;
            OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs {
                progressNormalized = 0f
            });
        } else {
            OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs {
                canCut = false
            });
        }
    }

    private void ObjectRemoved() {
        OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs {
            canCut = false
        });
    }

}
