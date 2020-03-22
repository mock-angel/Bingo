using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

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
    
    
    public GameObject AllPlayersObj;
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
    
    public int thisTurnNumberSelected = 0;
    public PhotonPlayerScript currentTurnScriptInstance = null;
    
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
        
        selfId = PhotonNetwork.LocalPlayer.ActorNumber;
    }
    
    void Update(){
        
        if(!transformed) ChangeParent();
        
        if(AllPlayersObj == null) AllPlayersObj = ConnectedPlayersStaticScript.instance;
        
        //Find who has the turn...
        PhotonPlayerScript mostCurrentTurnSciptInstance = null;
        
        //Assign mostCurrentTurnSciptInstance.
        {
            List<PhotonPlayerScript> otherPhotonPlayerScripts = GetComponentsInChildren<PhotonPlayerScript>().OfType<PhotonPlayerScript>().ToList();;    
            
            for(int i = 0; i < otherPhotonPlayerScripts.Count; i++)
            {
                if(otherPhotonPlayerScripts[i].isTurn){
                    mostCurrentTurnSciptInstance = otherPhotonPlayerScripts[i];
                    print("Found a player with isTurn == true");
                    break;
                }
            }
        }
        
//        {
//            if(currentTurnScriptInstance == null){
//                //If its no ones turn.
//                //Happens when game first started.
//                //Also happens when current turn player left?.
//            }
//        }
        
        //If game started.
        if(PhotonGameManagerBingo.scriptInstance.gameStarted){
            // if game just started...
            if(currentTurnScriptInstance == null){
//                if(PhotonNetwork.IsMasterClient){
//                    isTurn = true;
//                }
                currentTurnScriptInstance = mostCurrentTurnSciptInstance;
                print("currentTurnScriptInstance was null, tried to correct that.");
            }
            else if (currentTurnScriptInstance != mostCurrentTurnSciptInstance){
                //TODO: Run this script only if others turn Finished?
                
                //So turn changed and previous turn just finished.
                //Now we need to update our cards with data from turn.
                
                //First check if the number is updated. Return if not.
//                if(currentTurnScriptInstance.thisTurnNumberSelected == 0) return;
                
                //Update Cards now.
                PhotonGameManagerBingo. scriptInstance.OtherPlayersTurnFinished(currentTurnScriptInstance.thisTurnNumberSelected);
                
                //Now we know whose turn is next.
                currentTurnScriptInstance = mostCurrentTurnSciptInstance;
                print("Updating cards now.");
            }
            
        } 
        
        if (photonView.IsMine || devTesting) {
            
            return;
            
        }
        else{
        
        }
    
    }
                         
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
    
        if(stream.IsWriting){
            stream.SendNext(playerName);
            stream.SendNext(thisTurnNumberSelected);
            stream.SendNext(ready);
            stream.SendNext(isTurn);
            stream.SendNext(selfId);
        }else{
            playerName = (string) stream.ReceiveNext();
            thisTurnNumberSelected = (int) stream.ReceiveNext();
            ready = (bool) stream.ReceiveNext();
            isTurn = (bool) stream.ReceiveNext();
            selfId = (int) stream.ReceiveNext();
        }
    }
    
    //    [Command]
    public void CmdTurnFinished(int numberSelected){
        isTurn = false;
        thisTurnNumberSelected = numberSelected;
//        RpcTurnFinished(numberSelected);
//        ServerGameManagerScirpt.scriptInstance.TurnFinished(numberSelected);
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
//    public void SetClientID(){
//        CmdAssignClientID();
//    }
    
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
//    public void CmdAssignClientID(){
////        int nextId = ServerPlayerScript.instance.getNextID();
//        selfId = ServerPlayerScript.instance.getNextID();
////        NetworkIdentity identity = GetComponent<NetworkIdentity>();
//    }
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
//    public void RpcTurnFinished(int numberSelected){
//        PhotonGameManagerBingo.scriptInstance.TurnFinished(numberSelected);
//    }
    
//    [ClientRpc]
    public void RpcChangeParent(){
        GameObject[] PlayerObjects = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < PlayerObjects.Length; i++){//GameObject singlePlayer in PlayerObjects){
            PlayerObjects[i].transform.parent = ConnectedPlayersStaticScript.instance.transform;
        }
    }
}
