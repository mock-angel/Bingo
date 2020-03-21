using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;


public class UpdateConnectedPlayersList : MonoBehaviour
{
    private GameObject AllPlayersObj;
    private TextMeshProUGUI connectedPlayersText;
//    private int ChildCount = 0;
    private bool isServer = false;
    
    
    void Start(){
        if(ConnectedPlayersStaticScript.instance != null)
            AllPlayersObj = ConnectedPlayersStaticScript.instance.gameObject;
        connectedPlayersText = gameObject.GetComponent<TextMeshProUGUI>();
    }
    
    void FixedUpdate()
    {
        if(AllPlayersObj == null)
            AllPlayersObj = ConnectedPlayersStaticScript.instance.gameObject;
        string connectedListString = "";
        
        List<Transform> childrenList = AllPlayersObj.transform.Cast<Transform>().ToList();
        
        if(childrenList.Count == 1){
            //Assign this player as server.
            isServer = true;
            
            //make player recognise itself as server.
            PhotonPlayerScript.scriptInstance.isServer = true;
        }
        
        Transform child;
        for(int i = 0; childrenList.Count > i; i++)
        {   
            child = childrenList[i];
            string clientDisplayName;
            PhotonPlayerScript clientPlayerScript = child.gameObject.GetComponent<PhotonPlayerScript>();
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
