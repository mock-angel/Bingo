using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuLogic : MonoBehaviour
{
    public static void OnChangeToRoom(){
        SceneManager.LoadScene("PhotonRoomLoaded");
    }
    
    public static void OnChangeToLobby(){
        SceneManager.LoadScene("PhotonScene");
    }
}
