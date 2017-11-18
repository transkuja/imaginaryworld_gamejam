using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardInstance : MonoBehaviour {

    #region Properties
    private Card cardData;

    public bool IsReady = false;
    public bool IsSelected = false;

    public bool isHidden = false;

    private Image refImage;

    public Card CardData
    {
        get
        {
            return cardData;
        }

        set
        {
            if(cardData == null && value != null)
            {
                if (!refImage)
                    refImage = GetComponent<Image>();
                if (value.GetType() == typeof(SwordCard))
                {
                    refImage.sprite = GameManager.instance.SpriteUtils.swordSprite;
                }
                else if(value.GetType() == typeof(ShieldCard))
                {
                    refImage.sprite = GameManager.instance.SpriteUtils.shieldSprite;
                }
            }
            cardData = value;
        }
    }

    public bool IsHidden
    {
        get
        {
            return isHidden;
        }

        set
        {
            //If we need to hide the card, change the sprite and hide the numbers
            if(value)
            {
                if (!refImage)
                    refImage = GetComponent<Image>();
                refImage.sprite = GameManager.instance.SpriteUtils.hiddenSprite;
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(false);
                transform.GetChild(2).gameObject.SetActive(false);
                transform.GetChild(3).gameObject.SetActive(false);
            }
            else
            {
                if(cardData != null)
                {
                    if (!refImage)
                        refImage = GetComponent<Image>();
                    if (cardData.GetType() == typeof(SwordCard))
                    {
                        refImage.sprite = GameManager.instance.SpriteUtils.swordSprite;
                    }
                    else if (cardData.GetType() == typeof(ShieldCard))
                    {
                        refImage.sprite = GameManager.instance.SpriteUtils.shieldSprite;
                    }
                    transform.GetChild(0).gameObject.SetActive(true);
                    transform.GetChild(1).gameObject.SetActive(true);
                    transform.GetChild(2).gameObject.SetActive(true);
                    transform.GetChild(3).gameObject.SetActive(true);
                }
            }
            isHidden = value;
        }
    }
    #endregion

    public void Start()
    {
        refImage = GetComponent<Image>();
        //Allows to run the code in the IsHidden setter
        IsHidden = isHidden;
    }

    public void Update()
    {

    }


    public void ChangeShaderMask(int maskIndice)
    {
        // Do nothing for now
    }

    public void RefreshValues()
    {
        for (int i = 0; i < CardData.combinationValues.Length; i++)
        {
            gameObject.transform.GetChild(i).GetComponent<Text>().text = "" + CardData.combinationValues[i];
        } 

    }
}
