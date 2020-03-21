using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using MyAttributes;

public class Player : MonoBehaviour
{
    public string DisplayName;
    public static Player instance;
    
    public bool displayAsText;
    [ConditionalField("displayAsText")]
    public TMP_InputField displayText;
    
    public void Start(){
        instance = this;
        LoadPlayer();
    }
    
    public void SavePlayer(){
        SaveSystem.SavePlayer(this);
    }
    
    public void LoadPlayer(){
        PlayerData data = SaveSystem.LoadPlayer();
        if(data != null)
            DisplayName = data.DisplayName;
        else SavePlayer();
        
        if(displayAsText) displayText.text = DisplayName;
    }
    
    public void UpdatePlayerName(){
        if(displayAsText) DisplayName = displayText.text;
        SavePlayer();
    }
}
