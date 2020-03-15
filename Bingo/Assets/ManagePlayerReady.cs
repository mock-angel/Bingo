using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagePlayerReady : MonoBehaviour
{
    public void OnReady(bool ready){
        ClientPlayerScript.scriptInstance.ClientOnReady(ready);
    }
}
