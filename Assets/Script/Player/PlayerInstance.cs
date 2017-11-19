using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum CardType { Sword, Shield }

[Serializable]
public struct CardInfo
{
    public CardType cardType;
    public int value1;
    public int value2;
    public int value3;
    public int value4;
}



public class PlayerInstance : MonoBehaviour {


    public Player playerData;

    public List<GameObject> selectedCards= new List<GameObject>();

    public CardInfo[] buildDeckWith;

    // Use this for initialization
    void Start () {
        playerData = new Player();

        if (GameObject.Find("PersistentPlayerData") && GameObject.Find("PersistentPlayerData").GetComponent<PersistentPlayerData>().isInitialized)
        {
            playerData.playerDeck = GameObject.Find("PersistentPlayerData").GetComponent<PersistentPlayerData>().PlayerData.playerDeck;
        }
        else
        {
            InitDeck();
        }

        playerData.ShuffleDeck();
        InitHand();
        UIManager.instance.PlayerInitHand(playerData.playerCards);

        GameManager.instance.CurrentPlayer = this;
    }
	
	// Update is called once per frame
	void Update () {

	}

 
    public void InitDeck()
    {
        for (int i = 0; i < buildDeckWith.Length; i++)
        {
            switch (buildDeckWith[i].cardType)
            {
                case CardType.Sword:
                    playerData.playerDeck.Add(new SwordCard(buildDeckWith[i].value1, buildDeckWith[i].value2, buildDeckWith[i].value3, buildDeckWith[i].value4));
                    break;
                case CardType.Shield:
                    playerData.playerDeck.Add(new ShieldCard(buildDeckWith[i].value1, buildDeckWith[i].value2, buildDeckWith[i].value3, buildDeckWith[i].value4));
                    break;
            }

        }

        // Default
        if (buildDeckWith.Length == 0)
        {
            playerData.playerDeck.Add(new SwordCard(1, 2, 3, 4));
            playerData.playerDeck.Add(new ShieldCard(1, 2, 3, 4));
        }
    }

    public void InitHand()
    {
        for ( int i = 0; i < Player.MAX_CARDS_IN_HAND; i++)
        {
            DrawCard();
        }
    }

    public void DrawCard()
    {
        playerData.DrawNextCard();
        UIManager.instance.RefreshNbCardLeftPlayer(playerData.playerDeck.Count);
    }
}
