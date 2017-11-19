using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyInstance: MonoBehaviour {

    public AIType typeOfAI;
    public Player enemyData;

    public CardInfo[] buildDeckWith;

    // Use this for initialization
    void Start () {
        enemyData = new Player();
        enemyData.lootQuantity = 5;

        InitDeck();

        Debug.Log(enemyData.playerDeck.Count);
        enemyData.ShuffleDeck();
        InitHand();

        UIManager.instance.EnemyInitHand(enemyData.playerCards);

        GameManager.instance.CurrentEnemy = this;
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
                    enemyData.playerDeck.Add(new SwordCard(buildDeckWith[i].value1, buildDeckWith[i].value2, buildDeckWith[i].value3, buildDeckWith[i].value4));
                    break;
                case CardType.Shield:
                    enemyData.playerDeck.Add(new ShieldCard(buildDeckWith[i].value1, buildDeckWith[i].value2, buildDeckWith[i].value3, buildDeckWith[i].value4));
                    break;
            }

        }

        // Default
        if (buildDeckWith.Length == 0)
        {
            enemyData.playerDeck.Add(new SwordCard(1, 2, 3, 4));
            enemyData.playerDeck.Add(new SwordCard(1, 2, 3, 4));
            enemyData.playerDeck.Add(new SwordCard(1, 2, 3, 4));
            enemyData.playerDeck.Add(new SwordCard(1, 2, 3, 4));
            enemyData.playerDeck.Add(new SwordCard(1, 2, 3, 4));
            enemyData.playerDeck.Add(new SwordCard(1, 2, 3, 4));
            enemyData.playerDeck.Add(new SwordCard(1, 2, 3, 4));
            enemyData.playerDeck.Add(new SwordCard(1, 2, 3, 4));
            enemyData.playerDeck.Add(new SwordCard(1, 2, 3, 4));
            enemyData.playerDeck.Add(new SwordCard(1, 2, 3, 4));
            enemyData.playerDeck.Add(new ShieldCard(1, 2, 3, 4));
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
        if (enemyData.DrawNextCard())
        {
         
            UIManager.instance.RefreshNbCardLeftEnemy(enemyData.playerDeck.Count);
        }
        else
        {
            Destroy(UIManager.instance.DeckEnemy.gameObject);
        }
    }

}
