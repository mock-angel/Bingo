using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int Coins = 0;
    
    public void Start(){
        LoadPlayer();
    }
    
    public void SavePlayer(){
        SaveSystem.SavePlayer(this);
    }
    
    public void LoadPlayer(){
        PlayerData data = SaveSystem.LoadPlayer();
        if(data != null)
            Coins = data.Coins;
        else SavePlayer();
    }
}
