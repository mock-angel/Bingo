using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class photonConnect : MonoBehaviourPunCallbacks
{
    public GameObject connectingView;
    public GameObject connectedView;
    public GameObject disconnectedView;
    
    void Awake(){
        PhotonNetwork.ConnectUsingSettings();
        
        Debug.Log("Connecting To photon");
    }
    
    public override void OnConnectedToMaster(){
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        
        Debug.Log("We are connected to master");
    }
    
    public override void OnJoinedLobby(){
        
        if(connectedView != null) connectedView.SetActive(true);
        if(connectingView != null) connectingView.SetActive(false);
        Debug.Log("On joined Lobby");
    }
    
    public override void OnDisconnected(DisconnectCause cause){
        if(connectingView.activeSelf)
            connectingView.SetActive(false);
        
        if(connectedView.activeSelf)
            connectedView.SetActive(false);
        
        disconnectedView.SetActive(true);
        Debug.Log("Disconnected from photon services");
    }
}
