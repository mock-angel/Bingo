using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateConnectedPlayersList : MonoBehaviour
{
    public GameObject AllPlayersObj;
    private TextMeshProUGUI connectedPlayersText;
    
    void Start(){
        connectedPlayersText = gameObject.GetComponent<TextMeshProUGUI>();
    }
    
    void FixedUpdate()
    {
        string connectedListString = "";
        foreach (Transform child in AllPlayersObj.transform)
        {
            ClientPlayerScript clientPlayerScript = child.gameObject.GetComponent<ClientPlayerScript>();
            connectedListString += clientPlayerScript.playerName + "\n";
        }
        connectedPlayersText.text = connectedListString;
//        for (int i = 0; i < children; i++){
//            ClientPlayerScript clientPlayerScript = transform.GetChild(i).gameObject.GetComponent<ClientPlayerScript>();
//            connectedList += clientPlayerScript.playerName + "\n";
//        }
    }
}
