using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour {

    public Player playerData;

    // Use this for initialization
    void Start () {
        playerData.playerDeck.Add(new SwordCard());
        playerData.playerDeck.Add(new ShieldCard());
        playerData.playerDeck.Add(new SwordCard());
        playerData.playerDeck.Add(new SwordCard());
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void ShuffleDeck()
    {
        DeckUtilities.Shuffle<Card>(playerData.playerDeck);
    }

    public bool DrawNextCard()
    {
        if(playerData.playerDeck.Count <= 0)
            return false;

        if (playerData.playerCards.Count >= playerData.MAX_CARDS_IN_HAND)
            return false;

        playerData.playerCards.Add(playerData.playerDeck[0]);
        playerData.playerDeck.RemoveAt(0);
        return true;
    }
}
