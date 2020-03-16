using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedPlayersStaticScript : MonoBehaviour
{
    public static GameObject instance;
    void Start()
    {
        instance = gameObject;
    }
}
