using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class PhotonRoom : MonoBehaviourPunCallbacks
{
    
    public override void OnDisconnected(DisconnectCause cause){
        menuLogic.OnChangeToLobby();
        print("Disconnected from photon, must start over.");
    }
}
