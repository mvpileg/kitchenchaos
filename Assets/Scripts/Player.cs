using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotateSpeed = 20f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    
    private float playerRadius = 0.7f;
    private float playerHeight = 2f;

    private bool isWalking;

    private Vector3 forwardDir;

    private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void Update() {
        Vector2 inputDirVectorRaw = gameInput.GetDirectionVectorNormalized();
        Vector3 inputDirVector = new Vector3(inputDirVectorRaw.x, 0f, inputDirVectorRaw.y);

        if (inputDirVector != Vector3.zero) {
            forwardDir = inputDirVector;
        }

        HandleMovement(inputDirVector);
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e) {

        // Check for interactable objects
        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, forwardDir, out RaycastHit raycastHit, interactDistance, countersLayerMask)) {
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter)) {
                clearCounter.Interact();
            }
        } else {
            Debug.Log("-");
        }
    }

    private void HandleMovement(Vector3 moveDir) {
     
        // Face direction of input
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);

        // Attempt original input movement
        bool didMove = Move(moveDir) ;

        if (!didMove) {
            // Attempt only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f);
            didMove = Move(moveDirX);
        }

        if (!didMove) {
            // Attempt only Z movement
            Vector3 moveDirZ = new Vector3(0f, 0f, moveDir.z);
            didMove = Move(moveDirZ);
        }
    
        isWalking = didMove;
    }

    private bool Move(Vector3 direction) {
        if (direction == Vector3.zero) {
            return false;
        }

        float moveDistance = moveSpeed * Time.deltaTime;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, direction, moveDistance);

        if (canMove) {
            transform.position += direction * moveDistance;
        }

        return canMove;
    }

    public bool IsWalking() {
        return isWalking;
    }

}
