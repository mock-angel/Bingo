using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.SceneManagement;

public class menuLogic : MonoBehaviour
{
    public void OnChangeToGameLevel(){
        SceneManager.LoadScene("GameScene");
    }
}
