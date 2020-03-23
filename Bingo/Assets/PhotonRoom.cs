using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class PhotonRoom : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    public GameObject gamePlayPrefab;
    
    public void Start(){
        spawnPlayer();
    }
    
    void Update(){
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }
    
    public void spawnPlayer(){
        if(PhotonNetwork.IsMasterClient) PhotonNetwork.Instantiate(gamePlayPrefab.name, gamePlayPrefab.transform.position, gamePlayPrefab.transform.rotation, 0);
        
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, playerPrefab.transform.position, playerPrefab.transform.rotation, 0);
        
        player.GetComponent<PhotonPlayerScript>().StartLocalPlayer();
        player.GetComponent<PhotonPlayerScript>().playerName = Player.instance.DisplayName;
//        player.GetComponent<PhotonPlayerScript>().ChangeParent();
    }
    
    public void OnClickLeaveRoom(){
        PhotonNetwork.LeaveRoom();
    }
    
    public override void OnLeftRoom(){
        menuLogic.OnChangeToLobby();
    }
    
    public override void OnDisconnected(DisconnectCause cause){
        menuLogic.OnChangeToLobby();
    }
}
