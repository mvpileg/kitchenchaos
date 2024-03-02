using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public interface IKitchenObjectParent {

    // public void SetKitchenObject(KitchenObject kitchenObject);

    // public KitchenObject GetKitchenObject();

    public Transform GetKitchenObjectFollowTransform();

    KitchenObject KitchenObject { get; set; }
    // Transform LocalPosition { get; }

    public bool HasKitchenObject() {
        return KitchenObject != null;
    }

    public void ClearKitchenObject() {
        KitchenObject = null;
    }

    // public Transform GetKitchenObjectFollowTransform() {
    //     return LocalPosition;
    // }


}
