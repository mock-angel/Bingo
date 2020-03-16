using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardNumGetter : MonoBehaviour
{
    public int id = 0;
    public GameManagerBingo gameManagerBingo;
    public Text text;
    
    public void AssignCardNum(){
        int i = gameManagerBingo.getNextCardNumAndAssign(id);
        text.text = "" + i;
        
        //Also disable this button, but prolly do it in OnClick;
    }
}
