﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardNumGetter : MonoBehaviour
{
    public int id = 0;
    public PhotonGameManagerBingo gameManagerBingo;
    public TextMeshProUGUI text;
    
    public void AssignCardNum(){
        if(gameManagerBingo == null) gameManagerBingo = PhotonGameManagerBingo.scriptInstance;
        
        int i = gameManagerBingo.getNextCardNumAndAssign(id);
        text.text = "" + i;
    }
}
