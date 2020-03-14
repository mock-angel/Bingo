using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ClientPlayerScript : NetworkBehaviour
{
//    public List<PlayerNetworkData> playerDataObj;
    public string playerName;
    public int selfId;
    
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
    
    [ClientRpc]
    public void RpcAssignClientID(int id){
        selfId = id;
    }
}
