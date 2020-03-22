using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using MyAttributes;
using TMPro;

public class photonPlayerListing : MonoBehaviour
{
    List<string> playerListings;
    
    public bool UpdateOnText;
    [ConditionalField("UpdateOnText")]
    public TextMeshProUGUI listingsText;
    
    void Start(){
        playerListings = new List<string>();
    }
    
    public void GetCurrentRoomPlayers(){
        playerListings.Clear();
        
        if(!PhotonNetwork.IsConnected)  return;
        if(PhotonNetwork.CurrentRoom == null)  return;
        if(PhotonNetwork.CurrentRoom.Players == null)  return;
        
//        foreach(KeyValuePair<int, Photon.Realtime.Player> playerInfo in PhotonNetwork.CurrentRoom.Players){
//            playerListings.Add(playerInfo.Value.NickName);
//            print("found some player here");
//        }

//        for(int i = 0; i < PhotonNetwork.playerList.Count; i++){
//            playerListings.Add(PhotonNetwork.playerList[i].name);
//        }

         foreach(Photon.Realtime.Player pl in PhotonNetwork.PlayerList)
         {
            print(pl.NickName);
             playerListings.Add(pl.NickName);
         }
    }
    
    public void FixedUpdate(){
        GetCurrentRoomPlayers();
        
        listingsText.text = "";
        if(UpdateOnText) 
            for(int i = 0; i < playerListings.Count; i++)
                listingsText.text += playerListings[i] + "\n";
    }
}
