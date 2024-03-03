using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{

    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParent kitchenObjectParent;

    public KitchenObjectSO GetKitchenObjectSO() {
        return kitchenObjectSO;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent) {
        this.kitchenObjectParent?.ClearKitchenObject();
        this.kitchenObjectParent = kitchenObjectParent;

        kitchenObjectParent.KitchenObject = this;
        transform.parent = kitchenObjectParent.HoldPosition;
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectParent GetClearCounter() {
        return kitchenObjectParent;
    }

    public void DestroySelf() {
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

}
