using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class ServerPlayerScript : NetworkBehaviour
{
    public static ServerPlayerScript instance;
    public List<ClientPlayerScript> ClientPlayerList;
    
    int connectedIds = 0;
    
    //Cuz we have only one? prolly move this during creation?
    public void Start(){
        instance = this;
    }
    
    public int getNextID(){
        return connectedIds++;
    }
}
