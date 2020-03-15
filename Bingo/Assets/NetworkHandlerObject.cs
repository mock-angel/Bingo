using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class NetworkHandlerObject : NetworkBehaviour
{
    public bool start = false;
    
    public void startGame(){
        start = true;
    }
}
