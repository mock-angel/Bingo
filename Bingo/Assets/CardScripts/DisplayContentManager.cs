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
    
    
    public void OnReadyPress(){
        ReadyButton.GetComponent<Button>().interactable = false;
        
//        if(PhotonNetwork.IsMasterClient)
//            StartButton.SetActive(true);
//        else
//            StartButton.SetActive(false);
    }
    
    void FixedUpdate(){
        
        if(scriptInstance == null)  scriptInstance = this;
        
        if(BingoManager.isReady() == false){
            DisplayText.text = NotReadyText;
            ReadyButton.GetComponent<Button>().interactable = false;
            
        }
        else {
            DisplayText.text = PressReadyText;
//            if(PhotonPlayerScript.scriptInstance.gameStarted)
//                StartButton.SetActive(false);
            //TODO: Moved to GameManagerBingo, is it an effecient method?
//            ReadyButton.GetComponent<Button>().interactable = true;
        }
        //else()
    }
}
