using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int Coins;
//    public int LevelsReached;
    public PlayerData(Player player){
        Coins = player.Coins;
    }
}
