using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class DisplayContentManager : MonoBehaviour
{
    public Text DisplayText;
    public GameObject ReadyButton;
    public PhotonGameManagerBingo BingoManager;
    
    public string NotReadyText = "Please Fill All Tiles";
    public string PressReadyText = "Press Ready To begin";
    public void OnReadyPress(){
        ReadyButton.GetComponent<Button>().interactable = false;
    }
    
    void FixedUpdate(){
        if(BingoManager.isReady() == false){
            DisplayText.text = NotReadyText;
            ReadyButton.GetComponent<Button>().interactable = false;
        }
        else {
            DisplayText.text = PressReadyText;
            //TODO: Moved to GameManagerBingo, is it an effecient method?
//            ReadyButton.GetComponent<Button>().interactable = true;
        }
        //else()
    }
}
