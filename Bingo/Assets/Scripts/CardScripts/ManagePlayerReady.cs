using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagePlayerReady : MonoBehaviour
{
    public void OnReady(bool ready){
        PhotonPlayerScript.scriptInstance.ClientOnReady(ready);
        
        if(!PhotonGameManagerBingo.scriptInstance.isReady()){
            gameObject.GetComponent<Button>().interactable = false;
        }
    }
}
