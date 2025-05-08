using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {
    public static PlayerCamera Singleton;
    
    /// <summary>
    /// 处理相机跟随
    /// </summary>
    public PlayerManager player;
    private Vector3 followVelocity;
    private float smoothFactor = 0.2f;
    
    /// <summary>
    ///处理相机旋转 
    /// </summary>
    private Vector2 cameraInput;
    private float horizontalInput;
    private float verticalInput;
    private float horizontalAmount;
    private float verticalAmount;
    private float limitRotate = 30f;
    private Vector3 rotationNormal;//旋转的法向量
    private Quaternion targetRotation;//法向量转换的四元数
    private float rotateSpedd = 5;
    [SerializeField]private Transform cameraPivot;//上下旋转

    /// <summary>
    /// 处理相机碰撞
    /// </summary>
    private Camera cameraObject;
    private Vector3 hitDetectVector;//碰撞检测方向
    private float collideRadius;//球形碰撞检测半径
    [SerializeField] private LayerMask mask;
    private float cameraBaseZPosition;
    private float cameraTargetZPosition;
    

    private void Awake() {
        if (Singleton == null){
            Singleton = this;
        }
        else {
            Destroy(gameObject);
        }
        cameraObject = GetComponentInChildren<Camera>();
    }

    private void Start() {
        DontDestroyOnLoad(gameObject);
        cameraBaseZPosition = cameraObject.transform.position.z;
    }

    public void HandleAllCameraMovement(){
        //1.跟随玩家
        //2.相机旋转
        //3.相机碰撞
        FollowerPlayer();
        RotateCamera();
        CameraCollide();
    }

    private void FollowerPlayer(){
        if(!player) return;
        transform.position = Vector3.SmoothDamp(transform.position, player.transform.position, ref followVelocity,smoothFactor);
    }

    private void HandleCameraInput(){
        cameraInput = PlayerInput.Singleton.GetCameraMove();
        horizontalInput = cameraInput.x;
        verticalInput = cameraInput.y;
    }

    private void RotateCamera(){
        HandleCameraInput();
        horizontalAmount += horizontalInput * rotateSpedd * Time.deltaTime;
        verticalAmount -= verticalInput * rotateSpedd * Time.deltaTime;//屏幕向下为正向

        //绕y轴旋转
        rotationNormal = new Vector3(0, horizontalAmount, 0);
        targetRotation = Quaternion.Euler(rotationNormal);
        transform.rotation = targetRotation;

        //绕x轴旋转
        verticalAmount = Mathf.Clamp(verticalAmount, -limitRotate, limitRotate);
        rotationNormal = new Vector3(verticalAmount, 0, 0);
        targetRotation = Quaternion.Euler(rotationNormal);
        cameraPivot.localRotation = targetRotation;

    }

    /// <summary>
    ///当相机碰撞到物体时，将相机往Z轴正方向推 
    /// </summary>
    private void CameraCollide(){
        cameraTargetZPosition = cameraBaseZPosition;
        //碰撞对象
        RaycastHit hit;
        //碰撞射线的方向向量：从相机pivot指向相机object
        hitDetectVector = (cameraObject.transform.position - cameraPivot.position).normalized;
        
        //以player为球心，半径cameraBaseZPosition为半径，以hitDetectVector为方向向量
        if(Physics.SphereCast(cameraPivot.position, collideRadius, hitDetectVector, out hit, Mathf.Abs(cameraTargetZPosition), mask)){
            //dist为击中点到相机pivot的距离
            float dist = Vector3.Distance(hit.point, cameraPivot.position);
            //减去碰撞半径，得出值为距离
            dist = -(dist - collideRadius);
            cameraTargetZPosition = dist;
        }
        //camera和玩家的距离小于collideridus时，相机的位置为-collideradius
        if(Mathf.Abs(cameraTargetZPosition) < collideRadius){
            cameraTargetZPosition = -collideRadius;
        }
        cameraObject.transform.localPosition = new Vector3(0, 0,Mathf.Lerp(cameraObject.transform.localPosition.z, cameraTargetZPosition, smoothFactor));

    }
}
