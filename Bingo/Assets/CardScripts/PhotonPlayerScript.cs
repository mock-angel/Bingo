﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(PhotonView))]
public class PhotonPlayerScript : MonoBehaviourPunCallbacks, IPunObservable
{
    public bool devTesting = false;
    
    public static PhotonPlayerScript scriptInstance;
//    public List<PlayerNetworkData> playerDataObj;
//    [SyncVar]
    public string playerName = "Unnamed";
    
//    [SyncVar]
    public int selfId;
    
//    [SyncVar]
    public bool ready;
    
    public bool ObjectAuthority = false;
    
//    [SyncVar]
    public bool isTurn = false;
    
//    [SyncVar]
    public bool gameWon = false;
    
    public bool isServer = false;
//    [SyncVar]
    public int winOrder = 0;
    
    private bool transformed = false;
    
//    public override void OnStartLocalPlayer(){
//        base.OnStartLocalPlayer();
//        ClientSetPlayerName(Player.instance.DisplayName);
//        
//        CmdChangeParent();
//        
//        scriptInstance = this;
//        
//        ObjectAuthority = true;
//    }
    
//    public void Start(){
//        ChangeParent();
//    }
    
    public void ChangeParent(){
        print("trying to set player");
        if(gameObject != null)
            if(gameObject.transform != null) {
                gameObject.transform.parent = ConnectedPlayersStaticScript.instance.transform;
                transformed = true;
                print("Player Set");
            }
    }
    
    public void StartLocalPlayer(){
//        base.StartLocalPlayer();
//        ClientSetPlayerName(Player.instance.DisplayName);
        
//        CmdChangeParent();
        scriptInstance = this;
        print("script instance assigned");
        ObjectAuthority = true;
        
        
    }
    
    void Update(){
        
        if(!transformed) ChangeParent();
        
//        if(!gameStarted){
//            //Check if game started for any other player.
//            if(PhotonGameManagerBingo.scriptInstance.)
//        } 
        
        if (photonView.IsMine || devTesting) {
            
            return;
            
        }
        else{
        
        }
    
    }
                         
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
    
        if(stream.IsWriting){
            stream.SendNext(playerName);
            stream.SendNext(ready);
            stream.SendNext(isTurn);
//            stream.SendNext(gameStarted);
        }else{
            playerName = (string) stream.ReceiveNext();
            ready = (bool) stream.ReceiveNext();
            isTurn = (bool) stream.ReceiveNext();
//            gameStarted = (bool) stream.ReceiveNext();
        }
    }
    
    //    [Command]
    public void CmdTurnFinished(int numberSelected){
        RpcTurnFinished(numberSelected);
        ServerGameManagerScirpt.scriptInstance.TurnFinished(numberSelected);
    }
    
    
    //When start game.
//    public void CmdStartGame(){
//        PhotonView photonView = PhotonView.Get(this);
//        gameStarted = true;
//        photonView.RPC("RpcStartGame", RpcTarget.All);
//    }
//    
//    [PunRPC]
//    public void RpcStartGame(){
////        gameStarted = true;
////        if(ObjectAuthority == true){
////            print("Authority start");
////            PhotonGameManagerBingo.scriptInstance.RpcStartGame();
////        }
//    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    //script on client side.
    public void SetClientID(){
        CmdAssignClientID();
    }
    
    public void ClientSetPlayerName(string name){
        CmdSetPlayerNameString(name);
    }
    
    public void ClientOnReady(bool readyVar){
        ready = readyVar;
        CmdAllClientsSetReady(readyVar);
    }
    
    //server side.
//    [Command]
    public void CmdAllClientsSetReady(bool newReady){
        ready = newReady;
    }
//    [Command]
    public void CmdSetPlayerNameString(string name){
        playerName = name;
    }
    
//    [Command]
    public void CmdAssignClientID(){
//        int nextId = ServerPlayerScript.instance.getNextID();
        selfId = ServerPlayerScript.instance.getNextID();
//        NetworkIdentity identity = GetComponent<NetworkIdentity>();
    }
//    [Command]
    public void CmdChangeParent(){
        transform.parent = ConnectedPlayersStaticScript.instance.transform;
        
        //Instruct all clients to change parent as well.
        RpcChangeParent();
    }
    
//    [Command]
    public void CmdGameWon(){
        if(gameWon) return;
        
        gameWon = true;
        winOrder = ++ServerGameManagerScirpt.scriptInstance.winnersCount;
    }
    
//    [ClientRpc]
    public void RpcTurnFinished(int numberSelected){
        PhotonGameManagerBingo.scriptInstance.TurnFinished(numberSelected);
    }
    
//    [ClientRpc]
    public void RpcChangeParent(){
        GameObject[] PlayerObjects = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < PlayerObjects.Length; i++){//GameObject singlePlayer in PlayerObjects){
            PlayerObjects[i].transform.parent = ConnectedPlayersStaticScript.instance.transform;
        }
    }
}
