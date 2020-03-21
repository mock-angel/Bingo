using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class PhotonRoom : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    
    public void Start(){
        spawnPlayer();
    }
    
    public void spawnPlayer(){
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, playerPrefab.transform.position, playerPrefab.transform.rotation, 0);
        
        player.GetComponent<PhotonPlayerScript>().StartLocalPlayer();
        player.GetComponent<PhotonPlayerScript>().playerName = Player.instance.DisplayName;
    }
    
    public override void OnDisconnected(DisconnectCause cause){
        menuLogic.OnChangeToLobby();
        print("Disconnected from photon, must start over.");
    }
}
