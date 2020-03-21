using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(PhotonView))]
public class PhotonPlayerScript : MonoBehaviourPunCallbacks, IPunObservable
{
    public bool devTesting = false;
    
    public static PhotonPlayerScript scriptInstance;
//    public PhotonView photonView;
//    public List<PlayerNetworkData> playerDataObj;
//    [SyncVar]
    public string playerName = "Unnamed";
    
//    [SyncVar]
    public int selfId;
    
//    [SyncVar]
    public bool ready;
    
    bool ObjectAuthority = false;
    
//    [SyncVar]
    public bool isTurn = false;
    
    public bool gameStarted = false;
    
//    [SyncVar]
    public bool gameWon = false;
//    [SyncVar]
    public int winOrder = 0;
    
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
    
    public void Start(){
        ChangeParent();
    }
    
    public void ChangeParent(){
        transform.parent = ConnectedPlayersStaticScript.instance.transform;
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
        }else{
            playerName = (string) stream.ReceiveNext();
            ready = (bool) stream.ReceiveNext();
            isTurn = (bool) stream.ReceiveNext();
        }
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
    public void CmdTurnFinished(int numberSelected){
        RpcTurnFinished(numberSelected);
        ServerGameManagerScirpt.scriptInstance.TurnFinished(numberSelected);
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
    
//    [ClientRpc]
    public void RpcStartGame(){
        gameStarted = true;
        if(ObjectAuthority == true){
            print("Authority start");
            PhotonGameManagerBingo.scriptInstance.RpcStartGame();
        }
    }
}
