using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player
{

    #region Properties
    public List<Card> playerCards = new List<Card>();

    public List<Card> playerDeck = new List<Card>();
    public delegate void DamagedCard(Player player, int cardIndex, bool destroyed);

    public static DamagedCard OnCardDamage;

    public static int MAX_CARDS_IN_HAND = 8;

    public int currentTurnDefenseValue;
    #endregion


    public void ShuffleDeck()
    {
        DeckUtilities.Shuffle<Card>(playerDeck);
    }

    // Damage cards owned
    public void TakeDamage(int _damage)
    {
        currentTurnDefenseValue -= _damage;
        if (currentTurnDefenseValue < 0)
        {
            int damageOnCards = -currentTurnDefenseValue;
            currentTurnDefenseValue = 0;

            // TODO: damage cards
            int damagedCardIndex = Random.Range(0, playerCards.Count);
            playerCards[damagedCardIndex].CardHealth -= damageOnCards;
            if(playerCards[damagedCardIndex].CardHealth == 0)
            {
                playerCards.RemoveAt(damagedCardIndex);
                if (OnCardDamage != null)
                    OnCardDamage(this, damagedCardIndex, true);
            }
            else
            {
                if (OnCardDamage != null)
                    OnCardDamage(this, damagedCardIndex, false);
            }
        }
    }


    public bool DrawNextCard()
    {
        if (playerDeck.Count <= 0)
            return false;

        if (playerCards.Count >= MAX_CARDS_IN_HAND)
            return false;

        playerCards.Add(playerDeck[0]);
        playerDeck.RemoveAt(0);
        return true;
    }
}