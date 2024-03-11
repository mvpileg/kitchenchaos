using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress {

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

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
                ResetCookingTime();
                break;
            case State.Frying:
                IncrementCookingTime(Time.deltaTime);
                if (cookingTime > fryingRecipeSO.fryingTimerMax) {
                    SetState(State.Fried);
                    KitchenObject.DestroySelf();
                    KitchenObject = KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                }
                break;
            case State.Fried:
                IncrementCookingTime(Time.deltaTime);
                if (cookingTime > fryingRecipeSO.burntTimerMax) {
                    SetState(State.Burnt);
                    KitchenObject.DestroySelf();
                    KitchenObject = KitchenObject.SpawnKitchenObject(fryingRecipeSO.burntOutput, this);
                }
                break;
            case State.Burnt:
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
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                shouldShowProgress = false
            });
        } else if (playerHasKitchenObject && hasKitchenObject) {
            if (player.KitchenObject.TryGetPlate(out PlateKitchenObject playerPlate)) {
                if (playerPlate.TryGiveKitchenObject(KitchenObject)) {
                    KitchenObject.DestroySelf();
                    SetState(State.Idle);
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        shouldShowProgress = false
                    });
                }
            } else if (KitchenObject.TryGetPlate(out PlateKitchenObject counterPlate)) {
                if (counterPlate.TryGiveKitchenObject(player.KitchenObject)) {
                    player.KitchenObject.DestroySelf();
                }   
            }
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

    private void ResetCookingTime() {
        cookingTime = 0f;
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
            progressNormalized = 0f
        });
    }

    private void IncrementCookingTime(float time) {
        cookingTime += time;

        IHasProgress.State progressBarState;
        float progressNormalized;
        bool shouldShowProgress;

        if (cookingTime < fryingRecipeSO.fryingTimerMax) {
            progressNormalized = cookingTime / fryingRecipeSO.fryingTimerMax;
            progressBarState = IHasProgress.State.Normal;
            shouldShowProgress = true;
        } else if (cookingTime < fryingRecipeSO.burntTimerMax) {
            float timeToBurnFromCooked = fryingRecipeSO.burntTimerMax - fryingRecipeSO.fryingTimerMax;
            float timeSinceCooked = cookingTime - fryingRecipeSO.fryingTimerMax;
            progressNormalized = timeSinceCooked / timeToBurnFromCooked;
            progressBarState = IHasProgress.State.Warning;
            shouldShowProgress = true;
        } else {
            progressNormalized = 0f;
            progressBarState = IHasProgress.State.Normal;
            shouldShowProgress = false;
        }

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
            progressNormalized = progressNormalized,
            state = progressBarState,
            shouldShowProgress = shouldShowProgress
        });
    }

}
