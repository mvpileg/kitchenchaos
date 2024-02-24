using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotateSpeed = 20f;
    [SerializeField] private GameInput gameInput;
    
    private float playerRadius = 0.7f;
    private float playerHeight = 2f;

    private bool isWalking;

    private void Update() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

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
