using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerBingo : MonoBehaviour
{
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
    
    void Start(){
        horizontalsList = new List<GameObject>();
        idLocation = new List<int>();
        
        CreateEmptyCards();
    }
    
    public void DestroyHorizontals(){
        for(int i = 0; i < horizontalsList.Count; i++){
            Destroy(horizontalsList[i]);
        }
        
        horizontalsList.Clear();
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
}
