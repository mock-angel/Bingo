using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string DisplayName;
    public PlayerData(Player player){
        DisplayName = player.DisplayName;
    }
}
