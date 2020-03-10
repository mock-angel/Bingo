using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int Coins = 0;
    
    private ThisGameData thisGameData;
    
    public void Start(){
        thisGameData = gameObject.GetComponent<ThisGameData>();
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
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Coins")
        {
            thisGameData.coinsCollected += collision.gameObject.GetComponent<CoinData>().CoinValue;
            Destroy(collision.gameObject);
        }
    }
    
}
