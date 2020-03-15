using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateConnectedPlayersList : MonoBehaviour
{
    private GameObject AllPlayersObj;
    private TextMeshProUGUI connectedPlayersText;
    
    void Start(){
        AllPlayersObj = ConnectedPlayersStaticScript.instance.gameObject;
        connectedPlayersText = gameObject.GetComponent<TextMeshProUGUI>();
    }
    
    void FixedUpdate()
    {
        string connectedListString = "";
        foreach (Transform child in AllPlayersObj.transform)
        {
            string clientDisplayName;
            ClientPlayerScript clientPlayerScript = child.gameObject.GetComponent<ClientPlayerScript>();
            clientDisplayName = clientPlayerScript.playerName;
            
            if(clientPlayerScript.gameStarted == true){
                if(clientPlayerScript.gameWon == true){
                    clientDisplayName += " (Won " + clientPlayerScript.winOrder + " )" ;
                }
                else if (clientPlayerScript.isTurn) clientDisplayName += " (Current Turn)";
            }
            else{
                if (clientPlayerScript.ready) clientDisplayName += " (Ready)";
            }
            
            clientDisplayName += "\n";
            connectedListString += clientDisplayName;
        }
        
        connectedPlayersText.text = connectedListString;
        
//        for (int i = 0; i < children; i++){
//            ClientPlayerScript clientPlayerScript = transform.GetChild(i).gameObject.GetComponent<ClientPlayerScript>();
//            connectedList += clientPlayerScript.playerName + "\n";
//        }
    }
}
