using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class ServerGameManagerScirpt : MonoBehaviour
{
    public static ServerGameManagerScirpt scriptInstance;
    
    private GameObject AllPlayersObj;
    public bool EveryOneReady = false;
    
    public int winnersCount = 0;
    
    void Start(){
        scriptInstance = this;
        AllPlayersObj = ConnectedPlayersStaticScript.instance;
    }
    
//    public void StartGame(){
//        //Start game for all players
////        foreach (Transform child in AllPlayersObj.transform)
////        {
////            PhotonPlayerScript clientPlayerScript = child.gameObject.GetComponent<ClientPlayerScript>();
////            clientPlayerScript.CmdStartGame();
////        }
////        PhotonPlayerScript.scriptInstance.CmdStartGame();
////        PhotonPlayerScript.scriptInstance.isTurn = true;
//    }
    
    public void TurnFinished(int numberSelectedDuringTurn){
        
        //Need to move all code away from this function into PlayerScript for consistency.
        return;
        
        bool turnPlayerDetected = false;
        print("Tick validity");
        
        List<PhotonPlayerScript> clientScripts = new List<PhotonPlayerScript>();
        
        if(AllPlayersObj == null) AllPlayersObj = ConnectedPlayersStaticScript.instance;
        foreach (Transform child in AllPlayersObj.transform)
        {
            PhotonPlayerScript clientPlayerScript = child.gameObject.GetComponent<PhotonPlayerScript>();
            
            clientScripts.Add(clientPlayerScript);
            
        }
        
        for(int i = 0; i < clientScripts.Count; i++){
            PhotonPlayerScript clientPlayerScript = clientScripts[i];
            if(clientPlayerScript.isTurn == true){
                clientPlayerScript.isTurn = false;
                turnPlayerDetected = true;
            }else if(turnPlayerDetected == true){
                if(clientPlayerScript.gameWon == false){
                    clientPlayerScript.isTurn = true;
                    return;
                }
            }
        }
        
        //If it survived the check, then select first player.
        foreach (Transform child in AllPlayersObj.transform)
        {
            PhotonPlayerScript clientPlayerScript = child.gameObject.GetComponent<PhotonPlayerScript>();
            if(clientPlayerScript.gameWon == false){
                clientPlayerScript.isTurn = true;
                return;
            }
        }
    }
    
    void FixedUpdate()
    {
        if(PhotonGameManagerBingo.scriptInstance.gameStarted) return;
        
        //Assume everyone's ready
        bool everyOneReady = true;
        if(AllPlayersObj == null) AllPlayersObj = ConnectedPlayersStaticScript.instance;
        
        //Constantly check if all players are ready;
        foreach (Transform child in AllPlayersObj.transform)
        {
            PhotonPlayerScript clientPlayerScript = child.gameObject.GetComponent<PhotonPlayerScript>();
            if(!clientPlayerScript.ready)
                everyOneReady = false;
        }
        
        if(PhotonNetwork.IsMasterClient){
            DisplayContentManager.scriptInstance.StartButtonScript.gameObject.SetActive(true);
        }
        
        EveryOneReady = everyOneReady;
        if(PhotonNetwork.IsMasterClient){
            DisplayContentManager.scriptInstance.StartButtonScript.interactable = EveryOneReady;
        }
    }
}
