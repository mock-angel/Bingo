using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//using Mirror;

public class GameManagerBingo : MonoBehaviour
{
    public static GameManagerBingo scriptInstance;
    public List<GameObject> Cards;
    
    public GameObject BingoCardPrefab;
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
    public int currentBingoCount = 0;
    
    private GameObject BingoHorizontal;
    private List<GameObject> bingoCardList;
    void Start(){
        horizontalsList = new List<GameObject>();
        idLocation = new List<int>();
        Cards = new List<GameObject>();
        bingoCardList = new List<GameObject>();
        
        CreateEmptyCards();
        scriptInstance = this;
    }
    
    void OnEnable()
    {
//        horizontalsList = new List<GameObject>();
//        idLocation = new List<int>();
//        Cards = new List<GameObject>();
        CreateEmptyCards();
    }
    
    public void DestroyHorizontals(){
        for(int i = 0; i < Cards.Count; i++){
            Destroy(Cards[i]);
        }
        
        for(int i = 0; i < horizontalsList.Count; i++){
            Destroy(horizontalsList[i]);
        }
        
        horizontalsList.Clear();
        Cards.Clear();
    }
    
    public void Reset(){
        DestroyHorizontals();
        
        //Reset all list elements.
//        idLocation = new List<int>();
        idLocation.Clear();
        
        print("Reached break point");
        
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
        
        bingoCardList.Clear();
        Destroy(BingoHorizontal);
        BingoHorizontal = Instantiate(Horizontals, GamePanelObject.transform);
        
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
        
        List<string> listBingo = new List<string>(){"B", "I", "N", "G", "O"};
        
        //Now add bingo cards;
        for(int i = 0; i < listBingo.Count; i++){
            GameObject bingoCard = Instantiate(BingoCardPrefab, BingoHorizontal.transform);
            bingoCard.GetComponent<BingoNum>().text.text = listBingo[i];
            bingoCardList.Add(bingoCard);
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
        
        bingoCardList.Clear();
        Destroy(BingoHorizontal);
        BingoHorizontal = Instantiate(Horizontals, GamePanelObject.transform);
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
        
        List<string> listBingo = new List<string>(){"B", "I", "N", "G", "O"};
        
        //Now add bingo cards;
        for(int i = 0; i < listBingo.Count; i++){
            GameObject bingoCard = Instantiate(BingoCardPrefab, BingoHorizontal.transform);
            bingoCard.GetComponent<BingoNum>().text.text = listBingo[i];
            bingoCardList.Add(bingoCard);
        }
    }
    
    public void Tick(GameObject card, int id){
        print("Pressed : " + id);
        
        SetGamePlayEnable(false);
        gamePlayEnabled = false;
        
        ClientPlayerScript.scriptInstance.CmdTurnFinished(id);
        
//        idLocation[idLocation.IndexOf(id)] = 0;
        //The below line might be a fix, but might break system.
//        ClientPlayerScript.scriptInstance.isTurn = false;
    }
    
    public void TurnFinished(int numberSelected){
        Cards[idLocation.IndexOf(numberSelected)].GetComponent<Button>().interactable = false;
        
        //This is done fo convinence in BINGO checking.
        idLocation[idLocation.IndexOf(numberSelected)] = 0;
        
        CheckBingo();
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
        //check horizontal first.
        bool flag = true;
        int bingoCount = 0;
        
        //TODO: Combine horizontal and vertical checks to reduce search complexity by 2?
        flag = true;
        for (int i = 0; i < 5; i++){
            flag = true;
            for(int j = i*5; j < (5 + i*5); j++){
                if(idLocation[j] != 0)
                    flag = false;
            }
            if(flag == true) bingoCount++;
        }
//        string stringDeb = "";
        for (int i = 0; i < 5; i++){
            flag = true;
            for(int j = i*5; j < (5 + i*5); j++){
                if(idLocation[(j%5)*5 + i] != 0){
//                    stringDeb += (j%5)*5 + i;
                    flag = false;
                }
            }
            if(flag == true) bingoCount++;
        }
//        print(stringDeb);
//        
//        //Check diagonal
        flag = true;
        for (int i = 0; i < 5; i++){
            if(idLocation[i*5 + i] != 0)
                flag = false;
        }
        if(flag == true) bingoCount++;
        
        flag = true;
        for (int i = 0; i < 5; i++){
            if(idLocation[(i+1)*4] != 0)
                flag = false;
        }
        if(flag == true) bingoCount++;
        
        if (bingoCount>5) bingoCount = 5;
        
        currentBingoCount = bingoCount;
//        print("Bingo count is: " + bingoCount);
    }
}


