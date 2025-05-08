using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerManager : NetworkBehaviour {

    private PlayerLocomotion playerLocomotion;
    private void Awake(){
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }
    private void Start() {
        DontDestroyOnLoad(gameObject);
        if(IsOwner){
            PlayerCamera.Singleton.player = this;
        }
    }

    private void Update() {
        playerLocomotion.HandleAllPlayerMove();
    }

    private void LateUpdate() {
        PlayerCamera.Singleton.HandleAllCameraMovement();
    }
}
