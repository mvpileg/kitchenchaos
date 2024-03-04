using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour {

    private enum Mode {
        LookAt,
        LookAway
    }

    [SerializeField] private Mode mode;

    private void LateUpdate() {
        // 5:13:24
        switch (mode) {
            case Mode.LookAt: 
                transform.forward = Camera.main.transform.forward;
                break;
            case Mode.LookAway:
                transform.forward = -Camera.main.transform.forward;
                break;
        }
    }

}
