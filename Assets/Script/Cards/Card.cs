using UnityEngine;

public class Card : MonoBehaviour {
    
    #region Properties
    private int cardHealth;
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

        // After effect
        ChangeShaderMask(cardHealth);
    }

    public void ChangeShaderMask(int maskIndice)
    {
        // Do nothing for now
    }
}
