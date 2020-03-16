using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardNumPlayer : MonoBehaviour
{
    public int id = 0;
    public GameManagerBingo gameManagerBingo;
    public Text text;
    
    public void ApplyText(){
        text.text = "" + id;
    }
    public void ClickedOnce(){
        
    }
}
