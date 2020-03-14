using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ClientPlayerScript : NetworkBehaviour
{
//    public List<PlayerNetworkData> playerDataObj;
    public string playerName;
    public int selfId;
    
    public override void OnStartLocalPlayer(){
        base.OnStartLocalPlayer();
        ClientSetPlayerName(Player.instance.DisplayName);
        
        CmdChangeParent();
    }
    
    //script on client side.
    public void SetClientID(){
        CmdAssignClientID();
    }
    
    public void ClientSetPlayerName(string name){
        CmdSetPlayerNameString(name);
    }
    
    //server side.
    [Command]
    public void CmdSetPlayerNameString(string name){
        playerName = name;
    }
    
    [Command]
    public void CmdAssignClientID(){
//        int nextId = ServerPlayerScript.instance.getNextID();
        selfId = ServerPlayerScript.instance.getNextID();
        
        NetworkIdentity identity = GetComponent<NetworkIdentity>();
        RpcAssignClientID(selfId);
    }
    [Command]
    public void CmdChangeParent(){
        transform.parent = ConnectedPlayersStaticScript.instance.transform;
        
//        GameObject[] PlayerObjects = GameObject.FindGameObjectsWithTag("Player");
//        foreach (GameObject singlePlayer in PlayerObjects){
//            singlePlayer.transform.parent = ConnectedPlayersStaticScript.instance.transform;
//        }
//        
        //Instruct all clients to change parent as well.
        RpcChangeParent();
    }
    [ClientRpc]
    public void RpcAssignClientID(int id){
        selfId = id;
    }
    
    [ClientRpc]
    public void RpcChangeParent(){
        GameObject[] PlayerObjects = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < PlayerObjects.Length; i++){//GameObject singlePlayer in PlayerObjects){
            PlayerObjects[i].transform.parent = ConnectedPlayersStaticScript.instance.transform;
        }
    }
}
