﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum CardType { Sword, Shield }
public class PlayerController : MonoBehaviour {
    [Serializable]
    public struct CardInfo
    {
        public CardType cardType;
        public int value1;
        public int value2;
        public int value3;
        public int value4;
    }

    public Player playerData;

    public CardInfo[] buildDeckWith;

    // Use this for initialization
    void Start () {
        playerData = new Player();

        InitDeck();
        playerData.ShuffleDeck();
        InitHand();


        UIManager.instance.PlayerInitHand(playerData.playerCards);
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
            playerData.DrawNextCard();
        }

    }

}
