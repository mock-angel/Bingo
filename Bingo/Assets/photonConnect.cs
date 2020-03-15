using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class photonConnect : MonoBehaviour
{
    public string versionname = "0.1";
    
    
    public GameObject player;
    
    public GameObject connected, disconnected, connect;
    public void connectToPhoton(){
        PhotonNetwork.ConnectUsingSettings(versionname);
        
        Debug.Log("Connecting To photon");
    }
    
    private void OnConnectedToMaster(){
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        
        connect.SetActive(false);
        connected.SetActive(true);
        Debug.Log("Connected to lobby");
    }
    
    private void OnJoinedLobby(){
        Debug.Log("On joined Lobby");
    }
    
    private OnDisconnectedFromPhoton(){
    
        connected.SetActive(true);
        Debug.Log("Disconnected from photon services");
    }
}
