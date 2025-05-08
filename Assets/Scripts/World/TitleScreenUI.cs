using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;


public class TitleScreenUI : NetworkBehaviour {
    private Button pressStartBtn;
    
    private int worldIndex = 1;

    public bool startAtClient = false;
    
    private void Awake() {
        pressStartBtn = GameObject.Find("Start Game Button").GetComponent<Button>();
    }

    private void Start() {
        pressStartBtn.onClick.AddListener(()=>{
            if(startAtClient){
                NetworkManager.Singleton.StartClient();
            }
            else {
                NetworkManager.Singleton.StartHost();
            }
            StartGame(worldIndex);
            Cursor.lockState = CursorLockMode.Locked;
        });
    }

    private IEnumerator StartNewGameCoroutine(int idx){
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(idx);
        yield return null;
    }    
    private void StartGame(int idx){
        StartCoroutine(StartNewGameCoroutine(idx));
    }
}
