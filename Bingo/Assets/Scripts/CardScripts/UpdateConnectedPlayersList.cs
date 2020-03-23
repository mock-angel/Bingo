using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class UpdateConnectedPlayersList : MonoBehaviour
{
    private GameObject AllPlayersObj;
    private TextMeshProUGUI connectedPlayersText;
    
    void Start(){
        connectedPlayersText = gameObject.GetComponent<TextMeshProUGUI>();
    }
    
    void FixedUpdate()
    {
        if(AllPlayersObj == null)
            AllPlayersObj = ConnectedPlayersStaticScript.instance.gameObject;
        
        string connectedListString = "";
        
        List<Transform> childrenList = AllPlayersObj.transform.Cast<Transform>().ToList();
        
        Transform child;
        for(int i = 0; childrenList.Count > i; i++)
        {   
            child = childrenList[i];
            PhotonPlayerScript clientPlayerScript = child.gameObject.GetComponent<PhotonPlayerScript>();
            
            string clientDisplayName = clientPlayerScript.playerName;
            
            //If game started, get from PhotonGameManagerBingo.
            if(PhotonGameManagerBingo.scriptInstance.gameStarted == true){
                //Do something if player won game.
                if(clientPlayerScript.gameWon == true)
                    clientDisplayName += " (Won " + clientPlayerScript.winOrder + " )" ;
                
                //If this player yet to win, do somethign if its turn.
                else if (clientPlayerScript.isTurn) clientDisplayName += " (Current Turn)";
            }
            
            //If still in selection round, check if player ready.
            else if (clientPlayerScript.ready) clientDisplayName += " (Ready)";
            
            clientDisplayName += "\n";
            connectedListString += clientDisplayName;
        }
        
        //Set the player display list text.
        connectedPlayersText.text = connectedListString;
    }
}
