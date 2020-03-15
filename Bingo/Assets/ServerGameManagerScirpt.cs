using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ServerGameManagerScirpt : MonoBehaviour
{
    private GameObject AllPlayersObj;
    public bool EveryOneReady = false;
    public Button StartButton;
    
    void Start(){
        AllPlayersObj = ConnectedPlayersStaticScript.instance;
    }
    
    public void StartGame(){
        //Constantly check if all players are ready;
        foreach (Transform child in AllPlayersObj.transform)
        {
            
            ClientPlayerScript clientPlayerScript = child.gameObject.GetComponent<ClientPlayerScript>();
            clientPlayerScript.RpcStartGame();
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
