using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

using Photon.Pun;
using Photon.Realtime;

public class photonSelectionButton : MonoBehaviourPunCallbacks
{
    public TMP_InputField createRoomInput, joinRoomInput;
    
    public GameObject JoinRoomView;
    public GameObject InRoomView;
    
    public void OnClickCreateRoom(){
        //Dont let player create room when name isnt set.
        if(Player.instance.DisplayName.Length < 1) return;

        if(createRoomInput.text.Length >= 1)
        PhotonNetwork.CreateRoom(createRoomInput.text, new RoomOptions(){MaxPlayers = 4}, null);
    }
    
    public void OnClickJoinRoom(){
        //Dont let player join when name isnt set.
        if(Player.instance.DisplayName.Length < 1) return;
        
        if(joinRoomInput.text.Length >= 1)
        PhotonNetwork.JoinRoom(joinRoomInput.text);
    }
    
    public override void OnJoinedRoom(){
        Debug.Log("We are connected to the room!");
        
        JoinRoomView.SetActive(false);
        InRoomView.SetActive(true);
//        menuLogic.OnChangeToRoom();
    }
    public override void OnLeftRoom(){
        Debug.Log("We left the room!");
        
        JoinRoomView.SetActive(true);
        InRoomView.SetActive(false);
//        menuLogic.OnChangeToRoom();
    }
    public void OnClickStartGame(){
        if(PhotonNetwork.IsMasterClient){
            PhotonNetwork.LoadLevel(1);
        }
    }
    
    public void OnClickLeaveRoom(){
        PhotonNetwork.LeaveRoom();
    }
}
