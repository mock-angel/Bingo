using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
//using Mirror;

[RequireComponent(typeof(PhotonView))]
public class PhotonGameManagerBingo : MonoBehaviourPunCallbacks, IPunObservable
{
    public static PhotonGameManagerBingo scriptInstance;
    
    //Created list of cards.
    public List<GameObject> Cards;
    
    //Prefabs for game.
    public GameObject BingoCardPrefab;
    public GameObject CardPrefab;
    public GameObject EmptyCardPrefab;
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
    
    public bool gameStarted = false;
    
    void Start(){
        horizontalsList = new List<GameObject>();
        idLocation = new List<int>();
        Cards = new List<GameObject>();
        bingoCardList = new List<GameObject>();
        
        CreateEmptyCards();
        scriptInstance = this;
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
        idLocation = new List<int>();
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
    
    //must be called only be master client.
    public void CmdStartGame(){
        gameStarted = true;
        PhotonView photonView = PhotonView.Get(this);
        PhotonPlayerScript.scriptInstance.isTurn = true;
        photonView.RPC("RpcStartGame", RpcTarget.All);
        
    }
    
    [PunRPC]
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
        
        PhotonPlayerScript.scriptInstance.CmdTurnFinished(id);
    }
    
    public void OtherPlayersTurnFinished(int numberSelected){
        if(numberSelected <1 || numberSelected>25) return;
        
        
        print("numberSelected : " +numberSelected);
        
        if(idLocation.IndexOf(numberSelected) == -1) return;
        Cards[idLocation.IndexOf(numberSelected)].GetComponent<Button>().interactable = false;
        
        //This is done fo convinence in BINGO checking.
        idLocation[idLocation.IndexOf(numberSelected)] = 0;
        
        CheckBingo();
    }
    
    public void SetGamePlayEnable(bool gamePlayEnabled){
        DisableCanvas.SetActive(!gamePlayEnabled);
    }
    
    void FixedUpdate(){
        
        if(gameStarted == false) return; 
        
        if(PhotonPlayerScript.scriptInstance.isTurn == true){
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
        
        for(int i = 0; i < bingoCount; i++){
            bingoCardList[i].GetComponent<Button>().interactable = false;
        }
        
        for(int i = bingoCount; i < 5; i++){
            bingoCardList[i].GetComponent<Button>().interactable = true;
        }
        
        //Call for victory if bingoCount == 5.
        if(bingoCount == 5) PhotonPlayerScript.scriptInstance.CmdGameWon();
        
//        print("Bingo count is: " + bingoCount);
    }
    
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
    
        if(stream.IsWriting){
            stream.SendNext(gameStarted);
        }else{
            gameStarted = (bool) stream.ReceiveNext();
        }
    }
}


