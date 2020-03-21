﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ClientPlayerScript : NetworkBehaviour
{
    public static ClientPlayerScript scriptInstance;
    
//    public List<PlayerNetworkData> playerDataObj;
    [SyncVar]
    public string playerName;
    
    [SyncVar]
    public int selfId;
    
    [SyncVar]
    public bool ready;
    
    bool ObjectAuthority = false;
    
    [SyncVar]
    public bool isTurn = false;
    
    public bool gameStarted = false;
    
    [SyncVar]
    public bool gameWon = false;
    [SyncVar]
    public int winOrder = 0;
    
    public override void OnStartLocalPlayer(){
        base.OnStartLocalPlayer();
        ClientSetPlayerName(Player.instance.DisplayName);
        
        CmdChangeParent();
        
        scriptInstance = this;
        
        ObjectAuthority = true;
    }
    
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
    [Command]
    public void CmdAllClientsSetReady(bool newReady){
        ready = newReady;
    }
    [Command]
    public void CmdSetPlayerNameString(string name){
        playerName = name;
    }
    
    [Command]
    public void CmdAssignClientID(){
//        int nextId = ServerPlayerScript.instance.getNextID();
        selfId = ServerPlayerScript.instance.getNextID();
//        NetworkIdentity identity = GetComponent<NetworkIdentity>();
    }
    [Command]
    public void CmdChangeParent(){
        transform.parent = ConnectedPlayersStaticScript.instance.transform;
        
        //Instruct all clients to change parent as well.
        RpcChangeParent();
    }
    
    [Command]
    public void CmdTurnFinished(int numberSelected){
        RpcTurnFinished(numberSelected);
        ServerGameManagerScirpt.scriptInstance.TurnFinished(numberSelected);
    }
    
    [Command]
    public void CmdGameWon(){
        if(gameWon) return;
        
        gameWon = true;
        winOrder = ++ServerGameManagerScirpt.scriptInstance.winnersCount;
    }
    
    [ClientRpc]
    public void RpcTurnFinished(int numberSelected){
        PhotonGameManagerBingo.scriptInstance.TurnFinished(numberSelected);
    }
    
    [ClientRpc]
    public void RpcChangeParent(){
        GameObject[] PlayerObjects = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < PlayerObjects.Length; i++){//GameObject singlePlayer in PlayerObjects){
            PlayerObjects[i].transform.parent = ConnectedPlayersStaticScript.instance.transform;
        }
    }
    
    [ClientRpc]
    public void RpcStartGame(){
        gameStarted = true;
        if(ObjectAuthority == true){
            print("Authority start");
            PhotonGameManagerBingo.scriptInstance.RpcStartGame();
        }
    }
}
