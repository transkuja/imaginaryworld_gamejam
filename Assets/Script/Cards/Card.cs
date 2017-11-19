using UnityEngine;

public class Card {
    
    #region Properties
    private int cardHealth;

    public int[] combinationValues = new int[4];
    public const int MIN_COMBINATION_VALUE = 1;
    public const int MAX_COMBINATION_VALUE = 9;

    public int combinationPlayed;
    #endregion


    virtual public void Use() { }


    #region Accessors
    public int CardHealth
    {
        get
        {
            return cardHealth;
        }

        set
        {
            // Control
            if (value < 0)
            {
                //Debug.LogError("Card >> Card Health : negative value");
                cardHealth = 0;
            }
            else
                // Affectation
                cardHealth = value;
        }
    }
    #endregion


    public Card(int value1, int value2, int value3, int value4, int health = 3)
    {
        combinationValues[0] = value1;
        combinationValues[1] = value2;
        combinationValues[2] = value3;
        combinationValues[3] = value4;
        cardHealth = health;
    }

    public Card(Card c)
    {
        combinationValues[0] = c.combinationValues[0];
        combinationValues[1] = c.combinationValues[1];
        combinationValues[2] = c.combinationValues[2];
        combinationValues[3] = c.combinationValues[3];
        cardHealth = c.cardHealth;
    }

    public void HandleCardHealth(int _newValue)
    {
        cardHealth += _newValue;
    }


}
