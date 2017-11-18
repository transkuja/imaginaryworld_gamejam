using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{

    #region Properties

    public List<Card> playerCards = new List<Card>();

    public List<Card> playerDeck = new List<Card>();

    public int MAX_CARDS_IN_HAND = 8;
    #endregion

    public int currentTurnDefenseValue;

    // Damage cards owned
    public void TakeDamage(int _damage)
    {
        currentTurnDefenseValue -= _damage;
        if (currentTurnDefenseValue < 0)
        {
            int damageOnCards = -currentTurnDefenseValue;
            currentTurnDefenseValue = 0;

            // TODO: damage cards
        }
    }
}