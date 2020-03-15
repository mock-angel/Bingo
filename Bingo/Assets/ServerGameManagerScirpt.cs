using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerGameManagerScirpt : MonoBehaviour
{
    public GameObject AllPlayersObj;
    public bool EveryOneReady = false;
    
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
        
    }
}
