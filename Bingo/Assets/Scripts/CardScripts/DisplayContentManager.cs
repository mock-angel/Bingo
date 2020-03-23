using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class DisplayContentManager : MonoBehaviour
{
    public static DisplayContentManager scriptInstance;
    public Text DisplayText;
    public GameObject ReadyButton;
    public Button StartButtonScript;
    public PhotonGameManagerBingo BingoManager;
    
    public string NotReadyText = "Please Fill All Tiles";
    public string PressReadyText = "Press Ready To begin";
    
    void Start(){
        scriptInstance = this;
    }
    
    void FixedUpdate(){
        //Updates instruction text at the top.
        if(scriptInstance == null)  scriptInstance = this;
        
        if(BingoManager.isReady() == false){
            DisplayText.text = NotReadyText;
        }
        else {
            DisplayText.text = PressReadyText;
        }
    }
}
