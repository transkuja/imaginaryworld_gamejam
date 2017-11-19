using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentPlayerData : MonoBehaviour {

    private Player playerData;
    public bool isInitialized = false;

    public Player PlayerData
    {
        get
        {
            return playerData;
        }

        set
        {
            playerData = value;
            isInitialized = true;
        }
    }

    void Start () {
        DontDestroyOnLoad(gameObject);
	}
	
}
