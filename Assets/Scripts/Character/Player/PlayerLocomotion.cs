using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    /// <summary>
    /// 玩家移动相关输入
    /// </summary>
    private Vector2 playerMove = Vector2.zero;
    private float moveAmount;
    private float horizontalMove;
    private float verticalMove;
    private MagicNumberSetting magicNumber = new MagicNumberSetting();
    private Vector3 targetPosition;
    private float moveSpeed = 3f;

    private Vector3 targetRotationVector;
    private Quaternion targetRotation;
    private float slerpFactor = 0.2f;
    private PlayerAnimatorManager animatorManager;

    private CharacterController characterController;

    private void Awake() {
        characterController = GetComponent<CharacterController>();
        animatorManager = GetComponent<PlayerAnimatorManager>();
    }

    private void HandlePlayerInput(){
        playerMove = PlayerInput.Singleton.GetPlayerMove();
        horizontalMove = playerMove.x;
        verticalMove = playerMove.y;

        //设置moveAmount
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalMove) + Mathf.Abs(verticalMove));
        if(moveAmount > magicNumber.lowerEps && moveAmount < magicNumber.upperEps){
            moveAmount = 0.5f;
        }
        else if(moveAmount > magicNumber.upperEps){
            moveAmount = 1f;
        }
        else {
            moveAmount = magicNumber.zero;
        }
        animatorManager.UpdateAnimations(0, moveAmount);
    }

    private void HandlePlayerMove(){
        targetPosition = (PlayerCamera.Singleton.transform.forward * verticalMove + PlayerCamera.Singleton.transform.right * horizontalMove).normalized;

        characterController.Move(targetPosition * moveSpeed * Time.fixedDeltaTime);
    }

    private void HandlePlayerRotate(){
        targetRotationVector = (PlayerCamera.Singleton.transform.forward * verticalMove +
            PlayerCamera.Singleton.transform.right * horizontalMove).normalized;
        
        if (targetRotationVector == Vector3.zero){
            targetRotationVector = transform.forward;
        }

        targetRotation = Quaternion.LookRotation(targetRotationVector);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, slerpFactor);
    }

    public void HandleAllPlayerMove(){
        HandlePlayerInput();
        HandlePlayerMove();
        HandlePlayerRotate();
    }

}
