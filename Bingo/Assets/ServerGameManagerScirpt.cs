using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerGameManagerScirpt : MonoBehaviour
{
    public GameObject AllPlayersObj;
    void FixedUpdate()
    {
        //Constantly check if all players are ready;
        foreach (Transform child in AllPlayersObj.transform)
        {
            ClientPlayerScript clientPlayerScript = child.gameObject.GetComponent<ClientPlayerScript>();
            
        }
    }
}
