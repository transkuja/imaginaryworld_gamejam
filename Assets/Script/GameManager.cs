using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;


public enum AIType { Bully, Nerd, Prof }
public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public LayerMask layerMask;
    public Texture2D icon;
    public SpriteUtils SpriteUtils;

    public List<CardInstance> selectedCards;
    public Dictionary<CardInstance, int> dictionarySelectedCardsValues;

    private PlayerInstance currentPlayer;
    private EnemyInstance currentEnemy;

    private bool clickOnValue = false;

    public PlayerInstance CurrentPlayer
    {
        get
        {
            return currentPlayer;
        }

        set
        { 
            currentPlayer = value;
            if (CurrentEnemy)
            {
                BattleHandler.StartBattle(currentPlayer.playerData, currentEnemy.enemyData);
            }
                 
        }
    }

    public EnemyInstance CurrentEnemy
    {
        get
        {
            return currentEnemy;
        }

        set
        {
            currentEnemy = value;
            if (currentPlayer)
            {
                BattleHandler.StartBattle(currentPlayer.playerData, currentEnemy.enemyData);
            }
        }
    }

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        instance.selectedCards = new List<CardInstance>();
        instance.dictionarySelectedCardsValues = new Dictionary<CardInstance, int>();
        //Cursor.SetCursor(icon, Vector2.zero, CursorMode.Auto);
    }

    public void Update()
    {
        if( BattleHandler.currentTurn == BattleHandler.EntityTurn.Player)
            MouseControls();
        if (BattleHandler.currentTurn == BattleHandler.EntityTurn.AI)
            HandleAI();
    }

    public void MouseControls()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                RaycastHit2D hitInfo;
                clickOnValue = false;
                hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.transform.forward * 0.01f, Camera.main.transform.forward, ~layerMask);
                if (hitInfo)
                {
    
                    if (hitInfo.transform.tag == "value" && hitInfo.transform.gameObject.GetComponentInParent<CardInstance>().IsReady && selectedCards.Contains(hitInfo.transform.gameObject.GetComponentInParent<CardInstance>()))
                    {
                        clickOnValue = true;
                        CardInstance cardInstance = hitInfo.transform.gameObject.GetComponentInParent<CardInstance>();
                        bool alreadySelected = false;

                        if (instance.dictionarySelectedCardsValues.ContainsKey(cardInstance))
                        {
                            if (hitInfo.transform.GetComponent<Outline>().effectColor == Color.red)
                            {
                                alreadySelected = true;
                            }

                            cardInstance.IsLock = false;
                            instance.dictionarySelectedCardsValues.Remove(cardInstance);
                            for (int i = 0; i < cardInstance.transform.childCount; i++)
                            {
                                cardInstance.transform.GetChild(i).GetComponent<Outline>().effectColor = Color.white;
                            }

                        }

                        if (!alreadySelected)
                        {
                            instance.dictionarySelectedCardsValues.Add(cardInstance, Convert.ToInt32(hitInfo.transform.GetComponent<Text>().text));
                            cardInstance.IsLock = true;
                            hitInfo.transform.GetComponent<Outline>().effectColor = Color.red;

                            // Button Fight
                            if(instance.dictionarySelectedCardsValues.Count == 3)
                            {
                                UIManager.instance.buttonFight.GetComponent<Button>().interactable = true;
                            } else
                            {
                                UIManager.instance.buttonFight.GetComponent<Button>().interactable = false;
                            }
                            
                        }


                    }
                }

                hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.transform.forward * 0.01f, Camera.main.transform.forward, layerMask);
                if (!clickOnValue && hitInfo)
                {
                    if (hitInfo.transform.tag == "card" && hitInfo.transform.gameObject.GetComponent<CardInstance>().IsReady)
                    {
                        CardInstance cardInstance = hitInfo.transform.gameObject.GetComponent<CardInstance>();
                        if (!selectedCards.Contains(cardInstance))
                        {
                            for (int i =0; i < cardInstance.transform.parent.childCount; i++)
                            {

                                if(i != cardInstance.transform.GetSiblingIndex())
                                {
                                    if (instance.selectedCards.Contains(cardInstance.transform.parent.GetChild(i).GetComponent<CardInstance>()))
                                    {
                                        if (!cardInstance.transform.parent.GetChild(i).GetComponent<CardInstance>().IsLock)
                                        {
                                            instance.selectedCards.Remove(cardInstance.transform.parent.GetChild(i).GetComponent<CardInstance>());
                                            cardInstance.transform.parent.GetChild(i).GetComponent<CardInstance>().transform.parent.GetChild(i).transform.localPosition -= new Vector3(0, 60f, 0);

                                        }
                                   
                                    }
                                }
                            }


                            instance.selectedCards.Add(cardInstance);
                            hitInfo.transform.localPosition += new Vector3(0, 60f, 0);
                        }
                        else
                        {
                            instance.selectedCards.Remove(cardInstance);
                            hitInfo.transform.localPosition -= new Vector3(0, 60f, 0);

                            instance.dictionarySelectedCardsValues.Remove(cardInstance);
                            for (int i = 0; i < cardInstance.transform.childCount; i++)
                            {
                                cardInstance.transform.GetChild(i).GetComponent<Outline>().effectColor = Color.white;
                            }

                        }

                        if (instance.dictionarySelectedCardsValues.Count == 3)
                        {
                            UIManager.instance.buttonFight.GetComponent<Button>().interactable = true;
                        }
                        else
                        {
                            UIManager.instance.buttonFight.GetComponent<Button>().interactable = false;
                        }
                    }
                }
            }

        }

    }

    public void HandleAI()
    {
        List<Card> selectedCardData = new List<Card>();
        List<Card> copyList = currentEnemy.enemyData.playerCards;
        switch (currentEnemy.typeOfAI)
        {
            case (AIType.Bully):
            case (AIType.Nerd):
            case (AIType.Prof):
            default:
                Debug.Log("Using random intelligence on an AI");
                for (int i = 0; i < 4; i++)
                {
                    int randomCard = Random.Range(0, copyList.Count-1);
                    copyList.RemoveAt(randomCard);
                    // Random Intelligence
                    int randomCombinaison = Random.Range(0, 4);
                    currentEnemy.enemyData.playerCards[i].combinationPlayed = currentEnemy.enemyData.playerCards[randomCard].combinationValues[randomCombinaison];
                    selectedCardData.Add(currentEnemy.enemyData.playerCards[i]);
                }
                break;
        }
   
        BattleHandler.SendCardSelection(selectedCardData);
        BattleHandler.CardResolution();
        BattleHandler.NextTurn();
    }

}
