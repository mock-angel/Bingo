using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

[RequireComponent(typeof(PhotonView))]
public class PhotonPlayerScript : MonoBehaviourPunCallbacks, IPunObservable
{
    public static PhotonPlayerScript scriptInstance;
    
    public string playerName = "";
    public int selfId;
    public bool ready;
    
    public bool isTurn = false;
    public bool gameWon = false;
    public int winOrder = 0;
    
    private bool transformed = false;
    
    private GameObject AllPlayersObj;
    
    protected int thisTurnNumberSelected = 0;
    private PhotonPlayerScript currentTurnScriptInstance = null;
    
    public int nextPlayerid = 0;
    
    public void ChangeParent(){
        if(gameObject != null){
            if(gameObject.transform != null) {
                gameObject.transform.parent = ConnectedPlayersStaticScript.instance.transform;
                transformed = true;
            }
        }
    }
    
    public void StartLocalPlayer(){
        scriptInstance = this;
        selfId = PhotonNetwork.LocalPlayer.ActorNumber;
        nextPlayerid = PhotonNetwork.LocalPlayer.GetNext().ActorNumber;
    }
    
    void Update(){
        
        if(!transformed) ChangeParent();
        
        if(AllPlayersObj == null) AllPlayersObj = ConnectedPlayersStaticScript.instance;
        
        if (!photonView.IsMine) return;
        
        //If game started.
        if(PhotonGameManagerBingo.scriptInstance.gameStarted){
            //Find who has the turn...
            PhotonPlayerScript mostCurrentTurnSciptInstance = null;
        
            List<PhotonPlayerScript> otherPhotonPlayerScripts = AllPlayersObj.GetComponentsInChildren<PhotonPlayerScript>().ToList();;    
            
            for(int i = 0; i < otherPhotonPlayerScripts.Count; i++)
            {
                if(otherPhotonPlayerScripts[i].isTurn){
                    mostCurrentTurnSciptInstance = otherPhotonPlayerScripts[i];
                    break;
                }
            }
            
            //If game just started...
            if(currentTurnScriptInstance == null){
                currentTurnScriptInstance = mostCurrentTurnSciptInstance;
            }
            else if (currentTurnScriptInstance != mostCurrentTurnSciptInstance){
                //TODO: Run this script only if others turn Finished?
                
                //So turn changed and previous turn just finished.
                //Now we need to update our cards with data from turn.
                
                //First check if the number is updated. Return if not.
                if (PhotonPlayerScript.scriptInstance.selfId == currentTurnScriptInstance.nextPlayerid){
                    PhotonPlayerScript.scriptInstance.isTurn = true;
                    mostCurrentTurnSciptInstance = this;
                }
                
                //Update Cards now.
                PhotonGameManagerBingo. scriptInstance.OtherPlayersTurnFinished(currentTurnScriptInstance.thisTurnNumberSelected);
                
                //Now we know whose current turn.
                if(mostCurrentTurnSciptInstance!= null) currentTurnScriptInstance = mostCurrentTurnSciptInstance;
                print("Updating cards now.");
            }
        } 
    }
                         
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
    
        if(stream.IsWriting){
            stream.SendNext(playerName);
            stream.SendNext(thisTurnNumberSelected);
            stream.SendNext(ready);
            stream.SendNext(isTurn);
            stream.SendNext(selfId);
            stream.SendNext(nextPlayerid);
            stream.SendNext(gameWon);
            stream.SendNext(winOrder);
        }else{
            playerName = (string) stream.ReceiveNext();
            thisTurnNumberSelected = (int) stream.ReceiveNext();
            ready = (bool) stream.ReceiveNext();
            isTurn = (bool) stream.ReceiveNext();
            selfId = (int) stream.ReceiveNext();
            nextPlayerid = (int) stream.ReceiveNext();
            gameWon = (bool) stream.ReceiveNext();
            winOrder = (int) stream.ReceiveNext();
        }
    }
    
    public void CmdTurnFinished(int numberSelected){
        isTurn = false;
        thisTurnNumberSelected = numberSelected;
        
        if(PhotonNetwork.LocalPlayer.GetNext() != null)
            nextPlayerid = PhotonNetwork.LocalPlayer.GetNext().ActorNumber;
        else nextPlayerid = selfId;
    }
    
    public void ClientOnReady(bool readyVar){
        ready = readyVar;
    }
    
    public void CmdGameWon(){
        if(gameWon) return;
        
        gameWon = true;
        
        List<PhotonPlayerScript> otherPhotonPlayerScripts = AllPlayersObj.GetComponentsInChildren<PhotonPlayerScript>().ToList();;    
        
        int gameWonCount = 0;
        for(int i = 0; i < otherPhotonPlayerScripts.Count; i++)
        {
            if(otherPhotonPlayerScripts[i].gameWon){
                gameWonCount += 1;
            }
        }
        
        winOrder = gameWonCount;
    }
}
