using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatesCounter : BaseCounter {

    public event EventHandler<OnPlateCountChangedEventArgs> OnPlateCountChanged;

    public class OnPlateCountChangedEventArgs: EventArgs {

        public enum Action {
            add,
            remove
        }
        public int count;
        public Action action;

    }

    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private int platesSpawnedCount;
    private int platesSpawnedMaxCount = 4;

    private void Update() {
        if (platesSpawnedCount >= platesSpawnedMaxCount) {
            return;
        }

        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer > spawnPlateTimerMax) {
            platesSpawnedCount++;
            spawnPlateTimer = 0f;

            OnPlateCountChanged?.Invoke(this, new OnPlateCountChangedEventArgs {
                count = platesSpawnedCount,
                action = OnPlateCountChangedEventArgs.Action.add
            });
        }

    }

    public override void Interact(Player player) {
        base.Interact(player);
        
        if (player.KitchenObject != null || platesSpawnedCount == 0) {
            return;
        }
        platesSpawnedCount--;
        OnPlateCountChanged?.Invoke(this, new OnPlateCountChangedEventArgs {
            count = platesSpawnedCount,
            action = OnPlateCountChangedEventArgs.Action.remove
        });

        KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
    }

    protected override bool CanAccept(KitchenObject kitchenObject) {
        return false;
    }

}
