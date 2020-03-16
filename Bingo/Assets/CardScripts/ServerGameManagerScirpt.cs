using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ServerGameManagerScirpt : MonoBehaviour
{
    public static ServerGameManagerScirpt scriptInstance;
    
    private GameObject AllPlayersObj;
    public bool EveryOneReady = false;
    public Button StartButton;
    
    public int winnersCount = 0;
    
    void Start(){
        scriptInstance = this;
        AllPlayersObj = ConnectedPlayersStaticScript.instance;
    }
    
    public void StartGame(){
        //Constantly check if all players are ready;
        foreach (Transform child in AllPlayersObj.transform)
        {
            ClientPlayerScript clientPlayerScript = child.gameObject.GetComponent<ClientPlayerScript>();
            clientPlayerScript.RpcStartGame();
        }
        
        ClientPlayerScript.scriptInstance.isTurn = true;
    }
    
    public void TurnFinished(int numberSelectedDuringTurn){
        
        bool turnPlayerDetected = false;
        print("Tick validity");
        
        List<ClientPlayerScript> clientScripts = new List<ClientPlayerScript>();
        
        foreach (Transform child in AllPlayersObj.transform)
        {
            ClientPlayerScript clientPlayerScript = child.gameObject.GetComponent<ClientPlayerScript>();
            
            clientScripts.Add(clientPlayerScript);
            
        }
        
        for(int i = 0; i < clientScripts.Count; i++){
            ClientPlayerScript clientPlayerScript = clientScripts[i];
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
            ClientPlayerScript clientPlayerScript = child.gameObject.GetComponent<ClientPlayerScript>();
            if(clientPlayerScript.gameWon == false){
                clientPlayerScript.isTurn = true;
                return;
            }
        }
    }
    
    void FixedUpdate()
    {
        //Assume everyone's ready
        bool everyOneReady = true;
        
        //Constantly check if all players are ready;
        foreach (Transform child in AllPlayersObj.transform)
        {
            ClientPlayerScript clientPlayerScript = child.gameObject.GetComponent<ClientPlayerScript>();
            if(!clientPlayerScript.ready)
                everyOneReady = false;
        }
        
        EveryOneReady = everyOneReady;
        StartButton.interactable = EveryOneReady;
    }
}
