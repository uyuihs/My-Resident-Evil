using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
    public static PlayerInput Singleton;

    private InputComtroller inputController;
    private Vector2 playerMove;
    private Vector2 cameraMove;

    private void Awake() {
        if(Singleton == null){
            Singleton = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable() {
        if(inputController == null){
            inputController = new InputComtroller();
            inputController.PlayerMove.Move.performed += i => playerMove = i.ReadValue<Vector2>();
            inputController.CameraMove.Move.performed += i => cameraMove = i.ReadValue<Vector2>();
        }
        inputController.Enable();
    }

    public Vector2 GetPlayerMove(){
        return playerMove;
    }

    public Vector2 GetCameraMove(){
        return cameraMove;
    }
}
