using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public List<Card> playerCards = new List<Card>();
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
