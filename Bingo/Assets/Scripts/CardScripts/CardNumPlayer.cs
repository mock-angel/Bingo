using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardNumPlayer : MonoBehaviour
{
    public int id = 0;
    public PhotonGameManagerBingo gameManagerBingo;
    public TextMeshProUGUI text;
    
    public void ApplyText(){
        text.text = "" + id;
    }
}
