using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter {

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs {
        public State state;
    }

    public enum State {
        Idle,
        Frying,
        Fried,
        Burnt
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;

    private State state;
    private float cookingTime;
    private FryingRecipeSO fryingRecipeSO;

    private void Start() {
        state = State.Idle;
    }

    private void Update() {
        if (KitchenObject == null) {
            return;
        }
        switch (state) {
            case State.Idle:
                SetState(State.Frying);
                fryingRecipeSO = GetFryingRecipeSOWithInput(KitchenObject);
                cookingTime = 0f;
                break;
            case State.Frying:
                cookingTime += Time.deltaTime;
                if (cookingTime > fryingRecipeSO.fryingTimerMax) {
                    SetState(State.Fried);
                    KitchenObject.DestroySelf();
                    KitchenObject = KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                }
                break;
            case State.Fried:
                cookingTime += Time.deltaTime;
                if (cookingTime > fryingRecipeSO.burntTimerMax) {
                    SetState(State.Burnt);
                    KitchenObject.DestroySelf();
                    KitchenObject = KitchenObject.SpawnKitchenObject(fryingRecipeSO.burntOutput, this);
                }
                break;
            case State.Burnt:
                cookingTime += Time.deltaTime;
                break;      
        }
    }

    public override void Interact(Player player) {
        bool playerHasKitchenObject = player.KitchenObject != null;
        bool hasKitchenObject = KitchenObject != null;

        if (playerHasKitchenObject && !hasKitchenObject && CanFry(player.KitchenObject)) {
            // give to stove
            player.KitchenObject.SetKitchenObjectParent(this);
        } else if (hasKitchenObject && !playerHasKitchenObject) {
            // give to player
            KitchenObject.SetKitchenObjectParent(player);
            SetState(State.Idle);
        }
    }

    private bool CanFry(KitchenObject kitchenObject) {
        return GetFryingRecipeSOWithInput(kitchenObject) != null;
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObject inputKitchenObject) {
        if (inputKitchenObject == null) {
            return null;
        }
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray) {
            if (fryingRecipeSO.input == inputKitchenObject.GetKitchenObjectSO()) {
                return fryingRecipeSO;
            }
        }
        return null;
    }

    private void SetState(State state) {
        this.state = state;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
            state = state
        });
    }

}
