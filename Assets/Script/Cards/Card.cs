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


    public Card(int value1, int value2, int value3, int value4)
    {
        combinationValues[0] = value1;
        combinationValues[1] = value2;
        combinationValues[2] = value3;
        combinationValues[3] = value4;
    }

    public void HandleCardHealth(int _newValue)
    {
        cardHealth += _newValue;
    }


}
