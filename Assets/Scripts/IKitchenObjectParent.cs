using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public interface IKitchenObjectParent {

    KitchenObject KitchenObject { get; set; }
    Transform HoldPosition { get; }

    public bool HasKitchenObject() {
        return KitchenObject != null;
    }

    public void ClearKitchenObject() {
        KitchenObject = null;
    }

}
