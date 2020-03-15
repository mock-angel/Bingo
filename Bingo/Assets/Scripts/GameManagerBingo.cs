using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//using Mirror;

public class GameManagerBingo : MonoBehaviour
{
    public static GameManagerBingo scriptInstance;
    public List<GameObject> Cards;
    public GameObject CardPrefab;
    public GameObject EmptyCardPrefab;
    public int current;
    
    public GameObject GamePanelObject;
    public GameObject Horizontals;
    
    private bool ready = false;
    
    public List<GameObject> horizontalsList;
    
    //List of nums that are allready selected.
    private List<int> idLocation;
    private int nextCardNum = 1;
    
    public DisplayContentManager contextManager;
    
    private bool gamePlayEnabled = false;
    
    public GameObject DisableCanvas;
    
    void Start(){
        horizontalsList = new List<GameObject>();
        idLocation = new List<int>();
        Cards = new List<GameObject>();
        
        CreateEmptyCards();
        scriptInstance = this;
    }
    
    void OnEnable()
    {
        CreateEmptyCards();
    }
    
    public void DestroyHorizontals(){
        for(int i = 0; i < horizontalsList.Count; i++){
            Destroy(horizontalsList[i]);
        }
        
        horizontalsList.Clear();
        Cards.Clear();
    }
    
    public void Reset(){
        DestroyHorizontals();
        
        //Reset all list elements.
        idLocation.Clear();
        
        //Add 25 zeroes to idLocation.
        for (int i = 0; i < 25; i++){
            idLocation.Add(0);
        }
        
        ready = false;
        nextCardNum = 1;
    }
    
    public bool isReady(){
        return ready;
    }
    
    public void CreateEmptyCards(){
        
        Reset();
        ready = false;
        
        SetGamePlayEnable(true);
        gamePlayEnabled = true;
        
        for (int i = 1; i <= 5; i++){
            horizontalsList.Add(Instantiate(Horizontals, GamePanelObject.transform));
        }
        
        int id = 0;
        for (int i = 0; i < 5; i++){
            for (int j=1; j<=5 ; j++){
                GameObject card = Instantiate(EmptyCardPrefab, horizontalsList[i].transform);
                
                card.GetComponent<CardNumGetter>().id = id++;
                card.GetComponent<CardNumGetter>().gameManagerBingo = this;
            }
        }
    }
    
    public int getNextCardNumAndAssign(int id){
        idLocation[id] = nextCardNum;
        if(nextCardNum == 25) {
            ready = true;
            contextManager.ReadyButton.GetComponent<Button>().interactable = true;
        }
        
        return nextCardNum++;
    }
    
    public void RpcStartGame(){
        DestroyHorizontals();
        
        Cards.Clear();
        horizontalsList.Clear();
        
        SetGamePlayEnable(false);
        gamePlayEnabled = false;
        
        for (int i = 1; i <= 5; i++){
            horizontalsList.Add(Instantiate(Horizontals, GamePanelObject.transform));
        }
        
        int loc = 0;
        for (int i = 0; i < 5; i++){
            for (int j=1; j<=5 ; j++){
                GameObject card = Instantiate(CardPrefab, horizontalsList[i].transform);
                
                card.GetComponent<CardNumPlayer>().id = idLocation[loc++];
                card.GetComponent<CardNumPlayer>().gameManagerBingo = this;
                card.GetComponent<CardNumPlayer>().ApplyText();
                card.GetComponent<Button>().onClick.AddListener ( delegate { Tick(card, card.GetComponent<CardNumPlayer>().id); });
//                card.GetComponent<Button>().interactable = false;
                Cards.Add(card);
            }
        }
    }
    
    public void Tick(GameObject card, int id){
        print("Pressed : " + id);
        
        SetGamePlayEnable(false);
        gamePlayEnabled = false;
        
        ClientPlayerScript.scriptInstance.CmdTurnFinished(id);
        
        //The below line might be a fix, but might break system.
//        ClientPlayerScript.scriptInstance.isTurn = false;
    }
    
    public void TurnFinished(int numberSelected){
        Cards[idLocation.IndexOf(numberSelected)].GetComponent<Button>().interactable = false;
    }
    
    public void SetGamePlayEnable(bool gamePlayEnabled){
        DisableCanvas.SetActive(!gamePlayEnabled);
    }
    
    void FixedUpdate(){
        
        if(ClientPlayerScript.scriptInstance.gameStarted == false) return; 
        if(ClientPlayerScript.scriptInstance.isTurn == true){
            if(gamePlayEnabled == true){
                return;
            }
            else{
                //Enable gameplay.
                SetGamePlayEnable(true);
                gamePlayEnabled = true;
            }
        }
        else{
            if(gamePlayEnabled == false){
                return;
            }
            else{
                SetGamePlayEnable(false);
                gamePlayEnabled = false;
            }
        }
    }
    
    public void CheckBingo(){
    }
}


