using UnityEngine;

public class Card {
    
    #region Properties
    private int cardHealth;

    public int[] combinationValues = new int[2];
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
                Debug.LogError("Card >> Card Health : negative value");
            
            // Affectation
            cardHealth = value;

        }
    }
    #endregion



    public void HandleCardHealth(int _newValue)
    {
        cardHealth += _newValue;
    }


}
